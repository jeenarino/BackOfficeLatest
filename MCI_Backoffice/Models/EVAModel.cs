using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{
    public class EVARequestModel
    {
        public string ClientId { get; set; }
        public string ClientSecert { get; set; }
        public string webUrl { get; set; }
        public string accessToken { get; set; }
        public object RequestObject { get; set; }
    }
    public class EVAModel
    {
        public int ResponseID { get; set; }
        public string ReservationNameID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string IsMannualAutherized { get; set; }
        public string ResponseJSON { get; set; }
        public string CreatedAt { get; set; }
        public string ResponseDateTime { get; set; }
        public string TransactionId { get; set; }
        public string StatusCode{ get; set; }
        public string ErrorCodes{ get; set; }
        public string Message{ get; set; }
        public bool IsActive{ get; set; }
        public DateTime? CreatedDateTime{ get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string ReservationNumber{ get; set; }
        public DateTime?  ArrivalDate { get; set; }
        public DateTime?  DepartureDate { get; set; }
        public int TotalRecords { get; set; }
    }
    public class AccessTokenResponse
    {
        [JsonProperty("RefreshTokenExpiresIn")]
        public string RefreshTokenExpiresIn { get; set; }


        public List<string> ApiProductListJson { get; set; }


        public string OrganizationName { get; set; }


        public string DeveloperEmail { get; set; }


        public string TokenType { get; set; }


        public string IssuedAt { get; set; }


        public string ClientId { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }


        public string ApplicationName { get; set; }


        public string ExpiresIn { get; set; }


        public string RefreshCount { get; set; }


        public string Status { get; set; }
    }
    public class EVAImageVerification
    {

        public int ResponseID { get; set; }
        public bool IsActive{ get;set; }
        public string ReservationNameID { get; set; }
        public bool IsMannualAutherized { get; set; }
        public string ReservationNumber { get; set; }
        public string LivePhoto { get; set; }
        public byte[]  DocumentImage1 { get; set; }
        public byte[] DocumentImage2 { get; set; }
        public byte[] DocumentImage3 { get; set; }
        public byte[] FaceImage { get; set; }
        public int ProfileDetailID { get; set; }
        public Nullable<int> ProfileID { get; set; }
        public Nullable<int> ReservationDetailID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Nationality { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<System.DateTime> ArrivalDateTime { get; set; }
        public Nullable<System.DateTime> DepartureDateTime { get; set; }
        public string Gender { get; set; }
        public string DocumentType { get; set; }
        public string StatusCode { get; set; }

    }
    public class EVAResponseModel
    {
        public bool result { get; set; }
        public string responseMessage { get; set; }
        public object Response { get; set; }
        public string ResponseCode { get; set; }
    }
    internal class DateTimeFormatConverter : IsoDateTimeConverter
    {
        public DateTimeFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }

    internal class GenderConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            List<string> male = new List<string>() { "m", "male" };
            List<string> female = new List<string>() { "f", "female" };
            string gender = ((string)value).ToLower();

            writer.WriteValue(
                male.Any(s => gender.Equals(s)) ? "M"
                : female.Any(s => gender.Equals(s)) ? "F"
                : "U");
        }
    }
    #region Visitor Check-in Request
    public class VisitorCheckInRequest
    {
        [JsonProperty("visitor", NullValueHandling = NullValueHandling.Ignore)]
        public Visitor Visitor { get; set; }
    }

    public class Visitor
    {
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public VisitorInfo Info { get; set; }

        [JsonProperty("hotelDetails", NullValueHandling = NullValueHandling.Ignore)]
        public HotelDetails Hotel { get; set; }

        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public List<DocImage> Images { get; set; }
    }

    public class VisitorInfo
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }

        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(GenderConverter))]
        public string Gender { get; set; }

        [JsonProperty("passportNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportNumber { get; set; }

        [JsonProperty("dateOfBirth", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyyMMdd")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty("passportType", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportType { get; set; }

        [JsonProperty("checkIn", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyyMMdd")]
        public DateTime? CheckIn { get; set; }

        [JsonProperty("checkOut", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyyMMdd")]
        public DateTime? CheckOut { get; set; }

        [JsonProperty("mrzData", NullValueHandling = NullValueHandling.Ignore)]
        public string MRZ { get; set; }

        [JsonProperty("manuallyInputted")]
        public bool ManuallyInputted { get; set; }
    }

    public class HotelDetails
    {
        [JsonProperty("bookingId", NullValueHandling = NullValueHandling.Ignore)]
        public string BookingId { get; set; }

        [JsonProperty("hotelCode", NullValueHandling = NullValueHandling.Ignore)]
        public string HotelCode { get; set; }

        [JsonProperty("kioskId", NullValueHandling = NullValueHandling.Ignore)]
        public string KioskId { get; set; }

        [JsonProperty("kioskUserId", NullValueHandling = NullValueHandling.Ignore)]
        public string KioskUserId { get; set; }

        [JsonProperty("staffId", NullValueHandling = NullValueHandling.Ignore)]
        public string StaffId { get; set; }

        [JsonProperty("staffName", NullValueHandling = NullValueHandling.Ignore)]
        public string StaffName { get; set; }
    }

    public class DocImage
    {
        [JsonProperty("prefix_name", NullValueHandling = NullValueHandling.Ignore)]
        public string PrefixName { get; set; }

        [JsonProperty("base64_data", NullValueHandling = NullValueHandling.Ignore)]
        public string Base64Data { get; set; }
    }
    #endregion
    #region Visitor Check-in Response
    public class VisitorCheckInResponse
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public VisitorCheckInResponseData Data { get; set; }

        [JsonProperty("createdAt", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public VisitorCheckInResponseStatus Status { get; set; }
    }

    public class VisitorCheckInResponseData
    {
        [JsonProperty("errorCodeList", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ErrorCodeList { get; set; }

        [JsonProperty("errorMsg", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("responseDateTime", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(DateTimeFormatConverter), "yyyyMMddHHmmss")]
        public DateTime? ResponseDateTime { get; set; }

        [JsonProperty("resultCode", NullValueHandling = NullValueHandling.Ignore)]
        public int? ResultCode { get; set; }

        [JsonProperty("stbEvaTransactionId", NullValueHandling = NullValueHandling.Ignore)]
        public string TransactionId { get; set; }
    }

    public class VisitorCheckInResponseStatus
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public int? Code { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("errorCodeList", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ErrorCodeList { get; set; }

        [JsonProperty("debugId", NullValueHandling = NullValueHandling.Ignore)]
        public string DebugId { get; set; }


    }
    #endregion
    public class tbSTBResponse
    {
        public int ResponseID { get; set; }
        public string ReservationNameID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsMannualAutherized { get; set; }
        public string ResponseJSON { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ResponseDateTime { get; set; }
        public string TransactionId { get; set; }
        public int? StatusCode { get; set; }
        public string ErrorCodes { get; set; }
        public string Message { get; set; }
    }
    
}