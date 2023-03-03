using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{
    public class SmartTapConfigModel
    {
        public string SmartTapBaseURL { get; set; }
        public bool isProxyEnableForSmartTapCloudAPI { get; set; }
        public string SmartTapCloudAPIProxyHost { get; set; }
        public string SmartTapCloudAPIProxyUN { get; set; }
        public string SmartTapCloudAPIProxyPswd { get; set; }
        
    }
}