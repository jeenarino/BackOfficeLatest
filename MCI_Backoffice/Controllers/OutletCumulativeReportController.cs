using CheckinPortal.BackOffice.BusinessLogic;
using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Helpers;
using CheckinPortal.BackOffice.Models;
using CheckinPortal.BackOffice.Models.SmartTap;
using DigiDoc.BackOffice;
using DigiDoc.DataAccess.Models;
using MCIGrabberService.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class OutletCumulativeReportController : BaseController
    {
        // GET: Reports
        private PortalDBEntities db = new PortalDBEntities();
        SmartTapLogic smartlogic = new SmartTapLogic();
        public async Task<ActionResult> OutletCumulativeReport(string Message = null)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DateTime startDate = DateTime.Now;

            ViewBag.StartDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.EndDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.Message = Message;
            GetRequestModel get = new GetRequestModel();
            get.id = null;
            var outlet = await smartlogic.GetOutlets(get);
            if (outlet != null)
            {
                var outlets = (List<Outlet>)outlet.Data;

                ViewBag.OutletNameList = new SelectList(outlets, "OutletID", "OutletName");
            }
            List<GeneralPackages> vs = new List<GeneralPackages>();
           
             vs.Add(new GeneralPackages() {
                 PackageCode = "GP",
                 PackageDescription = "General Package"
             });
            vs.Add(new GeneralPackages()
            {
                PackageCode = "BP",
                PackageDescription = "BreakFast Package"
            });
            vs.Add(new GeneralPackages()
            {
                PackageCode = "CP",
                PackageDescription = "Complementary Package"
            });
            //  ViewBag.OutleOutletNameLists = new SelectList(outlets, "OutletID", "OutletName");
            ViewBag.FunctionalityList = new SelectList(vs, "PackageCode", "PackageDescription");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FetchOutletCumulativeReport(DocumentDataTableModel model, Models.Search search, string StartDate, string EndDate,int? OutletId,string functionality, string SubmitButon = null)
        {
           
                ViewBag.Message = null;
           
                if (functionality == "-- Select FunctionalityType--")
                {
                    functionality = null;
                }
                else
                {
                    if (functionality == "Complementary Package")
                    { functionality = "CP"; }
                    if (functionality == "BreakFast Package")
                    { functionality = "BP"; }
                    if (functionality == "General Package")
                    { functionality = "GP"; }
                }
                DateTime StartDateDT;
                DateTime EndDateDT;

                DateTime startDate = DateTime.Now;
                if (!string.IsNullOrEmpty(StartDate))
                {
                    StartDateDT = DateTime.ParseExact(StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(EndDate))

                {
                    EndDateDT = DateTime.ParseExact(EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }
                int start = 0;

                if (model.Start > 0)
                {
                    start = model.Start / model.Length;
                }

                start += 1;

                string filterby = string.Empty;
                string soryOrder = "DESC";
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
                if (sortBy == "0")
                {
                    sortColumn = "Functionality Type";
                }
                else if (sortBy == "1")
                {
                    sortColumn = "Outlet Name";
                }
                else if (sortBy == "2")
                {
                    sortColumn = "Guest Name";
                }
                else if (sortBy == "3")
                {
                    sortColumn = "Room No";
                }
                else if (sortBy == "4")
                {
                    sortColumn = "Consumed Count";
                }
                else if (sortBy == "5")
                {
                    sortColumn = "Transaction Date";
                }



                if (Request.Params["order[0][dir]"] != null)
                {
                    soryOrder = Request.Params["order[0][dir]"].ToString();
                }
           
                Models.CumulativeReportRequestModel cumulative = new Models.CumulativeReportRequestModel();
                cumulative.PageNumber = start;
                cumulative.PageSize = model.Length;
                cumulative.StartDate = string.IsNullOrEmpty(StartDate) ? DateTime.MinValue : DateTime.ParseExact(StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                cumulative.EndDate = string.IsNullOrEmpty(EndDate) ? DateTime.MinValue : DateTime.ParseExact(EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                cumulative.search = filterby;
                cumulative.Sort = sortColumn;
                cumulative.SortBy = soryOrder;
                cumulative.OutletId = OutletId;
                cumulative.FunctionalityId = functionality;
            if (string.IsNullOrEmpty(SubmitButon))
            {
                cumulative.IsPDF = false;
                var spResponse = await new SmartTapLogic().GetCumulativeReport(cumulative);
                if (spResponse != null && spResponse.Result && spResponse.Data != null)
                {
                    var CumulativeHeaders = new List<CumulativeReponseModel>();
                    try
                    {
                        CumulativeHeaders = (List<CumulativeReponseModel>)spResponse.Data;


                        var TotalCount = CumulativeHeaders[0].TotalRecords;

                        var response = new
                        {
                            draw = model.draw,
                            data = CumulativeHeaders,
                            recordsFiltered = TotalCount,
                            recordsTotal = TotalCount
                        };

                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new
                    {
                        draw = model.draw,
                        data = spResponse,
                        recordsFiltered = 0,
                        recordsTotal = 0
                    };
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                cumulative.IsPDF = true;
                var spResponse = await new SmartTapLogic().GetCumulativeReport(cumulative);
                if (spResponse != null && spResponse.Result && spResponse.Data != null)
                {
                    var CumulativeHeaders = new List<CumulativeReponseModel>();

                    CumulativeHeaders = (List<CumulativeReponseModel>)spResponse.Data;
                    ListtoDataTableConverter converter = new ListtoDataTableConverter();

                    DataTable dt = converter.ToDataTable(CumulativeHeaders);
                    var response = RDLCHelper.getOutletCumulativeReportAsBase64(dt, new DigiDoc.PdfHelper.Model.OutletCumulativeReportParameterModel()
                    {
                        FromDate = cumulative.StartDate.ToString("dd/MM/yyyy"),
                        ToDate = cumulative.EndDate.ToString("dd/MM/yyyy"),
                        ReportName = "Outlet Cumulative Report"
                    });
                    if (response.Result)
                    {
                        byte[] pdfBytes = Convert.FromBase64String(response.Data.ToString());
                        return File(pdfBytes, "PDF", "OutletCumulativeReport.pdf");
                    }
                    else
                    {
                        LogHelper.Instance.Debug($"Failled to generate Outlet Cumulative Report from RDLC with reason - {response.Message} (code : {response.ResponseCode})", "", "FetchOutletCumulativeReport", "Backoffice", "FetchOutletCumulativeReport");
                        return RedirectToAction("OutletCumulativeReport", new { Message = response.Message });
                    }

                }
            }
            return RedirectToAction("OutletCumulativeReport");

        }
    }
}