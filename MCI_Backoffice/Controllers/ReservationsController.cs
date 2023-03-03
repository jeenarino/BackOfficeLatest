using CheckinPortal.BackOffice.BusinessLogic;
using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Models;
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CheckinPortal.BackOffice.Controllers
{
    public class ReservationsController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();
        // GET: Reservations
        public ActionResult Index(string ReservationGroup = "Normal")
        {
            //var reservationList = db.usp_GetReservationList(ReservationGroup,"",).ToList();
            ViewBag.ReservationGroup = ReservationGroup;
            return View();
        }

        public ActionResult ReservationDetailsNotFound()
        {
            return View();
        }

        public ActionResult GetReservationListAjax(DataTableParameters model, Search search, string ReservationGroup = "PreCheckin")
        
        {
            int start = 0;

            if(model.Start > 0)
            {
               start =  model.Start / model.Length;
            }

            start += 1;

            string filterby = string.Empty;
            string soryOrder = "DEC";
            string sortBy = "";
            string sortColumn = "";

            if (Request.Params["search[value]"] != null)
            {
                filterby = Request.Params["search[value]"].ToString();
            }

            if (Request.Params["order[0][column]"] != null)
            {
                sortBy = Request.Params["order[0][column]"].ToString();
            }

            if(sortBy == "0")
            {
                sortColumn = "Confirmation No";
            }
            else if (sortBy == "1")
            {
                sortColumn = "Room No";
            }
            else if (sortBy == "2")
            {
                sortColumn = "Guest Full Name";
            }
            else if (sortBy == "3")
            {
                sortColumn = "Arrival Date";
            }
            else if (sortBy == "4")
            {
                sortColumn = "Departure Date";
            }



            if (Request.Params["order[0][dir]"] != null)
            {
                soryOrder = Request.Params["order[0][dir]"].ToString();
            }

            var reservationList = db.usp_GetReservationList(ReservationGroup, "", start, model.Length, filterby, soryOrder, sortColumn).ToList();

            var TotalCount = db.usp_GetReservationListCount(ReservationGroup,"", null).FirstOrDefault();

            var response =new
           {
               draw = model.draw,
               data = reservationList,
               recordsFiltered = TotalCount != null ? TotalCount.Value : reservationList.Count,
               recordsTotal = TotalCount !=null ? TotalCount.Value : reservationList.Count
            };

            return Json(response, JsonRequestBehavior.AllowGet);



        }

        public ActionResult OpenSaavyPay(int? reservationID)
        {
            
            if (!string.IsNullOrEmpty(Session["UserName"].ToString()))
            {
                //if(Response.Cookies.AllKeys.Contains("Saavy"))
                //{
                //    Response.Cookies.Remove("Saavy");
                //}

                if (Request.Cookies.AllKeys.Contains("Saavy"))
                {
                    Request.Cookies.Remove("Saavy");
                }

                Response.Cookies["Saavy"].Value = new MCIGrabberService.Helper.UtilityHelper().EncryptString("b14ca5898a4e4133bbce2ea2315a1916", Session["UserName"].ToString());
                Response.Cookies["Saavy"].Expires = DateTime.Now.AddMinutes(10);


                var result = new MCIGrabberService.Helper.UtilityHelper().GetDomainNameOfUrlString(System.Configuration.ConfigurationManager.AppSettings["SaavyBaseURL"].ToString());
                
                {
                    Response.Cookies["Saavy"].Domain = result;
                }
                
                Response.Cookies["Saavy"].Path = "/";
            }

            if (reservationID  != null)
            {
                var reservationList = db.usp_GetReservationListDetails(reservationID).ToList();

                if (reservationList != null)
                {
                    var reservation = reservationList.FirstOrDefault();
                    if(reservation != null)
                    {
                        var x = $"{ System.Configuration.ConfigurationManager.AppSettings["SaavyBaseURL"].ToString()}/Reservations/Details?ReservationNameID={reservation.ReservationNameID}&UID={Session["UserName"]}";
                        Response.Redirect(@x);
                        return null;
                    }
                }
            }

            return ReservationDetailsNotFound();


        }




        public async Task<ActionResult> Details(int ReservationID, int GuestIndex = 0)
        {
            BusinessLogic.ReservationLogics reservationLogics = new BusinessLogic.ReservationLogics();
            ViewBag.GuestIndex = GuestIndex;
            var reservationList = db.usp_GetReservationListDetails(ReservationID).ToList();

            if (reservationList != null)
            {
                var reservation = reservationList.Skip(GuestIndex).Take(1).FirstOrDefault();
                BusinessLogic.OwsHelper owsHelper = new BusinessLogic.OwsHelper();

                if (reservation != null)
                {
                    bool syncResult = false;
                    //Sync the reservation
                    if (reservation.ReservationSource != "CHECKEDOUT")
                    {
                        syncResult = await reservationLogics.syncReservationFromOpera(reservation.ReservationNumber);
                        
                        reservationList = db.usp_GetReservationListDetails(ReservationID).ToList();
                        if (reservationList != null)
                        {
                            reservation = reservationList.Skip(GuestIndex).Take(1).FirstOrDefault();
                        }
                    }

                    ViewBag.RoomNotFoundMessage = string.Empty;
                    ViewBag.IsRoomAssigned = true;
                    bool EnableAutoRoomAssign = false;

                    try
                    {
                        EnableAutoRoomAssign = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAutoRoomAssign"].ToString());
                    }
                    catch 
                    {

                    }

                    if (string.IsNullOrEmpty(reservation.RoomNumber) && EnableAutoRoomAssign && syncResult)
                    {
                        ViewBag.IsRoomAssigned = false;

                        if (!string.IsNullOrEmpty(reservation.RoomType))
                        {

                            #region Fetch Room List from OS
                            var FetchRoomResponse = await owsHelper.GetOperaRoomListByRoomType(reservation.DepartureDate.Value, reservation.RoomType);

                            if (FetchRoomResponse != null && FetchRoomResponse.responseData != null)
                            {
                                if (FetchRoomResponse.responseData.Count > 0)
                                {
                                    //Auto Assign Room
                                    BusinessLogic.OwsHelper localAPIHelper = new BusinessLogic.OwsHelper();
                                    //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Auto Assigning Room : {FetchRoomResponse.responseData[0].RoomNumber}" });
                                    var owsResponse = await localAPIHelper.AssignRoom(FetchRoomResponse.responseData[0].RoomNumber, reservation.ReservationNameID);
                                    if (owsResponse != null)
                                    {
                                        //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Auto Assigning Room Success" });
                                        ViewBag.IsRoomAssigned = true;
                                        ViewBag.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                        reservation.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                        var roomUpdateReservaiton = db.tbReservationDetails.Where(x => x.ReservationDetailID == reservation.ReservationDetailID).FirstOrDefault();
                                        if (roomUpdateReservaiton != null)
                                        {
                                            roomUpdateReservaiton.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                            db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Auto Assigning Failed" });

                                        ViewBag.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                        reservation.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                        ViewBag.RoomNotFoundMessage = string.Empty;
                                    }
                                }
                                else
                                {
                                    //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Room list not available for auto assign" });
                                    ViewBag.RoomNotFoundMessage = FetchRoomResponse.responseMessage;
                                }
                            }
                            else
                            {
                                //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Room list not available for auto assign" });
                                ViewBag.RoomNotFoundMessage = FetchRoomResponse.responseMessage;
                            }
                            #endregion
                        }

                    }
                    //Get Reservation document for primary guest
                    var reservationIDDocument = db.usp_GetReservationListDocumentsDetails(ReservationID).Skip(GuestIndex).Take(1).FirstOrDefault();
                   
                    ViewBag.reservationDocumnent = reservationIDDocument;


                    string path1 = Server.MapPath($"~/temp/{GuestIndex}_1_{reservation.ReservationNumber}.jpeg");
                    string path2 = Server.MapPath($"~/temp/{GuestIndex}_2_{reservation.ReservationNumber}.jpeg");
                    string path3 = Server.MapPath($"~/temp/{GuestIndex}_3_{reservation.ReservationNumber}.jpeg");


                    if (reservationIDDocument.DocumentImage1 != null)
                    {
                        System.IO.File.WriteAllBytes(path1, reservationIDDocument.DocumentImage1);
                    }

                    if (reservationIDDocument.DocumentImage2 != null)
                    {
                        System.IO.File.WriteAllBytes(path2, reservationIDDocument.DocumentImage2);
                    }

                    if (reservationIDDocument.DocumentImage3 != null)
                    {
                        System.IO.File.WriteAllBytes(path3, reservationIDDocument.DocumentImage3);
                    }

                    ViewBag.DocumentPath1 = $"{GuestIndex}_1_{reservation.ReservationNumber}.jpeg";
                    ViewBag.DocumentPath2 = $"{GuestIndex}_2_{reservation.ReservationNumber}.jpeg";
                    ViewBag.DocumentPath3 = $"{GuestIndex}_3_{reservation.ReservationNumber}.jpeg";


                    //Get Registration card and folio for reservation
                    var RegCardFolio = db.usp_GetReservationDocuments(ReservationID).ToList();
                    if(RegCardFolio != null && RegCardFolio.Count > 0)
                        ViewBag.RegCardFolio = RegCardFolio;

                    //get Declaration Question for Reservation
                    var feedBackList = db.Usp_FeedbackData(ReservationID).ToList();
                    if (feedBackList != null && feedBackList.Count > 0)
                        ViewBag.FeedBackData = feedBackList;
                    var swapProfileItems = new List<SelectListItem>();
                    int i = 1;

                    

                    foreach (var item in reservationList)
                    {
                       // System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Swap Profile {item.ProfileDetailID} with {reservation.ProfileDetailID}" });
                        
                        string GuestName = "Primary Guest";                        
                        if (item.ProfileDetailID != reservation.ProfileDetailID)
                        {
                            if (i > 1)
                            {
                                GuestName = $"Accompany Guest {i - 1}";
                            }

                            swapProfileItems.Add(new SelectListItem()
                            {
                                Text = GuestName,
                                Value = item.ProfileDetailID.ToString()
                            });
                        }
                        i++;
                    }

                    if(swapProfileItems != null && swapProfileItems.Count > 0)
                        ViewBag.SwapProfileList = new SelectList(swapProfileItems, "Value", "Text");

                    var countryMaster = db.tbCountryMasters.ToList();
                    var documentType =  db.tbDocumentMasters.ToList();
                    var stateMaster = db.tbStateMasters.ToList();

                    if (string.IsNullOrEmpty(reservation.Gender))
                    {
                        reservation.Gender = string.Empty;
                    }

                    List<SelectListItem> GenderSelectList = new List<SelectListItem>()
                    {
                        new SelectListItem(){ Text ="Male",Value="Male"},
                        new SelectListItem(){ Text = "Female",Value="Female"},
                        new SelectListItem(){ Text = "Unknown",Value="Unknown"}
                    };
                    string genselected = null;
                    if (!String.IsNullOrEmpty(reservation.Gender))
                    {

                        if (reservation.Gender.Trim().ToUpper() == "M" || reservation.Gender.Trim().ToUpper() == "MALE")
                            genselected = "Male";
                        if (reservation.Gender.Trim().ToUpper() == "F" || reservation.Gender.Trim().ToUpper() == "FEMALE")
                            genselected = "Female";
                        if (reservation.Gender.Trim().ToUpper() == "U" || reservation.Gender.Trim().ToUpper() == "UNKNOWN")
                            genselected = "Unknown";
                    }
                    ViewBag.GenderList = new SelectList(GenderSelectList, "Value", "Text", genselected);
                    ViewBag.CountryList = new SelectList(countryMaster, "CountryMasterID", "Country_Full_name", reservation.CountryMasterID);
                    ViewBag.StateList = new SelectList(stateMaster, "StateMasterID", "StateName", reservation.StateMasterID);
                    ViewBag.NationalityList = new SelectList(countryMaster, "CountryMasterID", "Country_Full_name", reservation.NationalityID);
                    ViewBag.IssueCountryList = new SelectList(countryMaster, "Country_2Char_code", "Country_Full_name", reservation.IssueCountry);
                    ViewBag.DocumentTypeList = new SelectList(documentType, "DocumentType", "DocumentType", reservation.DocumentType);

                    var RequestList = db.Usp_GetRequestDetails(null, null, ReservationID).ToList();
                    if (RequestList != null && RequestList.Count > 0)
                        //get Declaration Question for Reservation
                        ViewBag.RequestList = RequestList;

                    var result = new Helpers.DapperHelper().ExecuteSP("Usp_GetReservationAdditionalDetails", new { ReservationNumber = reservation.ReservationNumber});
                    if(result != null)
                    {
                        
                        ViewBag.additionalFields = result.ToList<dynamic>();
                    }
                    reservation.Gender = genselected;
                    string[] ETA = reservation.ETA.Split(':');
                    if(ETA.Length > 0)
                    {
                        reservation.ETA = ETA[0] + ":" + ETA[1];
                    }
                    return View(reservation);
                }
                else
                {
                    //var reservationList1 = db.usp_GetReservationList("PreCheckin","",1,25, null,null,null).ToList();
                    ViewBag.ReservationGroup = "PreCheckin";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.ReservationGroup = "PreCheckin";
                return View("Index");
            }
        }

        
        public async Task<ActionResult> ReservationDetails(string ConfirmationNumber, int GuestIndex = 0)
        {
            BusinessLogic.ReservationLogics reservationLogics = new BusinessLogic.ReservationLogics();
            ViewBag.GuestIndex = GuestIndex;


            var ReservationIDString = db.tbReservationDetails.Where(x=>x.ReservationNumber == ConfirmationNumber).FirstOrDefault();

            System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Reservation Details ID: {ReservationIDString}" });

            var s=ReservationIDString.ReservationNameID;


            if (ReservationIDString != null)
            {
                int ReservationID = Convert.ToInt32(ReservationIDString.ReservationDetailID);
                var reservationList = db.usp_GetReservationListDetails(ReservationID).ToList();

                if (reservationList != null)
                {
                    var reservation = reservationList.Skip(GuestIndex).Take(1).FirstOrDefault();
                    BusinessLogic.OwsHelper owsHelper = new BusinessLogic.OwsHelper();

                    if (reservation != null)
                    {
                        bool syncResult = false;
                        //Sync the reservation
                        if (reservation.ReservationSource != "CHECKEDOUT")
                        {
                            syncResult = await reservationLogics.syncReservationFromOpera(reservation.ReservationNumber);

                            reservationList = db.usp_GetReservationListDetails(ReservationID).ToList();
                            if (reservationList != null)
                            {
                                reservation = reservationList.Skip(GuestIndex).Take(1).FirstOrDefault();
                            }
                        }

                        ViewBag.RoomNotFoundMessage = string.Empty;
                        ViewBag.IsRoomAssigned = true;
                        bool EnableAutoRoomAssign = false;

                        try
                        {
                            EnableAutoRoomAssign = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAutoRoomAssign"].ToString());
                        }
                        catch
                        {

                        }

                        if (string.IsNullOrEmpty(reservation.RoomNumber) && EnableAutoRoomAssign && syncResult)
                        {
                            ViewBag.IsRoomAssigned = false;

                            if (!string.IsNullOrEmpty(reservation.RoomType))
                            {

                                #region Fetch Room List from OS
                                var FetchRoomResponse = await owsHelper.GetOperaRoomListByRoomType(reservation.DepartureDate.Value, reservation.RoomType);

                                if (FetchRoomResponse != null && FetchRoomResponse.responseData != null)
                                {
                                    if (FetchRoomResponse.responseData.Count > 0)
                                    {
                                        //Auto Assign Room
                                        BusinessLogic.OwsHelper localAPIHelper = new BusinessLogic.OwsHelper();
                                        //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Auto Assigning Room : {FetchRoomResponse.responseData[0].RoomNumber}" });
                                        var owsResponse = await localAPIHelper.AssignRoom(FetchRoomResponse.responseData[0].RoomNumber, reservation.ReservationNameID);
                                        if (owsResponse != null)
                                        {
                                            //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Auto Assigning Room Success" });
                                            ViewBag.IsRoomAssigned = true;
                                            ViewBag.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                            reservation.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                            var roomUpdateReservaiton = db.tbReservationDetails.Where(x => x.ReservationDetailID == reservation.ReservationDetailID).FirstOrDefault();
                                            if (roomUpdateReservaiton != null)
                                            {
                                                roomUpdateReservaiton.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                                db.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Auto Assigning Failed" });

                                            ViewBag.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                            reservation.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                            ViewBag.RoomNotFoundMessage = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Room list not available for auto assign" });
                                        ViewBag.RoomNotFoundMessage = FetchRoomResponse.responseMessage;
                                    }
                                }
                                else
                                {
                                    //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Room list not available for auto assign" });
                                    ViewBag.RoomNotFoundMessage = FetchRoomResponse.responseMessage;
                                }
                                #endregion
                            }

                        }
                        //Get Reservation document for primary guest
                        var reservationIDDocument = db.usp_GetReservationListDocumentsDetails(ReservationID).Skip(GuestIndex).Take(1).FirstOrDefault();
                        ViewBag.reservationDocumnent = reservationIDDocument;


                        string path1 = Server.MapPath($"~/temp/{GuestIndex}_1_{reservation.ReservationNumber}.jpeg");
                        string path2 = Server.MapPath($"~/temp/{GuestIndex}_2_{reservation.ReservationNumber}.jpeg");
                        string path3 = Server.MapPath($"~/temp/{GuestIndex}_3_{reservation.ReservationNumber}.jpeg");


                        if (reservationIDDocument.DocumentImage1 != null)
                        {
                            System.IO.File.WriteAllBytes(path1, reservationIDDocument.DocumentImage1);
                        }

                        if (reservationIDDocument.DocumentImage2 != null)
                        {
                            System.IO.File.WriteAllBytes(path2, reservationIDDocument.DocumentImage2);
                        }

                        if (reservationIDDocument.DocumentImage3 != null)
                        {
                            System.IO.File.WriteAllBytes(path3, reservationIDDocument.DocumentImage3);
                        }

                        ViewBag.DocumentPath1 = $"{GuestIndex}_1_{reservation.ReservationNumber}.jpeg";
                        ViewBag.DocumentPath2 = $"{GuestIndex}_2_{reservation.ReservationNumber}.jpeg";
                        ViewBag.DocumentPath3 = $"{GuestIndex}_3_{reservation.ReservationNumber}.jpeg";


                        //Get Registration card and folio for reservation
                        var RegCardFolio = db.usp_GetReservationDocuments(ReservationID).ToList();
                        if(RegCardFolio != null && RegCardFolio.Count > 0)
                            ViewBag.RegCardFolio = RegCardFolio;

                        //get Declaration Question for Reservation
                        var FeedBackData = db.Usp_FeedbackData(ReservationID).ToList();
                        if (FeedBackData != null && FeedBackData.Count > 0)
                            ViewBag.FeedBackData = FeedBackData;

                        var swapProfileItems = new List<SelectListItem>();
                        int i = 1;



                        foreach (var item in reservationList)
                        {
                            // System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Swap Profile {item.ProfileDetailID} with {reservation.ProfileDetailID}" });

                            string GuestName = "Primary Guest";
                            if (item.ProfileDetailID != reservation.ProfileDetailID)
                            {
                                if (i > 1)
                                {
                                    GuestName = $"Accompany Guest {i - 1}";
                                }

                                swapProfileItems.Add(new SelectListItem()
                                {
                                    Text = GuestName,
                                    Value = item.ProfileDetailID.ToString()
                                });
                            }
                            i++;
                        }
                        if (swapProfileItems != null && swapProfileItems.Count > 0)
                            ViewBag.SwapProfileList = new SelectList(swapProfileItems, "Value", "Text");

                        var countryMaster = db.tbCountryMasters.ToList();
                        var documentType = db.tbDocumentMasters.ToList();
                        var stateMaster = db.tbStateMasters.ToList();

                        if (string.IsNullOrEmpty(reservation.Gender))
                        {
                            reservation.Gender = string.Empty;
                        }

                        List<SelectListItem> GenderSelectList = new List<SelectListItem>()
                    {
                        new SelectListItem(){ Text ="Male",Value="Male"},
                        new SelectListItem(){ Text = "Female",Value="Female"},
                        new SelectListItem(){ Text = "Unknown",Value="Unknown"}
                    };
                        string genselected = null;
                        if (!String.IsNullOrEmpty(reservation.Gender))
                        {

                            if (reservation.Gender.Trim().ToUpper() == "M" || reservation.Gender.Trim().ToUpper() == "MALE")
                                genselected = "Male";
                            if (reservation.Gender.Trim().ToUpper() == "F" || reservation.Gender.Trim().ToUpper() == "FEMALE")
                                genselected = "Female";
                            if (reservation.Gender.Trim().ToUpper() == "U" || reservation.Gender.Trim().ToUpper() == "UNKNOWN")
                                genselected = "Unknown";
                        }
                        ViewBag.GenderList = new SelectList(GenderSelectList, "Value", "Text", genselected);
                        ViewBag.CountryList = new SelectList(countryMaster, "CountryMasterID", "Country_Full_name", reservation.CountryMasterID);
                        ViewBag.StateList = new SelectList(stateMaster, "StateMasterID", "StateName", reservation.StateMasterID);
                        ViewBag.NationalityList = new SelectList(countryMaster, "CountryMasterID", "Country_Full_name", reservation.NationalityID);
                        ViewBag.IssueCountryList = new SelectList(countryMaster, "Country_2Char_code", "Country_Full_name", reservation.IssueCountry);
                        ViewBag.DocumentTypeList = new SelectList(documentType, "DocumentType", "DocumentType", reservation.DocumentType);

                        var RequestList = db.Usp_GetRequestDetails(null, null, ReservationID).ToList();
                        if (RequestList != null && RequestList.Count > 0)
                            //get Declaration Question for Reservation
                            ViewBag.RequestList = RequestList;

                        var result = new Helpers.DapperHelper().ExecuteSP("Usp_GetReservationAdditionalDetails", new { ReservationNumber = reservation.ReservationNumber });
                        if (result != null )
                        {

                            ViewBag.additionalFields = result.ToList<dynamic>();
                        }
                        reservation.Gender = genselected;
                        return View("Details",reservation);
                    }
                    else
                    {
                        ViewBag.ReservationGroup = "PreCheckin";
                        return View("Index");
                    }
                }
                else
                {
                    ViewBag.ReservationGroup = "PreCheckin";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.ReservationGroup = "PreCheckin";
                return View("Index");
            }

        }


        public ActionResult Documents(int ReservationID, int ReservationDocumentID)
        {
            var reservation = db.usp_GetReservationListDetails(ReservationID).Take(1).FirstOrDefault();

            if (reservation != null)
            {
                var RegCardFolio = db.usp_GetReservationDocuments(ReservationID).ToList();
                ViewBag.RegCardFolio = RegCardFolio;
                
                if (RegCardFolio != null)
                {
                    var RegDocument = RegCardFolio.Where(x => x.ReservationdocumentID == ReservationDocumentID).FirstOrDefault();

                    string path = Server.MapPath($"~/temp/{RegDocument.DocumentType}_{reservation.ReservationNumber}.pdf");
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    System.IO.File.WriteAllBytes(path, RegDocument.Document);
                    ViewBag.DocumentType = RegDocument.DocumentType;
                }

                //get Declaration Question for Reservation
                var FeedBackData = db.Usp_FeedbackData(ReservationID).ToList();
                if (FeedBackData != null && FeedBackData.Count > 0)
                    ViewBag.FeedBackData = FeedBackData;

                var RequestList = db.Usp_GetRequestDetails(null, null, ReservationID).ToList();
                if (RequestList != null && RequestList.Count > 0)
                    //get Declaration Question for Reservation
                    ViewBag.RequestList = RequestList;

                var reservationList = db.usp_GetReservationListDetails(ReservationID).ToList();
                if (reservationList != null)
                {
                    reservation = reservationList.Skip(0).Take(1).FirstOrDefault();
                    var swapProfileItems = new List<SelectListItem>();
                    int i = 1;



                    foreach (var item in reservationList)
                    {
                        // System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Swap Profile {item.ProfileDetailID} with {reservation.ProfileDetailID}" });

                        string GuestName = "Primary Guest";
                        if (item.ProfileDetailID != reservation.ProfileDetailID)
                        {
                            if (i > 1)
                            {
                                GuestName = $"Accompany Guest {i - 1}";
                            }

                            swapProfileItems.Add(new SelectListItem()
                            {
                                Text = GuestName,
                                Value = item.ProfileDetailID.ToString()
                            });
                        }
                        i++;
                    }

                    if (swapProfileItems != null && swapProfileItems.Count > 0)
                        ViewBag.SwapProfileList = new SelectList(swapProfileItems, "Value", "Text");

                }

                return View(reservation);
            }
            else
            {
                ViewBag.ReservationGroup = "PreCheckin";
                return View("Index");
            }
        }

        public ActionResult Declarations(int ReservationID)
        {
            var reservation = db.usp_GetReservationListDetails(ReservationID).Take(1).FirstOrDefault();
            if (reservation != null)
            {
                //get Declaration Question for Reservation
                var feedbacklist = db.Usp_FeedbackData(ReservationID).ToList();
                if (feedbacklist != null && feedbacklist.Count > 0)
                    ViewBag.FeedBackData = feedbacklist;
                

                //Get Registration card and folio for reservation
                var RegCardFolio = db.usp_GetReservationDocuments(ReservationID).ToList();
                if(RegCardFolio !=  null && RegCardFolio.Count > 0)
                    ViewBag.RegCardFolio = RegCardFolio;

                var RequestList = db.Usp_GetRequestDetails(null, null, ReservationID).ToList();
                if (RequestList != null && RequestList.Count > 0)
                    //get Declaration Question for Reservation
                    ViewBag.RequestList = RequestList;

                var reservationList = db.usp_GetReservationListDetails(ReservationID).ToList();
                if (reservationList != null)
                {
                    reservation = reservationList.Skip(0).Take(1).FirstOrDefault();
                    var swapProfileItems = new List<SelectListItem>();
                    int i = 1;



                    foreach (var item in reservationList)
                    {
                        // System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Swap Profile {item.ProfileDetailID} with {reservation.ProfileDetailID}" });

                        string GuestName = "Primary Guest";
                        if (item.ProfileDetailID != reservation.ProfileDetailID)
                        {
                            if (i > 1)
                            {
                                GuestName = $"Accompany Guest {i - 1}";
                            }

                            swapProfileItems.Add(new SelectListItem()
                            {
                                Text = GuestName,
                                Value = item.ProfileDetailID.ToString()
                            });
                        }
                        i++;
                    }

                    if (swapProfileItems != null && swapProfileItems.Count > 0)
                        ViewBag.SwapProfileList = new SelectList(swapProfileItems, "Value", "Text");

                }
                return View(reservation);
            }
            else
            {
                ViewBag.ReservationGroup = "PreCheckin";
                return View("Index");
            }
        }

        public ActionResult ViewDocImage(int ReservationID, int DocId, int GuestID)
        {
            var reservation = db.usp_GetReservationListDetails(ReservationID).Take(1).FirstOrDefault();
            var reservationIDDocument = db.usp_GetReservationListDocumentsDetails(ReservationID).Skip(GuestID).Take(1).FirstOrDefault();
            ViewBag.reservationDocumnent = reservationIDDocument;
            string fileName = $"DocImage{GuestID}{DocId}{ReservationID}.jpeg";
            return File(reservationIDDocument.DocumentImage1, "image/jpeg");
        }

        public async Task<ActionResult> CheckForSharerReservation(int ReservationNameID)
        {
            BusinessLogic.OwsHelper owsHelper = new BusinessLogic.OwsHelper();
            System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} CheckForSharerReservation" });
            var localReservation = db.tbReservationDetails.Where(x => x.ReservationNameID == ReservationNameID.ToString()).FirstOrDefault();
            if (localReservation != null)
            {
                System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Local reservation found" });
                var operaReservationObj = await owsHelper.FetchReservation(localReservation.ReservationNumber);

                if (operaReservationObj != null && operaReservationObj.responseData != null)
                {
                    var operaReservation = operaReservationObj.responseData;// Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.OperaReservation>>(operaReservationObj.responseData.ToString());

                    if (operaReservation != null && operaReservation.Count > 0)
                    {
                        if (operaReservation[0].SharerReservations != null && operaReservation[0].SharerReservations.Count > 0)
                        {
                            return Json(new { result = true, sharerExist = true });
                        }
                    }
                    return Json(new { result = true, sharerExist = false });
                }
            }
            return Json(new { result = false, sharerExist = false });
        }

        [HttpPost]
        public async Task<ActionResult> CheckinIntoOpera(int ReservationNameID,bool WithSharer = false)
        {
            BusinessLogic.OwsHelper owsHelper = new BusinessLogic.OwsHelper();

            var localReservation = db.tbReservationDetails.Where(x => x.ReservationNameID == ReservationNameID.ToString()).FirstOrDefault();

            var operaReservationObj = await owsHelper.FetchReservation(localReservation.ReservationNumber);


            if(operaReservationObj!= null && operaReservationObj.responseData != null)
            {
                //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Opera Reservation Fetched : {operaReservationObj.responseData.ToString()}" });

                var operaReservation = operaReservationObj.responseData;// Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.OperaReservation>>(operaReservationObj.responseData.ToString());

                if (operaReservation != null && operaReservation.Count > 0)
                {
                    if(operaReservation[0].ReservationStatus == "DUEIN" || (operaReservation[0].ReservationStatus.Contains("RESERVED") && operaReservation[0].ArrivalDate != null && 
                        operaReservation[0].ArrivalDate.Value.Date == DateTime.Now.Date))
                    {
                        var owsResponse = await owsHelper.CheckinIntoOpera(ReservationNameID.ToString());
                        if (owsResponse != null && owsResponse.result)
                        {
                            if (WithSharer)
                            {
                                if (operaReservation[0].SharerReservations != null && operaReservation[0].SharerReservations.Count > 0)
                                {
                                    foreach (var sharer in operaReservation[0].SharerReservations)
                                    {
                                        var owsSharerResponse = await owsHelper.CheckinIntoOpera(sharer.ReservationNameID.ToString());
                                        if (owsResponse != null && owsResponse.result)
                                        {
                                            System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Sharer Reservation Success" });
                                        }
                                        else
                                        {
                                            System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Sharer Reservation Failed" });
                                        }
                                    }
                                }
                                else
                                {
                                    System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} No Sharer Reservation found" });
                                }
                            }
                            return Json(new { result = owsResponse.result, message = owsResponse.responseMessage });
                        }
                        else
                        {
                            string ErrorMessage = owsResponse != null ? owsResponse.responseMessage : "Null Response";
                            //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Primary Reservation checkin failed with message {ErrorMessage }" });
                            return Json(new { result = false, message = "Unable to do checkin, please try again later" });
                        }
                    }
                    else
                    {
                        //System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Primary Reservation is not DUE IN, Current State {operaReservation[0].ReservationStatus}" });
                        return Json(new { result = false, message = "Reservation status is not valid for checkin" });
                    }
                }
                else
                {
                   // System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Fetch Reservation Null Response from server" });
                    return Json(new { result = false, message = "Unable to do checkin, please try again later" });

                }
            }
            else
            {
                return Json(new { result = false, message = "Unable to do checkin, please try again later" });

            }


        }

        [HttpPost]
        public async Task<ActionResult> Encodekey(int ReservationID)
        {
            BusinessLogic.LocalAPICallsHelper localAPIHelper = new BusinessLogic.LocalAPICallsHelper();

            var reservation = db.usp_GetReservationListDetails(ReservationID).Take(1).FirstOrDefault();
            if (reservation != null)
            {
                Models.KeyEncodeRequestModel keyEncodeRequestModel = new Models.KeyEncodeRequestModel()
                {
                    CheckoutDate = reservation.DepartureDate.Value.ToString("yyyyMMdd"),
                    CheckoutTime = reservation.ETA,
                    EncoderID = string.Empty,
                    GuestName = reservation.LastName,
                    IsNewKey = false,
                    ReservationNo = reservation.ReservationNumber,
                    RoomNo = reservation.RoomNumber
                };

                var owsResponse = await localAPIHelper.Encodekey(keyEncodeRequestModel);
                if (owsResponse != null)
                {
                    return Json(new { result = owsResponse.result, message = owsResponse.responseMessage });
                }
                else
                {
                    return Json(new { result = false, message = "Unable to do cut the key, please try again later" });
                }
            }
            else
            {
                return Json(new { result = false, message = "Unable to fetch reservation details" });
            }
        }

        [HttpGet]
        public ActionResult DownloadDeclaration(int ReservationID)
        {
            var reservation = db.usp_GetReservationListDetails(ReservationID).Take(1).FirstOrDefault();
            if (reservation != null)
            {

                string GuestName = string.Empty;
                string ConfirmationNumber = reservation.ReservationNumber;
                if (!string.IsNullOrEmpty(reservation.FirstName))
                {
                    GuestName = $"{GuestName.Trim()} {reservation.FirstName.Trim()}";
                }
                if (!string.IsNullOrEmpty(reservation.MiddleName))
                {
                    GuestName = $"{GuestName.Trim()} {reservation.MiddleName.Trim()}";
                }
                if (!string.IsNullOrEmpty(reservation.LastName))
                {
                    GuestName = $"{GuestName.Trim()} {reservation.LastName.Trim()}".Trim();
                }

                //get Declaration Question for Reservation
                ViewBag.FeedBackData = db.Usp_FeedbackData(ReservationID).ToList();
                var FeedBacks = db.Usp_FeedbackData(ReservationID).ToList();

                DataTable DeclarationTable = new DataTable();

                #region DataTable Columns
                DeclarationTable.Columns.Add("Question", typeof(string));
                DeclarationTable.Columns.Add("Answer", typeof(string));
                #endregion

                foreach (var question in FeedBacks)
                {
                    DataRow declarationRow = DeclarationTable.NewRow();
                    declarationRow["Question"] = question.Question;
                    declarationRow["Answer"] = question.Answer;
                    DeclarationTable.Rows.Add(declarationRow);
                }

                ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/questionnaire.rdlc");

                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("GuestName", GuestName));
                parameters.Add(new ReportParameter("ConfirmationNo", ConfirmationNumber));
                rv.LocalReport.SetParameters(parameters);

                ReportDataSource declarationDatasource = new ReportDataSource();
                declarationDatasource.Name = "DataSet1";
                declarationDatasource.Value = DeclarationTable;
                rv.LocalReport.DataSources.Add(declarationDatasource);


                rv.LocalReport.Refresh();
                #region MyRegion
                byte[] streamBytes = null;
                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streamids = null;
                Warning[] warnings = null;
                #endregion
                streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                return File(streamBytes, "application/pdf", $"{ConfirmationNumber}_Declaration.pdf");
            }
            else
            {
                ViewBag.ReservationGroup = "PreCheckin";
                return View("Index");
            }
        }

        public ActionResult DeleteProfileInfo(DeleteProfileModel model)
        {
            //db.usp_GetReservationListDetails(model.ReservationDetailID);
            var result = new Helpers.DapperHelper().ExecuteSP("Usp_ClearProfileInfo", new { ReservationDetailID = model.ReservationDetailID, Comments = model.ProfileDeleteReason});
            //return RedirectToAction("Details", new { ReservationID = model.ReservationDetailID });
            //ViewBag.ReservationGroup = "PreCheckin";
            return Json(new { result = true, message = "" });
            //return View("Index");
        }

        public ActionResult ReservationUpsellDetails(int ReservationID)
        {

            var reservation = db.usp_GetReservationListDetails(ReservationID).Take(1).FirstOrDefault();
            if (reservation != null)
            {
                var RequestList = db.Usp_GetRequestDetails(null, null, ReservationID).ToList();
                if(RequestList != null && RequestList.Count > 0)
                //get Declaration Question for Reservation
                    ViewBag.RequestList = RequestList;

                var FeedBacks = db.Usp_FeedbackData(ReservationID).ToList();
                if(FeedBacks != null && FeedBacks.Count > 0)
                    ViewBag.FeedBackData = FeedBacks;

                //Get Registration card and folio for reservation
                var RegCardFolio = db.usp_GetReservationDocuments(ReservationID).ToList();
                if(RegCardFolio != null && RegCardFolio.Count > 0)
                    ViewBag.RegCardFolio = RegCardFolio;

                var reservationList = db.usp_GetReservationListDetails(ReservationID).ToList();
                if (reservationList != null)
                {
                    var reservation1 = reservationList.Skip(0).Take(1).FirstOrDefault();
                    var swapProfileItems = new List<SelectListItem>();
                    int i = 1;



                    foreach (var item in reservationList)
                    {
                        // System.IO.File.AppendAllLines(Server.MapPath(@"~\log.txt"), new string[] { $"{DateTime.Now.ToString()} Swap Profile {item.ProfileDetailID} with {reservation.ProfileDetailID}" });

                        string GuestName = "Primary Guest";
                        if (item.ProfileDetailID != reservation1.ProfileDetailID)
                        {
                            if (i > 1)
                            {
                                GuestName = $"Accompany Guest {i - 1}";
                            }

                            swapProfileItems.Add(new SelectListItem()
                            {
                                Text = GuestName,
                                Value = item.ProfileDetailID.ToString()
                            });
                        }
                        i++;
                    }

                    if (swapProfileItems != null && swapProfileItems.Count > 0)
                        ViewBag.SwapProfileList = new SelectList(swapProfileItems, "Value", "Text");
                }
                    return View(reservation);
            }
            else
            {
                ViewBag.ReservationGroup = "PreCheckin";
                return View("Index");
            }
        }

        public async Task<ActionResult> AssignRoom(AssignRoomModel model)
        {

            BusinessLogic.OwsHelper localAPIHelper = new BusinessLogic.OwsHelper();

            var owsResponse = await localAPIHelper.AssignRoom(model.RoomNumber, model.ReservationNameID);
            if (owsResponse != null)
            {
                return Json(new { result = owsResponse.result, message = owsResponse.responseMessage });
            }
            else
            {
                return Json(new { result = false, message = "Unable to do cut the key, please try again later" });
            }


        }

        public ActionResult profileSwap(ProfileSwapModel profileSwapModel)
        {
            db.Usp_SwapProfileDetails(profileSwapModel.fromProfileDetailId, profileSwapModel.toProfileDetailId);
            return Json(new {restult = true, message = "Success" });
        }

        public async Task<ActionResult> UpdateProfile(UpdateProfileModel model)
        {
            BusinessLogic.OwsHelper localAPIHelper = new BusinessLogic.OwsHelper();

            string Message = "Success";
            var existingReservaiton = db.usp_GetReservationListDetails(model.ReservationDetailID).ToList();

            db.Usp_UpdateProfileInformation(model.ProfileDetailID, model.EmailID,
                model.PhoneNumber, model.NationalityID, model.AddressLine1,
                null, model.StateMasterID, model.PostalCode, model.Gender, model.CountryMasterID, model.City, null);

            
            if(existingReservaiton != null && existingReservaiton.Count() > 0)
            {

                var existingProfile = existingReservaiton.Where(x => x.ProfileDetailID == model.ProfileDetailID).FirstOrDefault();

                var countryLisst = db.tbCountryMasters.ToList();

                var nationality = countryLisst.Where(x => x.CountryMasterID == model.NationalityID).FirstOrDefault();
                var country = countryLisst.Where(x => x.CountryMasterID == model.CountryMasterID).FirstOrDefault();

                model.Country = country.Country_2Char_code;
                model.Nationality = nationality.Country_2Char_code;


                if (existingProfile != null)
                {
                    var documentType = db.tbDocumentMasters.ToList();

                    var operaDOcType = documentType.Where(x => x.DocumentType == model.DocumentType).FirstOrDefault();

                    if (operaDOcType != null)
                    {

                    }

                    var email = new Emails()
                    {
                        displaySequence = 1,
                        email = model.EmailID,
                        primary = true
                    };

                    var phone = new Phones()
                    {
                        displaySequence = 1,
                        PhoneNumber = model.PhoneNumber,
                        phoneRole = "PHONE",
                        phoneType = "BUSINESS",
                        primary = true
                    };

                    var address = new Addresses()
                    {
                        displaySequence = 1,
                        address1 = model.AddressLine1,
                        address2 = "",
                        addressType = "BUSINESS",
                        city = model.City,
                        Country = model.Country,
                        state = model.State,
                        Zip = model.PostalCode,
                        primary = true
                    };

                    if(existingProfile.Email != model.EmailID)
                    {
                        var Response = await localAPIHelper.UpdateProfileEmail(email, model.PMSProfileID);
                        if(Response == null || !Response.result)
                        {
                            Message = "Unable to Update Email";
                        }
                    }
                    
                    if(existingProfile.Phone != model.PhoneNumber)
                    {
                        var Response =  await localAPIHelper.UpdateProfilePhone(phone, model.PMSProfileID);
                        if (Response == null || !Response.result)
                        {
                            Message += " Unable to Phone Number";
                        }
                    }

                    if (existingProfile.AddressLine1 != model.AddressLine1 ||
                        existingProfile.City != model.City ||
                        existingProfile.PostalCode != model.PostalCode||
                        existingProfile.StateMasterID != model.StateMasterID||
                        existingProfile.CountryMasterID != model.CountryMasterID)
                    {
                        var Response = await localAPIHelper.UpdateProfileAddress(address, model.PMSProfileID);
                        if (Response == null || !Response.result)
                        {
                            Message += " Unable to Address Details";
                        }

                    }


                    if (existingProfile.DocumentNumber != model.DocumentNumber ||
                        existingProfile.DocumentType != model.DocumentType)
                    {
                        var Response = await localAPIHelper.UpdateProfilePassport(email, phone, address, new DocInformation()
                        {
                            dob = model.BirthDate,
                            documentNumber = model.DocumentNumber,
                            documentType = operaDOcType != null ? operaDOcType.OperaDocumentCode : model.DocumentType,
                            gender = model.Gender,
                            issueCountry = model.IssueCountry,
                            issueDate = model.IssueCountry,
                            nationality = model.Nationality

                        }, model.PMSProfileID);

                        if (Response == null || !Response.result)
                        {
                            Message += $" {Response.responseMessage}";
                        }
                    }

                    DateTime IssueDate = new DateTime();
                    DateTime ExpiryDate = new DateTime();
                    DateTime BirthDate = new DateTime();

                    DateTime.TryParseExact(model.IssueDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out IssueDate);
                    DateTime.TryParseExact(model.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ExpiryDate);
                    DateTime.TryParseExact(model.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);

                    var fullGender = "UNKNOWN";
                    if(model.Gender == "M")
                    {
                        fullGender = "Male";
                    }
                    else if(model.Gender == "F")
                    {
                        fullGender = "Female";
                    }

                  

                    if (existingProfile.BirthDate != BirthDate ||
                        existingProfile.Gender != model.Gender ||
                        existingProfile.ExpiryDate != ExpiryDate ||
                        existingProfile.IssueDate != IssueDate ||
                        existingProfile.IssueCountry != model.IssueCountry ||
                        existingProfile.NationalityID != model.NationalityID)
                    {
                        var Responsename = await localAPIHelper.UpdateProfileName(email, phone, address, new DocInformation()
                        {
                            dob = BirthDate.ToString("yyyy-MM-dd"),
                            documentNumber = model.DocumentNumber,
                            documentType = operaDOcType != null ? operaDOcType.OperaDocumentCode : model.DocumentType,
                            gender = fullGender,
                            issueCountry = model.IssueCountry,
                            issueDate = IssueDate.ToString("yyyy-MM-dd"),
                            expiryDate = ExpiryDate.ToString("yyyy-MM-dd"),
                            nationality = model.Nationality,
                            

                        }, model.PMSProfileID);

                        if (Responsename == null || !Responsename.result)
                        {
                            Message += " Unable to update profile informations";
                        }
                    }
                    return Json(new { result = Message != "Success" ? false : true, message = Message });
                }
            }
            return Json(new { result = false, message = Message });
        }

        public async Task<ActionResult> SendPrecheckinLink(string ConfirmationNo)
        {
            //BusinessLogic.PushReservationLogic pushReservationLogic = new PushReservationLogic();
            //var result = await pushReservationLogic.PushReservationToCloud(ConfirmationNo);
            //return Json(result,JsonRequestBehavior.AllowGet);


            BusinessLogic.GrabberLogics pushReservationLogic = new GrabberLogics();
            var result = await GrabberLogics.PushDueInReservation(ConfirmationNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> SendPrecheckoutLink(string ConfirmationNo)
        {
            BusinessLogic.GrabberLogics pushReservationLogic = new GrabberLogics();
            var result = await GrabberLogics.PushDueOutReservation(ConfirmationNo);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CheckReservation(string ConfirmationNo)
        {
            int ReservationID = 0;
            var ReservationIDString = db.tbReservationDetails.Where(x => x.ReservationNumber == ConfirmationNo);
           
            if(ReservationIDString!=null && ReservationIDString.Any())
            {
                var reservationID = ReservationIDString.First();
                //  return RedirectToAction("Details", new { ReservationID = reservationID.ReservationDetailID });
                ReservationID = reservationID.ReservationDetailID;
                return Json(new { result = true, message = ReservationID }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LocalAPICallsHelper localHelper = new LocalAPICallsHelper();
                var result= await localHelper.PushLocalReservation(ConfirmationNo);
              
                if (result!=null && result.result)
                {
                  
                    ReservationIDString = db.tbReservationDetails.Where(x => x.ReservationNumber == ConfirmationNo);
                    if (ReservationIDString != null && ReservationIDString.Any())
                    {
                        var reservationID = ReservationIDString.First();
                        // return Json(new { Result = true, ReservationID = reservationID.ReservationDetailID });
                        //return RedirectToAction("Details", new { ReservationID = reservationID.ReservationDetailID });
                        ReservationID = reservationID.ReservationDetailID;
                       
                        return Json(new { result = true, message = ReservationID }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //return RedirectToAction("index", new { ReservationGroup = "Normal" });
                       // ReservationID = reservationID.ReservationDetailID;
                        return Json(new { result = false, message = 0 }, JsonRequestBehavior.AllowGet);
                    }
                   
                }
                //return RedirectToAction("index", new { ReservationGroup = "Normal"});
               
                return Json(new { result = false, message = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}