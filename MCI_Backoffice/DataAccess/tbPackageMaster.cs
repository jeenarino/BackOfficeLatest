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
    
    public partial class tbPackageMaster
    {
        public int PackageID { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public Nullable<decimal> PackageAmount { get; set; }
        public string PackageDesc { get; set; }
        public byte[] PackageImage { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}
