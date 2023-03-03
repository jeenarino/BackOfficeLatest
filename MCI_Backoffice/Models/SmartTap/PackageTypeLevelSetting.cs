using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models.SmartTap
{
    public class PackageTypeLevelSetting
    {
       public int PackageTypeLevelSettingId { get; set; }
       public bool IsMemberShipLevel { get; set; }
       public bool IsPackageLevel { get; set; } 
       public string PackageType { get; set; }
    }
}