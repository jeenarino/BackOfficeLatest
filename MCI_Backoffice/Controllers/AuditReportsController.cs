using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Helpers;
using DigiDoc.BackOffice;
using DigiDoc.DataAccess.Models;

using MCIGrabberService.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class AuditReportsController : BaseController
    {
        // GET: Reports
        private PortalDBEntities db = new PortalDBEntities();
        public ActionResult AuditReport(string Message = null)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DateTime startDate = DateTime.Now;

            ViewBag.StartDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.EndDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.Message = Message;
            return View();
        }

        [HttpPost]
        public ActionResult FetchAuditReport(DocumentDataTableModel model, Search search, string StartDate, string EndDate, string SubmitButon = null)
        {
            ViewBag.Message = null;
            if (string.IsNullOrEmpty(SubmitButon))
            {
                   DateTime StartDateDT;
                DateTime EndDateDT;
               
                DateTime startDate = DateTime.Now;
                if (string.IsNullOrEmpty(StartDate))
                    StartDateDT = startDate;
                else
                {
                    StartDateDT = DateTime.ParseExact(StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (string.IsNullOrEmpty(EndDate))
                    EndDateDT = startDate;
                else
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
                    sortColumn = "Audit Date";
                }
                else if (sortBy == "1")
                {
                    sortColumn = "Audit Date";
                }
                else if (sortBy == "2")
                {
                    sortColumn = "Module Name";
                }
                else if (sortBy == "3")
                {
                    sortColumn = "User Name";
                }
                else if (sortBy == "4")
                {
                    sortColumn = "Action Name";
                }
                else if (sortBy == "5")
                {
                    sortColumn = "Audit Date";
                }


                if (Request.Params["order[0][dir]"] != null)
                {
                    soryOrder = Request.Params["order[0][dir]"].ToString();
                }
                var spResponse =new AuditReportHelper().fetchAuditHeaderDetails(start, model.Length, StartDateDT.ToString("yyyyMMdd"), EndDateDT.ToString("yyyyMMdd"), filterby, sortColumn, soryOrder);        
                if (spResponse != null && spResponse.result)
                {
                    var auditHeaders = new List<AuditHeaderModel>();
                    try
                    {
                        auditHeaders = (List<AuditHeaderModel>)spResponse.ResponseData;

                        auditHeaders.ToList().ForEach(s => s.AuditTime = s.AuditDateTime.ToString("HH:mm:ss tt"));

                        var TotalCount = auditHeaders[0].TotalRecords;

                        var response = new
                        {
                            draw = model.draw,
                            data = auditHeaders,
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
                        data= spResponse,
                        recordsFiltered = 0,
                        recordsTotal = 0
                    };
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                DateTime StartDateDT = DateTime.ParseExact(StartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime EndDateDT = DateTime.ParseExact(EndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                var SPResponse = new AuditReportHelper().getAuditDetailsForReport(StartDateDT.ToString("yyyyMMdd"), EndDateDT.ToString("yyyyMMdd"));

                if (SPResponse.result)
                {

                   
                    var response =RDLCHelper.getAuditReportAsBase64(SPResponse.DataTable, new DigiDoc.PdfHelper.Model.AuditReportParameterModel()
                    {
                        FromDate = StartDateDT.ToString("dd/MM/yyyy"),
                        ToDate = EndDateDT.ToString("dd/MM/yyyy"),
                        ReportName = "Audit Report"
                    });
                    if (response.Result)
                    {
                        byte[] pdfBytes = Convert.FromBase64String(response.Data.ToString());
                        return File(pdfBytes, "PDF", "BackofficeAuditReport.pdf");
                    }
                    else
                    {
                        LogHelper.Instance.Debug($"Failled to generate audit report from RDLC with reason - {response.Message} (code : {response.ResponseCode})","", "FetchAuditReport", "Backoffice", "FetchAuditReport");
                        return RedirectToAction("AuditReport", new { Message = response.Message });
                    }
                }
                else
                {
                    LogHelper.Instance.Debug($"Failled to Fetch audit details from DB with reason - {SPResponse.ResponseMessage} (code : {SPResponse.ResultCode})", "", "FetchAuditReport", "Backoffice", "FetchAuditReport");
                    return RedirectToAction("AuditReport", new { Message = SPResponse.ResponseMessage });
                }
            }
        }
        [HttpPost]
        public ActionResult GetAuditDetails(int AuditID)
        {

            var spResponse = new AuditReportHelper().getAuditDetails(AuditID);
            if (spResponse != null && spResponse.result)
            {
                return PartialView((List<AuditDetailsModel>)spResponse.ResponseData);
            }
            else
                return PartialView();
        }

        [HttpPost]
        public ActionResult GetDetails()
        {

            return Json(new { id = "sdsad", JsonRequestBehavior.AllowGet });
        }
    }
}