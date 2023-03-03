namespace CheckinPortal.BackOffice.Models
{
    public class OwsConfigurationModel
    {
        public string hotelDomain { get; set; }
        public string kioskID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string systemType { get; set; }
        public string language { get; set; }
        public string legNumber { get; set; }
        public string chainCode { get; set; }
        public string destinationEntityID { get; set; }
        public string destinationSystemType { get; set; }
        public string BaseURL { get; set; }
      //  public string CloudAPIURL { get; set; }
        public string LocalAPIURL { get; set; }

        public string EmailAPIURL { get; set; }
        public string SaavyBaseURL { get; set; }
        public string FromEmail { get; set; }
        public string DisplayFromEmail { get; set; }
        public string RequestAcceptedSubject { get; set; }
        public string RequestRejectedSubject { get; set; }
        public string OwsBaseURL { get; set; }

        public string preAuthUDF { get; set; }
        public string preAuthAmntUDF { get; set; }

        public string GarunteeTypeCode { get; set; }


        public int DueInBufferDays { get; set; }
        public bool IsBreakFastValidationWithUDF { get; set; }
        public bool IsBreakFastValidationWithPackage { get; set; }
        public string PackageCodes { get; set; }

        public string MealPlanFieldName { get; set; }
        

    }


    public class GrabberConfigModel
    {
        public string LOCALURL { get; set; }
        public string MealPlanFieldName { get; set; }


        public string PreAuthUDF { get; set; }
        public string PreAuthAmntUDF { get; set; }
     //   public bool IsPaymentDisabled { get; set; }
       // public string URL { get; set; }
     //   public string EMAILURL { get; set; }
        public string PreArrivalFromEmail { get; set; }
        public string PreArrivalEmailSubject { get; set; }
        public string EmailDisplayName { get; set; }
      //  public bool IsProxyEnabled { get; set; }
    //    public string ProxyURL { get; set; }
    //    public string ProxyUserName { get; set; }
     //   public string ProxyPassword { get; set; }

        public bool IsETADefault { get; set; }

        public bool isProxyEnableForCloudAPI { get; set; }
        public string CloudAPIProxyHost { get; set; }
        public string CloudAPIProxyUN { get; set; }
        public string CloudAPIProxyPswd { get; set; }
        public string CloudAPIURL { get; set; }

       
        public bool isProxyEnableForLocalAPI { get; set; }
        public string EmailAPIProxyHost { get; set; }
        public string EmailAPIProxyUN { get; set; }
        public string EmailAPIProxyPswd { get; set; }

        public string EmailURL { get; set; }



        public bool isProxyEnableForEmailAPI { get; set; }
        public string LocalAPIProxyHost { get; set; }
        public string LocalAPIProxyUN { get; set; }
        public string LocalAPIProxyPswd { get; set; }
        public string LocalAPIURL { get; set; }

        public string PreArrivalConfirmationEmail { get; set; }
        public string PreArrivalConfirmationEmailSubject { get; set; }
        public string isAutoCheckOutEnabled { get; set; }

        //public string LogLocation { get; set; }


        public string PreCheckoutFolioEmailSubject { get; set; }
        public string PreCheckoutFolioEmail { get; set; }

        public int DueINBufferDays { get; set; }


        public string PreCheckoutFromEmail { get; set; }
        public string PreCheckoutEmailSubject { get; set; }

        
        public bool sendFolioFromOpera { get; set; }
        public bool IsPaymentDisabled { get; set; }
    }
}