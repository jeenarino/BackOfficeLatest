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
    
    public partial class usp_GetReservationDocuments_Result
    {
        public Nullable<int> ReservationDetailID { get; set; }
        public string ReservationNameID { get; set; }
        public string ReservationNumber { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentType { get; set; }
        public int ReseDocTypeMasterID { get; set; }
        public byte[] Document { get; set; }
        public int ReservationdocumentID { get; set; }
    }
}
