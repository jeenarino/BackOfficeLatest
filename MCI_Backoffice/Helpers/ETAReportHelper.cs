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
    public class ETAReportHelper
    {
        private readonly Helpers.DapperHelper db = new Helpers.DapperHelper();
            public UtilityResponseModel fetchReservationDetails(int PageNumber, int PageSize, string StartDate, string EndDate, string search, string Sort, string SortBy)
            {
                try
                {

                    var spResponse = db.ExecuteSP<ETAModel>("Usp_GetReservationETADetails"
                                                       , new { PageNumber=PageNumber, PageSize=PageSize, ReportStartDate=StartDate, ReportEndDate=EndDate, search=search, Sort=Sort, SortBy=SortBy }).ToList();
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
                        LogHelper.Instance.Debug("SP Usp_GetReservationETADetails returned null", " ", "fetchReservationDetails", "Backoffice", "ETAReport");
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
                    LogHelper.Instance.Error(ex, " ", "fetchAuditHeaderDetails", "Backoffice", "ETAReport");
                    return new UtilityResponseModel()
                    {
                        result = false,
                        ResponseMessage = "Generic exception",
                        ResultCode = "-1"
                    };
                }

            }

        public UtilityResponseModel getReservationForReport(string StartDate, string EndDate)
        {
            try
            {
                /// Timezoneid = "'"+ Timezoneid + "'";
                var spResponse = db.ExecuteSP(
                                                    "Usp_GetReservationETAReportDetails",

                                                    new { ReportStartDate = StartDate, ReportEndDate = EndDate });
                if (spResponse != null && spResponse.Count() > 0)
                {
                    return new UtilityResponseModel()
                    {
                        ResponseData = spResponse,
                        ResponseMessage = "Success",
                        result = true,
                        ResultCode = "200"
                    };
                }
                else
                {
                    return new UtilityResponseModel()
                    {
                        ResponseData = spResponse,
                        ResponseMessage = "DBError",
                        result = false,
                        ResultCode = "-2"
                    };
                }

            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex, "", "getAuditDetailsForReport", "Backoffice", "FetchAuditReport");
                return new UtilityResponseModel()
                {
                    ResponseData = null,
                    ResponseMessage = "General Exception",
                    result = false,
                    ResultCode = "-1"
                };
            }

        }
        public static UtilityResponseModel getUserProfileDetails(string roleid = null)
        {
            int ProfileID;
            if (!string.IsNullOrEmpty(roleid) && Int32.TryParse(roleid, out ProfileID))
            {
                try
                {
                    var spResponse = new DapperHelper().ExecuteSP<UserRoleModel>("usp_GetUserRoleModuleList", new { RoleID = ProfileID }).ToList();
                    if (spResponse != null && spResponse.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(spResponse.First().Result) && spResponse.First().Result.Equals("200"))
                        {
                            return new UtilityResponseModel()
                            {
                                ResponseData = spResponse,
                                result = true,
                                ResultCode = "200"
                            };
                        }
                        else
                        {
                            return new UtilityResponseModel()
                            {
                                ResponseMessage = spResponse.First().Message,
                                result = false,
                                ResultCode = spResponse.First().Result
                            };
                        }
                    }
                    else
                    {
                        LogHelper.Instance.Debug("SP usp_GetUserProfileModuleList returned null", "Get User Profile Details", "Portal", "Master","");
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
                    LogHelper.Instance.Error(ex, "Get User Profile Details", "Portal", "Master","");
                    return new UtilityResponseModel()
                    {
                        result = false,
                        ResponseMessage = "Generic exception",
                        ResultCode = "-1"
                    };
                }
            }
            else
            {
                try
                {
                    var spResponse = new DapperHelper().ExecuteSP<UserRoleModel>("usp_GetUserProfileModuleList").ToList();
                    if (spResponse != null && spResponse.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(spResponse.First().Result) && spResponse.First().Result.Equals("200"))
                        {
                            return new UtilityResponseModel()
                            {
                                ResponseData = spResponse,
                                result = true,
                                ResultCode = "200"
                            };
                        }
                        else
                        {
                            return new UtilityResponseModel()
                            {
                                ResponseMessage = spResponse.First().Message,
                                result = false,
                                ResultCode = spResponse.First().Result
                            };
                        }
                    }
                    else
                    {
                        LogHelper.Instance.Debug("SP usp_GetUserProfileModuleList returned null", "Get User Profile Details", "Portal", "Master","");
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
                    LogHelper.Instance.Error(ex, "Get User Profile Details", "Portal", "Master","");
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
}