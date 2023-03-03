using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Helpers
{
    public sealed class ConfigurationHelper
    {
        private static readonly Lazy<ConfigurationHelper>
          lazy = new Lazy<ConfigurationHelper>(() => new ConfigurationHelper()
          {
              //read configuration from web.config
              OWSConfig = new Models.OwsConfigurationModel()
              {
                  chainCode = ConfigurationManager.AppSettings["chainCode"].ToString(),
                  destinationEntityID = ConfigurationManager.AppSettings["destinationEntityID"].ToString(),
                  destinationSystemType = ConfigurationManager.AppSettings["destinationSystemType"].ToString(),
                  hotelDomain = ConfigurationManager.AppSettings["hotelDomain"].ToString(),
                  kioskID = ConfigurationManager.AppSettings["kioskID"].ToString(),
                  language = ConfigurationManager.AppSettings["language"].ToString(),
                  legNumber = ConfigurationManager.AppSettings["legNumber"].ToString(),
                  password = ConfigurationManager.AppSettings["password"].ToString(),
                  systemType = ConfigurationManager.AppSettings["systemType"].ToString(),
                  username = ConfigurationManager.AppSettings["username"].ToString(),
                  BaseURL = ConfigurationManager.AppSettings["OwsBaseURL"].ToString(),
                  // CloudAPIURL = ConfigurationManager.AppSettings["CloudAPIURL"].ToString(),
                  LocalAPIURL = ConfigurationManager.AppSettings["LocalAPIURL"].ToString(),
                  EmailAPIURL = ConfigurationManager.AppSettings["EmailAPIURL"].ToString(),
                  SaavyBaseURL = ConfigurationManager.AppSettings["SaavyBaseURL"].ToString(),
                  FromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString(),
                  DisplayFromEmail = ConfigurationManager.AppSettings["DisplayFromEmail"].ToString(),
                  RequestAcceptedSubject = ConfigurationManager.AppSettings["RequestAcceptedSubject"].ToString(),
                  RequestRejectedSubject = ConfigurationManager.AppSettings["RequestRejectedSubject"].ToString(),
                  OwsBaseURL = ConfigurationManager.AppSettings["OwsBaseURL"].ToString(),
                  preAuthUDF = ConfigurationManager.AppSettings["preAuthUDF"].ToString(),
                  preAuthAmntUDF = ConfigurationManager.AppSettings["preAuthAmntUDF"].ToString(),
                  GarunteeTypeCode = ConfigurationManager.AppSettings["GarunteeTypeCode"].ToString(),
                  DueInBufferDays = Convert.ToInt32(ConfigurationManager.AppSettings["DueInBufferDays"].ToString()),
                  IsBreakFastValidationWithPackage = Convert.ToBoolean(ConfigurationManager.AppSettings["IsBreakFastValidationWithPackage"].ToString()),
                  IsBreakFastValidationWithUDF = Convert.ToBoolean(ConfigurationManager.AppSettings["IsBreakFastValidationWithUDF"].ToString()),
                  PackageCodes = ConfigurationManager.AppSettings["PackageCodes"].ToString(),
                  MealPlanFieldName = ConfigurationManager.AppSettings["MealPlanFieldName"].ToString(),


              },
              GrabberConfig = new Models.GrabberConfigModel
              {
                  EmailDisplayName = ConfigurationManager.AppSettings["EmailDisplayName"].ToString(),
                  //  EMAILURL = ConfigurationManager.AppSettings["EMAILURL"].ToString(),
                  //  IsPaymentDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPaymentDisabled"].ToString()),
                  //  IsProxyEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsProxyEnabled"].ToString()),
                  LOCALURL = ConfigurationManager.AppSettings["LOCALURL"].ToString(),
                  MealPlanFieldName = ConfigurationManager.AppSettings["MealPlanFieldName"].ToString(),
                  PreArrivalEmailSubject = ConfigurationManager.AppSettings["PreArrivalEmailSubject"].ToString(),
                  PreArrivalFromEmail = ConfigurationManager.AppSettings["PreArrivalFromEmail"].ToString(),
                  PreAuthAmntUDF = ConfigurationManager.AppSettings["PreAuthAmntUDF"].ToString(),
                  PreAuthUDF = ConfigurationManager.AppSettings["PreAuthUDF"].ToString(),
                  //   ProxyPassword = ConfigurationManager.AppSettings["ProxyPassword"].ToString(),
                  //   ProxyURL = ConfigurationManager.AppSettings["ProxyURL"].ToString(),
                  //   ProxyUserName = ConfigurationManager.AppSettings["ProxyUserName"].ToString(),
                  //   URL = ConfigurationManager.AppSettings["URL"].ToString(),
                  IsETADefault = Convert.ToBoolean(ConfigurationManager.AppSettings["IsETADefault"].ToString()),
                  isProxyEnableForCloudAPI = Convert.ToBoolean(ConfigurationManager.AppSettings["isProxyEnableForCloudAPI"].ToString()),
                  CloudAPIProxyHost = ConfigurationManager.AppSettings["CloudAPIProxyHost"].ToString(),
                  CloudAPIProxyUN = ConfigurationManager.AppSettings["CloudAPIProxyUN"].ToString(),
                  CloudAPIProxyPswd = ConfigurationManager.AppSettings["CloudAPIProxyPswd"].ToString(),
                  CloudAPIURL = ConfigurationManager.AppSettings["CloudAPIURL"].ToString(),


                  isProxyEnableForLocalAPI = Convert.ToBoolean(ConfigurationManager.AppSettings["isProxyEnableForLocalAPI"].ToString()),
                  LocalAPIProxyHost = ConfigurationManager.AppSettings["LocalAPIProxyHost"].ToString(),
                  LocalAPIProxyUN = ConfigurationManager.AppSettings["LocalAPIProxyUN"].ToString(),
                  LocalAPIProxyPswd = ConfigurationManager.AppSettings["LocalAPIProxyPswd"].ToString(),

                  LocalAPIURL = ConfigurationManager.AppSettings["LocalAPIURL"].ToString(),


                  EmailAPIProxyHost = ConfigurationManager.AppSettings["EmailAPIProxyHost"].ToString(),
                  EmailAPIProxyUN = ConfigurationManager.AppSettings["EmailAPIProxyUN"].ToString(),
                  EmailAPIProxyPswd = ConfigurationManager.AppSettings["EmailAPIProxyPswd"].ToString(),
                  isProxyEnableForEmailAPI = Convert.ToBoolean(ConfigurationManager.AppSettings["isProxyEnableForEmailAPI"].ToString()),
                  EmailURL = ConfigurationManager.AppSettings["EmailURL"].ToString(),

                  PreArrivalConfirmationEmail = ConfigurationManager.AppSettings["PreArrivalConfirmationEmail"].ToString(),
                  PreArrivalConfirmationEmailSubject = ConfigurationManager.AppSettings["PreArrivalConfirmationEmailSubject"].ToString(),
                  // LogLocation = ConfigurationManager.AppSettings["LogLocation"].ToString(),

                  PreCheckoutFolioEmail = ConfigurationManager.AppSettings["PreCheckoutFolioEmail"].ToString(),
                  PreCheckoutFolioEmailSubject = ConfigurationManager.AppSettings["PreCheckoutFolioEmailSubject"].ToString(),

                  DueINBufferDays = Convert.ToInt32(ConfigurationManager.AppSettings["DueINBufferDays"].ToString()),


                  PreCheckoutFromEmail = ConfigurationManager.AppSettings["PreCheckoutFromEmail"].ToString(),
                  PreCheckoutEmailSubject = ConfigurationManager.AppSettings["PreCheckoutEmailSubject"].ToString(),
                  isAutoCheckOutEnabled = ConfigurationManager.AppSettings["isAutoCheckOutEnabled"].ToString(),
                  sendFolioFromOpera = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["sendFolioFromOpera"]) ? bool.Parse(ConfigurationManager.AppSettings["sendFolioFromOpera"]) : false

              },
              ReportConfig = new Models.ReportConfigurationModel()
              {
                  ReportBaseURL = ConfigurationManager.AppSettings["reportUrlBase"].ToString(),
              },
              smartTapConfig = new Models.SmartTapConfigModel()
              {
                  SmartTapBaseURL = ConfigurationManager.AppSettings["SmartTapURL"].ToString(),
                  isProxyEnableForSmartTapCloudAPI = Convert.ToBoolean(ConfigurationManager.AppSettings["IsProxyEnabledForSMartTapAPI"].ToString()),
                  SmartTapCloudAPIProxyHost  = ConfigurationManager.AppSettings["SmartTapAPIProxyHost"].ToString(),
                  SmartTapCloudAPIProxyUN = ConfigurationManager.AppSettings["SmartTapAPIProxyUN"].ToString(),
                  SmartTapCloudAPIProxyPswd = ConfigurationManager.AppSettings["SmartTapProxyPswd"].ToString(),
              }
    

          });
    

        public static ConfigurationHelper Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        public Models.OwsConfigurationModel OWSConfig { get; set; }

        public Models.ReportConfigurationModel ReportConfig { get; set; }

        public Models.GrabberConfigModel GrabberConfig { get; set; }

        public Models.SmartTapConfigModel smartTapConfig { get; set; }
    }
}