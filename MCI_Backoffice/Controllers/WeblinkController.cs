using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Helpers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class WeblinkController : Controller
    {
        private PortalDBEntities db = new PortalDBEntities();
        // GET: Weblink
        public async Task<ActionResult> ReservationDetails(string ReservationNameID, int GuestIndex = 0)
        {
            ViewBag.GuestIndex = GuestIndex;
            var reservationDetails = db.tbReservationDetails.Where(x => x.ReservationNameID == ReservationNameID).FirstOrDefault();
            if (reservationDetails != null)
            {

                var reservationList = db.usp_GetReservationListDetails(reservationDetails.ReservationDetailID).ToList();

                if (reservationList != null)
                {
                    var reservation = reservationList.Skip(GuestIndex).Take(1).FirstOrDefault();
                    BusinessLogic.OwsHelper owsHelper = new BusinessLogic.OwsHelper();

                    if (reservation != null)
                    {
                        ViewBag.RoomNotFoundMessage = string.Empty;
                        ViewBag.IsRoomAssigned = true;
                        if (string.IsNullOrEmpty(reservation.RoomNumber))
                        {
                            ViewBag.IsRoomAssigned = false;
                            #region Fetch Room List from OS
                            var FetchRoomResponse = await owsHelper.GetOperaRoomListByRoomType(reservation.DepartureDate.Value, reservation.RoomType);

                            if (FetchRoomResponse != null && FetchRoomResponse.responseData != null)
                            {
                                if (FetchRoomResponse.responseData.Count > 0)
                                {
                                    ViewBag.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                    reservation.RoomNumber = FetchRoomResponse.responseData[0].RoomNumber;
                                }
                                else
                                {
                                    ViewBag.RoomNotFoundMessage = FetchRoomResponse.responseMessage;
                                }
                            }
                            else
                            {
                                ViewBag.RoomNotFoundMessage = FetchRoomResponse.responseMessage;
                            }
                            #endregion
                        }
                        //Get Reservation document for primary guest
                        var reservationIDDocument = db.usp_GetReservationListDocumentsDetails(reservationDetails.ReservationDetailID).Skip(GuestIndex).Take(1).FirstOrDefault();
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
                        var RegCardFolio = db.usp_GetReservationDocuments(reservationDetails.ReservationDetailID).ToList();
                        ViewBag.RegCardFolio = RegCardFolio;

                        //get Declaration Question for Reservation
                        ViewBag.FeedBackData = db.Usp_FeedbackData(reservationDetails.ReservationDetailID).ToList();

                        var swapProfileItems = new List<SelectListItem>();
                        int i = 1;
                        foreach (var item in reservationList)
                        {
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

                        ViewBag.SwapProfileList = new SelectList(swapProfileItems, "Value", "Text");

                        return View(reservation);
                    }
                    else
                    {
                        var reservationList1 = db.usp_GetReservationList(null,null,1,25, null, null, null).ToList();
                        return View("Index", reservationList1);
                    }
                }
                else
                {
                    var reservationList1 = db.usp_GetReservationList(null, null, 1, 25, null, null, null).ToList();
                    return View("Index", reservationList1);
                }

            }
            else
            {
                return View("ReservationNotfound");
            }
        }

        public ActionResult ReservationDocuments(int ReservationID,int ReservationDocumentID)
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
                ViewBag.FeedBackData = db.Usp_FeedbackData(ReservationID).ToList();

                return View(reservation);
            }
            else
            {
                return View("ReservationNotfound");
            }
        }

        public ActionResult Declarations(int ReservationID)
        {
            var reservation = db.usp_GetReservationListDetails(ReservationID).Take(1).FirstOrDefault();
            if (reservation != null)
            {
                //get Declaration Question for Reservation
                ViewBag.FeedBackData = db.Usp_FeedbackData(ReservationID).ToList();

                var FeedBacks = db.Usp_FeedbackData(ReservationID).ToList();
                ViewBag.FeedBackData = FeedBacks;

                //Get Registration card and folio for reservation
                var RegCardFolio = db.usp_GetReservationDocuments(ReservationID).ToList();
                ViewBag.RegCardFolio = RegCardFolio;


                return View(reservation);
            }
            else
            {
                return View("ReservationNotfound");
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
                return View("ReservationNotfound");
            }
        }

        public ActionResult ReservationUpsellDetails(int ReservationID)
        {

            var reservation = db.usp_GetReservationListDetails(ReservationID).Take(1).FirstOrDefault();
            if (reservation != null)
            {
                var RequestList = db.Usp_GetRequestDetails(null, null, ReservationID).ToList();
                //get Declaration Question for Reservation
                ViewBag.RequestList = RequestList;

                var FeedBacks = db.Usp_FeedbackData(ReservationID).ToList();
                ViewBag.FeedBackData = FeedBacks;

                //Get Registration card and folio for reservation
                var RegCardFolio = db.usp_GetReservationDocuments(ReservationID).ToList();
                ViewBag.RegCardFolio = RegCardFolio;
                return View(reservation);
            }
            else
            {
                return View("ReservationNotfound");
            }
        }
    }
}