using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models.SmartTap
{
    public class Outlet
    {
       public int? OutletID { get; set; }
        [Required(ErrorMessage ="Outlet Name is required", AllowEmptyStrings = false)]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string OutletName { get; set; }
       public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsActive { get; set; }
        public int TotalRecords { get; set; }
    }

    public class OutletIDDetail
    {
        public int? OutletID { get; set; }
        public string OutletName { get; set; }
    }
}