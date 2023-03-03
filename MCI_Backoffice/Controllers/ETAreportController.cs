using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Helpers;
using CheckinPortal.BackOffice.Models;
using CheckinPortal.BackOffice.Models.Reports;
using DigiDoc.BackOffice;
using MCIGrabberService.Helper;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CheckinPortal.BackOffice.Controllers
{
    public class ETAreportController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();
        // GET: ETAreport
        public ActionResult Index(string Message = null)
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DateTime startDate = DateTime.Now;
          
           
            //ViewBag.StartDate = startDate.ToString("MM/dd/yyyy");
            //ViewBag.EndDate = startDate.ToString("MM/dd/yyyy");
            ViewBag.Message = Message;

            ViewBag.StartDate = startDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = startDate.ToString("yyyy-MM-dd");
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"RDLCs/ETAReport/ETAReport.rdlc";
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.SetParameters(new ReportParameter[] {
              
                
               
                new ReportParameter("FromDate", startDate.ToString("yyyy-MM-dd")),
                new ReportParameter("ToDate", startDate.ToString("yyyy-MM-dd")),
                
            });



         

            var SPResponse = new ETAReportHelper().getReservationForReport(startDate.ToString("yyyyMMdd"), startDate.ToString("yyyyMMdd"));

            if (SPResponse.result)
            {
                
                if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/ETAReport/ETAReport.rdlc")))
                {
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReservationDataSet", SPResponse.ResponseData));
                    reportViewer.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    ViewBag.ReportViewer = reportViewer;
                }
            }
            else
            {
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReservationDataSet", SPResponse.ResponseData));
                reportViewer.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                ViewBag.ReportViewer = reportViewer;
            }


            return View();
        }

       
        [HttpPost]
        public ActionResult Index(ETAReportRequestModel eTA)
        {
            ViewBag.StartDate = eTA.StartDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = eTA.EndDate.Value.ToString("yyyy-MM-dd");
            string Message = null;
            ViewBag.Message = Message;
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"RDLCs/ETAReport/ETAReport.rdlc";
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.SetParameters(new ReportParameter[] {
              
                
                //new ReportParameter("PropertyName",property.name),
                new ReportParameter("FromDate", eTA.StartDate.Value.ToString("yyyy-MM-dd")),
                new ReportParameter("ToDate", eTA.EndDate.Value.ToString("yyyy-MM-dd")),

            });





            var SPResponse = new ETAReportHelper().getReservationForReport(eTA.StartDate.Value.ToString("MM/dd/yyyy"), eTA.EndDate.Value.ToString("MM/dd/yyyy"));

            if (SPResponse.result)
            {

                if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/ETAReport/ETAReport.rdlc")))
                {
                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReservationDataSet", SPResponse.ResponseData));
                    reportViewer.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                    ViewBag.ReportViewer = reportViewer;
                }
            }
            else
            {
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ReservationDataSet", SPResponse.ResponseData));
                reportViewer.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                ViewBag.ReportViewer = reportViewer;
            }



            return View();
        }
    }
}