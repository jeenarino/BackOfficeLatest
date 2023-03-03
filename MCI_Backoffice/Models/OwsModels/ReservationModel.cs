using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models.OwsModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class RoomDetails
    {
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public string RoomTypeDescription { get; set; }
        public string RoomTypeShortDescription { get; set; }
        public string RoomStatus { get; set; }
        public string RTC { get; set; }
        public string RTCDescription { get; set; }
        public string RTCShortDescription { get; set; }

    }
    public class DailyRate
    {
        public DateTime PostingDate { get; set; }
        public double Amount { get; set; }
        public string description { get; set; }
        public bool IsTaxAmount { get; set; }

    }
    public class RateDetails
    {
        public string RateCode { get; set; }
        public double RateAmount { get; set; }
        public List<DailyRate> DailyRates { get; set; }
        public bool IsMultipleRate { get; set; }

    }
    public class PaymentMethod
    {
        public string PaymentType { get; set; }
        public object MaskedCardNumber { get; set; }
        public object ExpiryDate { get; set; }
        public object AprovalCode { get; set; }

    }
    public class UserDefinedField
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }

    }
    public class Phone
    {
        public string phoneType { get; set; }
        public string phoneRole { get; set; }
        public int operaId { get; set; }
        public bool primary { get; set; }
        public int displaySequence { get; set; }
        public string PhoneNumber { get; set; }

    }
    public class Address
    {
        public string addressType { get; set; }
        public int operaId { get; set; }
        public bool primary { get; set; }
        public int displaySequence { get; set; }
        public string address1 { get; set; }
        public object address2 { get; set; }
        public string city { get; set; }
        public object state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }

    }
    public class Email
    {
        public string emailType { get; set; }
        public int operaId { get; set; }
        public bool primary { get; set; }
        public int displaySequence { get; set; }
        public string email { get; set; }

    }
    public class GuestProfile
    {
        public string PmsProfileID { get; set; }
        public object FamilyName { get; set; }
        public object GivenName { get; set; }
        public string GuestName { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public string PassportNumber { get; set; }
        public string DocumentType { get; set; }
        public bool IsPrimary { get; set; }
        public object MembershipType { get; set; }
        public object MembershipNumber { get; set; }
        public object MembershipID { get; set; }
        public object MembershipName { get; set; }
        public object MembershipClass { get; set; }
        public object MembershipLevel { get; set; }
        public string FirstName { get; set; }
        public object MiddleName { get; set; }
        public string LastName { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Address> Address { get; set; }
        public List<Email> Email { get; set; }
        public string BirthDate { get; set; }
        public object IssueDate { get; set; }
        public object IssueCountry { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public object VipCode { get; set; }

    }
    public class Alert
    {
        public string AlertID { get; set; }
        public string AlertCode { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public bool isPrinterNotificationEnabled { get; set; }
        public bool isScreenNotificationEnabled { get; set; }
        public bool isGlobal { get; set; }

    }
    public class ResponseData
    {
        public object ConfirmationNumber { get; set; }
        public string ReservationNumber { get; set; }
        public string ReservationNameID { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int Adults { get; set; }
        public int Child { get; set; }
        public string ReservationStatus { get; set; }
        public string ComputedReservationStatus { get; set; }
        public object LegNumber { get; set; }
        public object ChainCode { get; set; }
        public DateTime? ExpectedDepartureTime { get; set; }
        public DateTime? ExpectedArrivalTime { get; set; }
        public string ReservationSourceCode { get; set; }
        public string ReservationType { get; set; }
        public bool? PrintRate { get; set; }
        public bool? NoPost { get; set; }
        public object DoNotMoveRoom { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalTax { get; set; }
        public bool IsTaxInclusive { get; set; }
        public double CurrentBalance { get; set; }
        public RoomDetails RoomDetails { get; set; }
        public RateDetails RateDetails { get; set; }
        public object PartyCode { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public object IsPrimary { get; set; }
        public object ETA { get; set; }
        public object FlightNo { get; set; }
        public object IsCardDetailPresent { get; set; }
        public object IsDepositAvailable { get; set; }
        public object IsPreCheckedInPMS { get; set; }
        public object IsSaavyPaid { get; set; }
        public List<ResponseData> SharerReservations { get; set; }
        public List<object> DepositDetail { get; set; }
        public List<object> PreferanceDetails { get; set; }
        public List<object> PackageDetails { get; set; }
        public List<UserDefinedField> userDefinedFields { get; set; }
        public List<GuestProfile> GuestProfiles { get; set; }
        public List<Alert> Alerts { get; set; }
        public bool IsMemberShipEnrolled { get; set; }
        public object IsBreakFastAvailable { get; set; }
        public object reservationDocument { get; set; }
        public object GuestSignature { get; set; }

    }
    public class OwsFetchReservationResponseModel
    {
        public List<ResponseData> responseData { get; set; }
        public bool result { get; set; }
        public string responseMessage { get; set; }
        public int statusCode { get; set; }

    }

}