//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CheckinPortal.BackOffice.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbRequestDetail
    {
        public int ReqDetailId { get; set; }
        public Nullable<int> RequestTypeMasterID { get; set; }
        public Nullable<int> ReservationDetailID { get; set; }
        public string RequestType { get; set; }
        public Nullable<bool> ReqStatus { get; set; }
        public Nullable<System.DateTime> ReqCreationDate { get; set; }
        public Nullable<System.DateTime> ReqUpdateDate { get; set; }
        public string ReqUserComment { get; set; }
        public Nullable<int> userid { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string PackageDesc { get; set; }
        public Nullable<decimal> PackageAmount { get; set; }
        public string MembershipNumber { get; set; }
        public Nullable<bool> IsApproved { get; set; }
    }
}
