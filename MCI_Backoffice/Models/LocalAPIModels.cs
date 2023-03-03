using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{

    public class LocalAPIRequestModel
    {
        public bool IsFromCloud { get; set; }
        public object RequestObject { get; set; }

    }

    public class LocalResponseModel
    {
        public object responseData { get; set; }
        public bool result { get; set; }
        public string responseMessage { get; set; }
        public int statusCode { get; set; }
    }

    

    public class KeyEncodeRequestModel
    {
        public string RoomNo { get; set; }
        public string CheckoutDate { get; set; }
        public string CheckoutTime { get; set; }
        public bool IsNewKey { get; set; }
        public string ReservationNo { get; set; }
        public string GuestName { get; set; }
        public string EncoderID { get; set; }
    }

    public class SendEmailRequestModel
    {

        /// <summary>
        /// 
        /// </summary>
        public string fromEmail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ToEmail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isPrecheckinEmail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string guestName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string confirmationNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BusinessLogic.EmailType EmailType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string displayFromEmail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AttchmentBase64 { get; set; }

        public string ItemName { get; set; }

        public string TotalAmount { get; set; }

    }
}