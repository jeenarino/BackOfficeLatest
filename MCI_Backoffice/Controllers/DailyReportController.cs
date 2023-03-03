using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class DailyReportController : Controller
    {
        private readonly Helpers.DapperHelper db = new Helpers.DapperHelper();
        // GET: DailyReport
        public ActionResult DailyReport()
        {
            var result = db.ExecuteSP("Usp_RPT_DailyStatusReport", new { FromDate = DateTime.Now.AddDays(-30).ToString("yyyyMMdd"), ToDate = DateTime.Now.ToString("yyyyMMdd") });


            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"RDLCs\DailyReport.rdlc";
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                //new ReportParameter("ReportLogo", ""),
                
                //new ReportParameter("PropertyName",property.name),
                new ReportParameter("FromDate",DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd")),
                new ReportParameter("ToDate",DateTime.Now.ToString("yyyy-MM-dd")),
                //new ReportParameter("lblReportName","test"),
            });


            if (result != null)
            {
                reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DailyReport", result.ToList()));
                reportViewer.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
                ViewBag.ReportViewer = reportViewer;
               
            }
            ViewBag.FromDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            ViewBag.ToDate = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        public ActionResult DailyReport(Models.Reports.DailyREportRequestModel model)
        {
            var result = db.ExecuteSP("Usp_RPT_DailyStatusReport", new { FromDate = model.FromDate.Value.ToString("yyyy-MM-dd"), ToDate = model.ToDate.Value.ToString("yyyy-MM-dd") });


            ReportViewer reportViewer = new ReportViewer();
            reportViewer.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"RDLCs\DailyReport.rdlc";
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                //new ReportParameter("ReportLogo", ""),
                
                //new ReportParameter("PropertyName",property.name),
                new ReportParameter("FromDate",model.FromDate.Value.ToString("yyyy-MM-dd")),
                new ReportParameter("ToDate",model.ToDate.Value.ToString("yyyy-MM-dd")),
                //new ReportParameter("lblReportName","test"),
            });



            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DailyReport", result.ToList()));
            reportViewer.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            ViewBag.ReportViewer = reportViewer;
            ViewBag.FromDate = model.FromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.ToDate = model.ToDate.Value.ToString("yyyy-MM-dd");
            return View();
        }
    }
}