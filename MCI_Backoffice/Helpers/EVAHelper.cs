using CheckinPortal.BackOffice.Models;
using DigiDoc.Models;
using MCIGrabberService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Helpers
{
    public class EVAHelper
    {
        private readonly Helpers.DapperHelper db = new Helpers.DapperHelper();
        public UtilityResponseModel fetchEVADetails(int PageNumber, int PageSize, string StartDate, string EndDate, string search, string Sort, string SortBy,string StatusType)
        {
            try
            {

                var spResponse = db.ExecuteSP<EVAModel>("Usp_Fetch_STBData"
                                                   , new { StatusType=StatusType,PageNumber = PageNumber, PageSize = PageSize, ReportStartDate = StartDate, ReportEndDate = EndDate, search = search, Sort = Sort, SortBy = SortBy }).ToList();
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
                    LogHelper.Instance.Debug("SP Usp_Fetch_STBData returned null", " ", "fetchEVADetails", "Backoffice", "EVAReport");
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
                LogHelper.Instance.Error(ex, " ", "fetchEVADetails", "Backoffice", "EVAReport");
                return new UtilityResponseModel()
                {
                    result = false,
                    ResponseMessage = "Generic exception",
                    ResultCode = "-1"
                };
            }

        }

        public UtilityResponseModel fetchEVADById(string ResponseId)
        {
            try
            {

                var spResponse = db.ExecuteSP<EVAImageVerification>("Usp_Fetch_STBDataById"
                                                   , new { ResponseId=ResponseId }).ToList();
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
                    LogHelper.Instance.Debug("SP Usp_Fetch_STBData returned null", " ", "fetchEVADetails", "Backoffice", "EVAReport");
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
                LogHelper.Instance.Error(ex, " ", "fetchEVADetails", "Backoffice", "EVAReport");
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