using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models.SmartTap
{
    public class GeneralSetting
    {
        public int? SettingsId { get; set; }
        public bool ComplementaryPackage { get; set; }
        public bool GeneralPackage { get; set; }      
        public bool BreakfastPackage { get; set; }
        [Required(ErrorMessage = "BreakfastShiftTime is required", AllowEmptyStrings = false)]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string BreakfastShiftTime { get; set; }
        [Required(ErrorMessage = "LunchShiftTime is required", AllowEmptyStrings = false)]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string LunchShiftTime { get; set; }
        [Required(ErrorMessage = "DinnerShiftTime is required", AllowEmptyStrings = false)]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string DinnerShiftTime { get; set; }
        [Required(ErrorMessage = "MidnightShiftTime is required", AllowEmptyStrings = false)]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string MidnightShiftTime { get; set; }
        public bool CIsMemberShipLevel { get; set; }    
        public bool CIsPackageLevel { get; set; }
        public bool GIsMemberShipLevel { get; set; }
        public bool GIsPackageLevel { get; set; }
        public bool BIsMemberShipLevel { get; set; }
        public bool BIsPackageLevel { get; set; }
        
        public  List<PackageTypeLevelSetting> PackageTypeLevels { get; set; }
    }
}