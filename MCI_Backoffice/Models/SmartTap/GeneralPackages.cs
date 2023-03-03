using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models.SmartTap
{
    public class GeneralPackages
    {
        public int? PackageId { get; set; }
        [Required(ErrorMessage = "Package Description is Required",AllowEmptyStrings =false)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2}  amd maximum of 15 characters long.", MinimumLength = 5)]
        public string PackageDescription { get; set; }
        [Required(ErrorMessage = "Package Code is Required",AllowEmptyStrings =false)]
        [StringLength(5, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]

        public string PackageCode { get; set; }
        public bool IsActive { get; set; }
        public int TotalRecords { get; set; }
    }
}