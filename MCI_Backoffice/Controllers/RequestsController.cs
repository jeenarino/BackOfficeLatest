using CheckinPortal.BackOffice.BusinessLogic;
using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Models;
using CheckinPortal.BackOffice.Models.SaavyModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class RequestsController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();
        // GET: Requests

        public ActionResult Index(bool Status = false,string Type="All")
        {
            var RequestList = db.Usp_GetRequestDetails(Status, null,null).Where(x=>x.RequestType == Type || Type == "All").ToList();
            int position = 0;
            foreach(DataAccess.Usp_GetRequestDetails_Result result in RequestList)
            {
                var reservationList = db.usp_GetReservationListDetails(result.ReservationDetailID).ToList();
                if(reservationList != null)
                {
                    var reservation = reservationList.FirstOrDefault();
                    RequestList[position].ArrivalDate = reservation.ArrivalDate;
                    RequestList[position].DepartureDate = reservation.DepartureDate;
                }
                position++;
            }

            List<SelectListItem> requestStatus = new List<SelectListItem>();

            requestStatus.Add(new SelectListItem()
            {
                Text = "Pending",
                Value = "false"
            });

            requestStatus.Add(new SelectListItem()
            {
                Text = "Closed",
                Value = "true"
            });

            
            ViewBag.RequestStatus = new SelectList(requestStatus,"Value","Text", Status);
            ViewBag.RequestType = Type;


            return View(RequestList);
        }
        public ActionResult Details(int RequestDetailID)
        {
            var Requestdetails = db.Usp_GetRequestDetails(null,RequestDetailID,null).FirstOrDefault();

            if(Requestdetails != null)
            {
                var ReservationDetails = db.usp_GetReservationListDetails(Requestdetails.ReservationDetailID).FirstOrDefault();
                ViewBag.ReservationDetails = ReservationDetails;
            }
            return View(Requestdetails);
        }


        public ActionResult UpdateStatus(UpdateRequestSstatusModel updateRequestSstatusModel)
        {
            int userid = -1;
            if(Session["UserID"] != null)
            {
                userid = Convert.ToInt32(Session["UserID"].ToString());
            }

            db.usp_ApproveRequest(updateRequestSstatusModel.ReqDetailId, true, updateRequestSstatusModel.Comments, userid, null);

            var RequestList = db.Usp_GetRequestDetails(false, null,null);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ChargeReservationForPackage(ChargeReservationModel model)
        {

            var Requestdetails = db.Usp_GetRequestDetails(null, model.RequestID, null).FirstOrDefault();
            
            if(Requestdetails != null)
            {
                var Reservation = db.usp_GetReservationListDetails(Requestdetails.ReservationDetailID).FirstOrDefault();

                if(Reservation != null)
                {
                    Helpers.HttpClientHelper httpClientHelper = new Helpers.HttpClientHelper($"{ConfigurationManager.AppSettings["SaavyBaseURL"].ToString()}");

                    var ActiveTransactions = await httpClientHelper.GetDataFromCloud($"/api/services/GetActiveTransaction?ReservationNameID={Reservation.ReservationNameID}");

                    if (ActiveTransactions.IsSuccessStatusCode)
                    {
                        List<FetchPaymentTransactionResult> fetchPaymentTransactionResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FetchPaymentTransactionResult>>(await ActiveTransactions.Content.ReadAsStringAsync());

                        if (fetchPaymentTransactionResult != null && fetchPaymentTransactionResult.Count > 0)
                        {
                            //Transaction found, charge in first tranaction
                            httpClientHelper = new Helpers.HttpClientHelper($"{ConfigurationManager.AppSettings["SaavyBaseURL"].ToString()}");
                            var TopUpResponse =  await httpClientHelper.PushDataToCloud(new { PaymentTransactionID = fetchPaymentTransactionResult[0].PaymentID, Amount = Requestdetails.PackageAmount }, "/api/services/TopupReservation");

                            if(TopUpResponse != null && TopUpResponse.IsSuccessStatusCode)
                            {
                                var topupResponseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<SaavyAPIResponseModel>(await TopUpResponse.Content.ReadAsStringAsync());

                                if(topupResponseModel != null) 
                                {
                                    return Json(topupResponseModel);
                                }
                                else
                                {
                                    return Json(new { result = false, Message = "Unable to charge the transaction" });
                                }
                            }
                            else
                            {
                                return Json(new { result = false, Message = "Unable to charge the transaction" });
                            }
                        }
                        else
                        {
                            return Json(new { result = false, Message = "Active Transactions Not found" });
                        }
                    }
                    else
                    {
                        return Json(new { result = false, Message = "Active payment transactions not found" });
                    }
                }
                else
                {
                    return Json(new { result = false, Message = "Unable to verify reservations details" });
                }
            }
            else
            {
                return Json(new { result = false, Message = "Unable to find the requset details" });
            }
        }

        public async Task<ActionResult> AcceptRequest(AccpetRejectRequestModel model)
        {
            BusinessLogic.LocalAPICallsHelper localAPIHelper = new BusinessLogic.LocalAPICallsHelper();
            var config = Helpers.ConfigurationHelper.Instance.OWSConfig;
            var reservation = db.usp_GetReservationListDetails(model.ReservationDetailID).Take(1).FirstOrDefault();
            if (reservation != null)
            {
                //Pull reservation from Operea and update it in local.
                ReservationLogics reservationLogics = new ReservationLogics();

                await reservationLogics.syncReservationFromOpera(reservation.ReservationNumber);
                
                var profileList = db.Usp_GetProfileInformationByReservationID(reservation.ReservationDetailID).ToList();

                if (profileList != null && profileList.Count > 0)
                {
                    var profile = profileList.FirstOrDefault();

                    var requestDetails = db.tbRequestDetails.Where(x => x.ReqDetailId == model.ReqDetailId).FirstOrDefault();

                    if (requestDetails != null)
                    {
                        SendEmailRequestModel sendEmailRequestModel = new SendEmailRequestModel()
                        {
                            AttchmentBase64 = string.Empty,
                            confirmationNumber = reservation.ReservationNumber,
                            EmailType = EmailType.AcceptRequest,
                            fromEmail = config.FromEmail,
                            ToEmail = profile.Email,
                            displayFromEmail = config.DisplayFromEmail,
                            subject = config.RequestAcceptedSubject,
                            guestName = $"{profile.FirstName} {profile.MiddleName} {profile.LastName}",
                            ItemName = requestDetails.PackageName,
                            TotalAmount = requestDetails.PackageAmount != null ? requestDetails.PackageAmount.Value.ToString("0.00") : "0.00"
                        };

                        var owsResponse = await localAPIHelper.SendEmail(sendEmailRequestModel);

                        if (owsResponse != null)
                        {
                            if (owsResponse.result)
                            {
                                int userid = -1;
                                if (Session["UserID"] != null)
                                {
                                    userid = Convert.ToInt32(Session["UserID"].ToString());
                                }

                                db.usp_ApproveRequest(model.ReqDetailId, true, model.Comments, userid, true);

                                return Json(new { result = owsResponse.result, message = owsResponse.responseMessage });
                            }
                            else
                            {
                                return Json(new { result = false, message = owsResponse.responseMessage });
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = "Unable to send email" });
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = "Unable to send email, Request informations not found" });
                    }
                }
                else
                {
                    return Json(new { result = false, message = "Unable to send email, couldn't read profile information" });
                }
            }
            else
            {
                return Json(new { result = false, message = "Unable to send email, Unable to fetch the reservations details" });
            }
        }

        public async Task<ActionResult> RejectRequest(AccpetRejectRequestModel model)
        {
            BusinessLogic.LocalAPICallsHelper localAPIHelper = new BusinessLogic.LocalAPICallsHelper();
            var config = Helpers.ConfigurationHelper.Instance.OWSConfig;
            var reservation = db.usp_GetReservationListDetails(model.ReservationDetailID).Take(1).FirstOrDefault();
            if (reservation != null)
            {
                //Pull reservation from Operea and update it in local.
                ReservationLogics reservationLogics = new ReservationLogics();

                await reservationLogics.syncReservationFromOpera(reservation.ReservationNumber);

                var profileList = db.Usp_GetProfileInformationByReservationID(reservation.ReservationDetailID).ToList();

                if (profileList != null && profileList.Count > 0)
                {
                    var profile = profileList.FirstOrDefault();

                    var requestDetails = db.tbRequestDetails.Where(x => x.ReqDetailId == model.ReqDetailId).FirstOrDefault();

                    if (requestDetails != null)
                    {
                        SendEmailRequestModel sendEmailRequestModel = new SendEmailRequestModel()
                        {
                            AttchmentBase64 = string.Empty,
                            confirmationNumber = reservation.ReservationNumber,
                            EmailType = EmailType.RejectRequest,
                            fromEmail = config.FromEmail,
                            ToEmail = profile.Email,
                            displayFromEmail = config.DisplayFromEmail,
                            subject = config.RequestRejectedSubject,
                            guestName = $"{profile.FirstName} {profile.MiddleName} {profile.LastName}",
                            ItemName = requestDetails.PackageName,
                            TotalAmount = requestDetails.PackageAmount != null ? requestDetails.PackageAmount.Value.ToString("0.00") : "0.00"
                        };

                        var owsResponse = await localAPIHelper.SendEmail(sendEmailRequestModel);

                        if (owsResponse != null)
                        {
                            if (owsResponse.result)
                            {
                                int userid = -1;
                                if (Session["UserID"] != null)
                                {
                                    userid = Convert.ToInt32(Session["UserID"].ToString());
                                }

                                db.usp_ApproveRequest(model.ReqDetailId, true, model.Comments, userid, false);

                                return Json(new { result = owsResponse.result, message = owsResponse.responseMessage });
                            }
                            else
                            {
                                return Json(new { result = false, message = owsResponse.responseMessage });
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = "Unable to send email" });
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = "Unable to send email, Request informations not found" });
                    }
                }
                else
                {
                    return Json(new { result = false, message = "Unable to send email, couldn't read profile information" });
                }
            }
            else
            {
                return Json(new { result = false, message = "Unable to send email, Unable to fetch the reservations details" });
            }
        }

        
    }
}