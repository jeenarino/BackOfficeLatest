using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models.SmartTap
{
    public class CumulativeReponseModel
    {
           public int ID { get; set; }
            public string ReservationNo { get; set; }
            public string ReservationNameID { get; set; }
            public string ShareID { get; set; }
            public string GuestFullName { get; set; }
            public string GuestTitle { get; set; }
            public string RoomNo { get; set; }
            public int AdultCount { get; set; }
            public int ChildCount { get; set; }
            public int MemberShipID { get; set; }
            public string MembershipNo { get; set; }
            public System.DateTime CheckinDate { get; set; }
            public System.DateTime CheckOutDate { get; set; }
            public string UDF1 { get; set; }
            public string Package { get; set; }
            public int TotalRecords { get; set; }  
            public string OutletName { get; set; }
            public DateTime TransactionDate { get; set; }
            public int ConsumedCount { get; set; }
            public string PackageType { get; set; } 
    }
}