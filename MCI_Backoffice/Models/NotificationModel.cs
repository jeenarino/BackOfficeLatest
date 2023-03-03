using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{
    public class NotificationModel
    {

        public int ID { get; set; }
        public string TYPEDESCRIPTION { get; set; }
        public string RESERVATIONAME { get; set; }
        public string ReservationNumber { get; set; }
        public string MESSAGE { get; set; }
        public DateTime INSERTEDDATETIME { get; set; }
        public DateTime UPDATEDDATETIME { get; set; }
        public int TotalRecords { get; set; }
        public bool ISACTIONTAKEN { get; set; }
        public string DEVICEID { get; set; }

    }
    public class NotificationRequestDTO
    {

        public int ID { get; set; }
        public bool ISACTIONTAKEN { get; set; }
        public string notificationID { get; set; }

    }
}