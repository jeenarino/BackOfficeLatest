using CheckinPortal.BackOffice.Models;
using DigiDoc.DataAccess.Models;
using DigiDoc.Models;
using MCIGrabberService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Helpers
{
   
    public class AuditReportHelper
    {
        private readonly Helpers.DapperHelper db = new Helpers.DapperHelper();
        public  UtilityResponseModel fetchAuditHeaderDetails(int PageNumber, int PageSize, string StartDate, string EndDate, string search, string Sort, string SortBy)
        {
            try
            {

                var spResponse =db.ExecuteSP<AuditHeaderModel>("Usp_GetAuditHeaderDetails"
                                                   , new { PageNumber, PageSize, StartDate, EndDate, search, Sort, SortBy }).ToList();
                if (spResponse != null && spResponse.Count() > 0)
                {
                    {
                        return new UtilityResponseModel()
                        {
                            ResponseData = spResponse,
                            result = true,
                            ResultCode = "200"
                        };
                    }
                }
                else
                {
                    LogHelper.Instance.Debug("SP Usp_GetAuditHeaderDetails returned null"," ", "fetchAuditHeaderDetails", "Backoffice", "FetchAuditReport");
                    return new UtilityResponseModel()
                    {
                        result = false,
                        ResponseMessage = "DB Error",
                        ResultCode = "-2"
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex, " ", "fetchAuditHeaderDetails", "Backoffice", "FetchAuditReport");
                return new UtilityResponseModel()
                {
                    result = false,
                    ResponseMessage = "Generic exception",
                    ResultCode = "-1"
                };
            }

        }
        public  UtilityResponseModel getAuditDetailsForReport(string StartDate, string EndDate)
        {
            try
            {
                /// Timezoneid = "'"+ Timezoneid + "'";
                var spResponse = db.ExecuteSPForDataSet(
                                                    "Usp_RPT_AuditDetails ",
                                                 
                                                    new { StartDate = StartDate, EndDate = EndDate});
                if (spResponse != null && spResponse.Rows.Count > 0)
                {
                    return new UtilityResponseModel()
                    {
                        DataTable = spResponse,
                        ResponseMessage = "Success",
                        result = true,
                        ResultCode = "200"
                    };
                }
                else
                {
                    return new UtilityResponseModel()
                    {
                        ResponseData = null,
                        ResponseMessage = "DBError",
                        result = false,
                        ResultCode = "-2"
                    };
                }

            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex,"","getAuditDetailsForReport", "Backoffice", "FetchAuditReport");
                return new UtilityResponseModel()
                {
                    ResponseData = null,
                    ResponseMessage = "General Exception",
                    result = false,
                    ResultCode = "-1"
                };
            }

        }
        public  UtilityResponseModel getAuditDetails(int auditHeaderID)
        {
            try
            {
                var spResponse = db.ExecuteSP<AuditDetailsModel>(
                                                    "Usp_FetchAuditDetailsChanges",
                                                    
                                                    new { AuditHeaderID = auditHeaderID }).ToList();
                if (spResponse != null && spResponse.Count > 0)
                {
                    {
                        return new UtilityResponseModel()
                        {
                            ResponseData = spResponse,
                            result = true,
                            ResultCode = "200"
                        };
                    }
                }
                else
                {
                    LogHelper.Instance.Debug("SP Usp_FetchAuditDetailsChanges returned null", " ", "getAuditDetails", "Backoffice", "FetchAuditReport");
                    return new UtilityResponseModel()
                    {
                        result = false,
                        ResponseMessage = "DB Error",
                        ResultCode = "-2"
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex, " ", "getAuditDetails", "Backoffice", "FetchAuditReport");
                return new UtilityResponseModel()
                {
                    result = false,
                    ResponseMessage = "Generic exception",
                    ResultCode = "-1"
                };
            }

        }
        public UtilityResponseModel fetchNotificationDetails(string IsActionTaken,string createdDate, int PageNumber, int PageSize, string search, string Sort, string SortBy,int Notificationtype)
        {
            try
            {

                var spResponse = db.ExecuteSP<NotificationModel>("usp_GetNotificationList"
                                                   , new { IsActionTaken, createdDate,PageNumber, PageSize, search, Sort, SortBy,Notificationtype}).ToList();
                if (spResponse != null && spResponse.Count() > 0)
                {
                    {
                        return new UtilityResponseModel()
                        {
                            ResponseData = spResponse,
                            result = true,
                            ResultCode = "200"
                        };
                    }
                }
                else
                {
                    LogHelper.Instance.Debug("SP usp_GetNotificationList returned null","", "fetchNotificationDetails", "Backoffice", "Notification");
                    return new UtilityResponseModel()
                    {
                        result = false,
                        ResponseMessage = "DB Error",
                        ResultCode = "-2"
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex,"", "fetchNotificationDetails", "Backoffice", "Notification");
                return new UtilityResponseModel()
                {
                    result = false,
                    ResponseMessage = "Generic exception",
                    ResultCode = "-1"
                };
            }

        }


        public UtilityResponseModel fetchNotificationMaster()
        {
            try
            {

                var spResponse = db.ExecuteSP<NotificationMaster>("Usp_GetNotificationtype").ToList();
                if (spResponse != null && spResponse.Count() > 0)
                {
                    {
                        return new UtilityResponseModel()
                        {
                            ResponseData = spResponse,
                            result = true,
                            ResultCode = "200"
                        };
                    }
                }
                else
                {
                    LogHelper.Instance.Debug("SP Usp_GetNotificationtype returned null","", "fetchNotificationMaster", "Backoffice", "NotificationMaster");
                    return new UtilityResponseModel()
                    {
                        result = false,
                        ResponseMessage = "DB Error",
                        ResultCode = "-2"
                    };
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex,"", "fetchNotificationMaster", "Backoffice", "NotificationMaster");
                return new UtilityResponseModel()
                {
                    result = false,
                    ResponseMessage = "Generic exception",
                    ResultCode = "-1"
                };
            }

        }

    }
}