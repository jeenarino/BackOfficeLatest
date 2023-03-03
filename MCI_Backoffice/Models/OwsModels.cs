using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{
    public class OWSResponseModel
    {
        public object responseData { get; set; }
        public bool result { get; set; }
        public string responseMessage { get; set; }
        public int statusCode { get; set; }
    }

    public class OperaReservation
    {
        public string ConfirmationNumber { get; set; } //No Need to process
        public string ReservationNumber { get; set; } //Added
        public string ReservationNameID { get; set; }
        public DateTime? ArrivalDate { get; set; } //Added
        public DateTime? DepartureDate { get; set; } //Added        
        public DateTime? CreatedDateTime { get; set; } //Added
        public int? Adults { get; set; } //Addedd
        public int? Child { get; set; } //Addedd        
        public string ReservationStatus { get; set; } //New one
        public string ComputedReservationStatus { get; set; } //New one
        public string LegNumber { get; set; }
        public string ChainCode { get; set; }
        public DateTime? ExpectedDepartureTime { get; set; }
        public DateTime? ExpectedArrivalTime { get; set; }
        public string ReservationSourceCode { get; set; }

        public string ReservationType { get; set; }
        public bool? PrintRate { get; set; }
        public bool? NoPost { get; set; }
        public bool? DoNotMoveRoom { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalTax { get; set; }
        public bool IsTaxInclusive { get; set; }
        public decimal CurrentBalance { get; set; }
        public RoomDetails RoomDetails { get; set; }
        //public RateDetails RateDetails { get; set; }
        public string PartyCode { get; set; }
        //public PaymentMethod PaymentMethod { get; set; }
        public bool? IsPrimary { get; set; }
        public DateTime? ETA { get; set; }
        public string FlightNo { get; set; }
        public bool? IsCardDetailPresent { get; set; }
        public bool? IsDepositAvailable { get; set; }
        public bool? IsPreCheckedInPMS { get; set; }
        public bool? IsSaavyPaid { get; set; }
        public List<OperaReservation> SharerReservations { get; set; }
        //public List<DepositDetail> DepositDetail { get; set; }
        //public List<PreferanceDetails> PreferanceDetails { get; set; }
        //public List<PackageDetails> PackageDetails { get; set; }
        //public List<UserDefinedFields> userDefinedFields { get; set; }
        //public List<GuestProfile> GuestProfiles { get; set; }
       // public List<Alert> Alerts { get; set; }
        public bool IsMemberShipEnrolled { get; set; }
        //public ReservationDocument reservationDocument { get; set; }
        public string GuestSignature { get; set; }
    }

    public class FetchRoomList
    {
        [JsonProperty("roomTypes")]
        public string[] roomTypes { get; set; }

        [JsonProperty("departureDate")]
        public string departureDate { get; set; }
    }
    public class fetchRoomList
    {
        [JsonProperty("hotelDomain")]
        public string hotelDomain { get; set; }

        [JsonProperty("kioskID")]
        public string kioskID { get; set; }

        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("systemType")]
        public string systemType { get; set; }

        [JsonProperty("language")]
        public string language { get; set; }

        [JsonProperty("legNumber")]
        public string legNumber { get; set; }

        [JsonProperty("chainCode")]
        public string chainCode { get; set; }

        [JsonProperty("destinationEntityID")]
        public string destinationEntityID { get; set; }

        [JsonProperty("destinationSystemType")]
        public string destinationSystemType { get; set; }

        [JsonProperty("FetchRoomList")]
        public FetchRoomList FetchRoomList { get; set; }
    }

    public class RoomDetails
    {
        [JsonProperty("RoomNumber")]
        public string RoomNumber { get; set; }

        [JsonProperty("RoomType")]
        public string RoomType { get; set; }

        [JsonProperty("RoomTypeDescription")]
        public string RoomTypeDescription { get; set; }

        [JsonProperty("RoomTypeShortDescription")]
        public string RoomTypeShortDescription { get; set; }

        [JsonProperty("RoomStatus")]
        public string RoomStatus { get; set; }

        [JsonProperty("RTC")]
        public string RTC { get; set; }
    }

    public class FetRoomResponseModel
    {
        [JsonProperty("responseData")]
        public List<RoomDetails> responseData { get; set; }

        [JsonProperty("result")]
        public bool result { get; set; }

        [JsonProperty("responseMessage")]
        public string responseMessage { get; set; }

        [JsonProperty("statusCode")]
        public int statusCode { get; set; }
    }


    //public class OperaReservation
    //{

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string ReservationNameID { get; set; }
    //}


    public class FetchBookingRequest
    {

        /// <summary>
        /// 
        /// </summary>
        public string reservationNumber { get; set; }
    }

    public class CheckinIntoOperaModel
    {

        /// <summary>
        /// 
        /// </summary>
        public string hotelDomain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string kioskID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string systemType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string legNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chainCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string destinationEntityID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string destinationSystemType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OperaReservation OperaReservation { get; set; }

        public FetchBookingRequest FetchBookingRequest { get; set; }
    }

    public class AssignRoomRequestModel
    {

        /// <summary>
        /// 
        /// </summary>
        public string hotelDomain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string kioskID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string systemType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string legNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string chainCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string destinationEntityID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string destinationSystemType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AssignRoomAPIModel AssignRoomRequest { get; set; }
    }

    public class AssignRoomAPIModel
    {
        public string ReservationNameID { get; set; }
        public string RoomNumber { get; set; }
        public string StationID { get; set; }
    }


    public class UpdateProileRequestModel
    {

        public string hotelDomain { get; set; }
        public string kioskID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string systemType { get; set; }
        public string language { get; set; }
        public string legNumber { get; set; }
        public string chainCode { get; set; }
        public string destinationEntityID { get; set; }
        public string destinationSystemType { get; set; }

        public UpdateProileRequest updateProileRequest { get; set; }


    }


    public class UpdateProileRequest
    {
        public string ProfileID { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string nationality { get; set; }
        public string issueCountry { get; set; }
        public string documentNumber { get; set; }
        public string documentType { get; set; }
        public string issueDate { get; set; }


        public List<Emails> emails { get; set; }
        public List<Addresses> addresses { get; set; }
        public List<Phones> phones { get; set; }
    }

    public class DocInformation
    {
        public string dob { get; set; }
        public string gender { get; set; }
        public string nationality { get; set; }
        public string issueCountry { get; set; }
        public string documentNumber { get; set; }
        public string documentType { get; set; }
        public string issueDate { get; set; }
        public string expiryDate { get; set; }
    }

    public class Emails
    {
        public string email { get; set; }
        public bool primary { get; set; }
        public int displaySequence { get; set; }
    }

    public class Phones
    {
        public string PhoneNumber { get; set; }
        public string phoneType { get; set; }
        public string phoneRole { get; set; }
        public bool primary { get; set; }
        public int displaySequence { get; set; }
    }

    public class Addresses
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string addressType { get; set; }
        public bool primary { get; set; }
        public int displaySequence { get; set; }
    }

}