using DigiDoc.PdfHelper.Model;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DigiDoc.BackOffice
{
    public class RDLCHelper
    {
        public static ResponseModel getAuditReportAsBase64(DataTable ReportDataSet,AuditReportParameterModel auditReportParameter)
        {
            try
            {
                
                if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/AuditReport/AuditReport.rdlc")))
                {
                    List<ReportParameter> reportParameters = new List<ReportParameter>();
                    if (!string.IsNullOrEmpty(auditReportParameter.FromDate))
                        reportParameters.Add(new ReportParameter("FromDate", auditReportParameter.FromDate));
                    if (!string.IsNullOrEmpty(auditReportParameter.ToDate))
                        reportParameters.Add(new ReportParameter("ToDate", auditReportParameter.ToDate));
                    if (!string.IsNullOrEmpty(auditReportParameter.ReportName))
                        reportParameters.Add(new ReportParameter("ReportName", auditReportParameter.ReportName));
                    ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.ProcessingMode = ProcessingMode.Local;
                    rv.LocalReport.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/AuditReport/AuditReport.rdlc");

                    ReportDataSource reportParameterDatasource = new ReportDataSource();
                    reportParameterDatasource.Name = "AuditDataSet";
                    reportParameterDatasource.Value = ReportDataSet;
                    rv.LocalReport.DataSources.Add(reportParameterDatasource);
                    rv.LocalReport.SetParameters(reportParameters);


                    rv.LocalReport.Refresh();
                    byte[] streamBytes = null;
                    string mimeType = "";
                    string encoding = "";
                    string filenameExtension = "";
                    string[] streamids = null;
                    Warning[] warnings = null;
                    streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                    var response = Convert.ToBase64String(streamBytes);
                    return new ResponseModel()
                    {
                        Data = response,
                        Message = "Success",
                        ResponseCode = "200",
                        Result = true
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        Data = null,
                        Message = "RDLC file can not be found",
                        ResponseCode = "-5",
                        Result = false
                    }; 
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Data = null,
                    Message = ex.Message,
                    ResponseCode = "-1",
                    Result = false
                };
            }
        }

        public static ResponseModel getReservationReportAsBase64(DataTable ReportDataSet, ETAReportParameterModel etaReportParameter)
        {
            try
            {

                if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/ETAReport/ETAReport.rdlc")))
                {
                    List<ReportParameter> reportParameters = new List<ReportParameter>();
                    if (!string.IsNullOrEmpty(etaReportParameter.FromDate))
                        reportParameters.Add(new ReportParameter("FromDate", etaReportParameter.FromDate));
                    if (!string.IsNullOrEmpty(etaReportParameter.ToDate))
                        reportParameters.Add(new ReportParameter("ToDate", etaReportParameter.ToDate));
                    if (!string.IsNullOrEmpty(etaReportParameter.ReportName))
                        reportParameters.Add(new ReportParameter("ReportName", etaReportParameter.ReportName));
                    ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.ProcessingMode = ProcessingMode.Local;
                    rv.LocalReport.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/ETAReport/ETAReport.rdlc");

                    ReportDataSource reportParameterDatasource = new ReportDataSource();
                    reportParameterDatasource.Name = "ReservationDataSet";
                    reportParameterDatasource.Value = ReportDataSet;
                    rv.LocalReport.DataSources.Add(reportParameterDatasource);
                    rv.LocalReport.SetParameters(reportParameters);


                    rv.LocalReport.Refresh();
                    byte[] streamBytes = null;
                    string mimeType = "";
                    string encoding = "";
                    string filenameExtension = "";
                    string[] streamids = null;
                    Warning[] warnings = null;
                    streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                    var response = Convert.ToBase64String(streamBytes);
                    return new ResponseModel()
                    {
                        Data = response,
                        Message = "Success",
                        ResponseCode = "200",
                        Result = true
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        Data = null,
                        Message = "RDLC file can not be found",
                        ResponseCode = "-5",
                        Result = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Data = null,
                    Message = ex.Message,
                    ResponseCode = "-1",
                    Result = false
                };
            }
        }

        public static ResponseModel getOutletCumulativeReportAsBase64(DataTable ReportDataSet, OutletCumulativeReportParameterModel auditReportParameter)
        {
            try
            {

                if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/OutletCumulativeReport/OutletCumulativeReport.rdlc")))
                {
                    List<ReportParameter> reportParameters = new List<ReportParameter>();
                    if (!string.IsNullOrEmpty(auditReportParameter.FromDate))
                        reportParameters.Add(new ReportParameter("FromDate", auditReportParameter.FromDate));
                    if (!string.IsNullOrEmpty(auditReportParameter.ToDate))
                        reportParameters.Add(new ReportParameter("ToDate", auditReportParameter.ToDate));
                    if (!string.IsNullOrEmpty(auditReportParameter.ReportName))
                        reportParameters.Add(new ReportParameter("ReportName", auditReportParameter.ReportName));
                    ReportViewer rv = new Microsoft.Reporting.WebForms.ReportViewer();
                    rv.ProcessingMode = ProcessingMode.Local;
                    rv.LocalReport.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/RDLCs/OutletCumulativeReport/OutletCumulativeReport.rdlc");

                    ReportDataSource reportParameterDatasource = new ReportDataSource();
                    reportParameterDatasource.Name = "OutletCumulativeDataSet";
                    reportParameterDatasource.Value = ReportDataSet;
                    rv.LocalReport.DataSources.Add(reportParameterDatasource);
                    rv.LocalReport.SetParameters(reportParameters);


                    rv.LocalReport.Refresh();
                    byte[] streamBytes = null;
                    string mimeType = "";
                    string encoding = "";
                    string filenameExtension = "";
                    string[] streamids = null;
                    Warning[] warnings = null;
                    streamBytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                    var response = Convert.ToBase64String(streamBytes);
                    return new ResponseModel()
                    {
                        Data = response,
                        Message = "Success",
                        ResponseCode = "200",
                        Result = true
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        Data = null,
                        Message = "RDLC file can not be found",
                        ResponseCode = "-5",
                        Result = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Data = null,
                    Message = ex.Message,
                    ResponseCode = "-1",
                    Result = false
                };
            }
        }

    }
}
