using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models.SaavyModels
{

    public class SaavyAPIResponseModel
    {
        public bool result { get; set; }
        public string Message { get; set; }
    }
    public class FetchPaymentTransactionResult
    {
        public int PaymentID { get; set; }
        public int TransactionID { get; set; }
        public string ReservationNumber { get; set; }
        public string ReservationNameID { get; set; }
        public string MaskedCardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string FundingSource { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string RecurringIdentifier { get; set; }
        public string AuthorisationCode { get; set; }
        public string pspReferenceNumber { get; set; }
        public string ParentPspRefereceNumber { get; set; }
        public string ResultCode { get; set; }
        public string ResponseMessage { get; set; }
        public bool IsActive { get; set; }
        public string TransactionType { get; set; }
        public string DisplayTransactionType { get; set; }
        public string StatusType { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CardType { get; set; }

        public string OperaPaymentTypeCode { get; set; }
    }

}