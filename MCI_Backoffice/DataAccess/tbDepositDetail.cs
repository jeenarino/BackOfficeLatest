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
    
    public partial class tbDepositDetail
    {
        public int DepositDetailID { get; set; }
        public Nullable<decimal> DepositAmount { get; set; }
        public Nullable<int> PaymentTypeMasterId { get; set; }
        public Nullable<int> CardTypeId { get; set; }
    }
}
