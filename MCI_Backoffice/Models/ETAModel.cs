using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{
    public class ETAModel
    {
       
            public int ReservationDetailID { get; set; }
            public string ReservationNameID { get; set; }
            public string ReservationNumber { get; set; }
            public Nullable<System.DateTime> ArrivalDate { get; set; }
            public Nullable<System.DateTime> DepartureDate { get; set; }
            public Nullable<int> Adultcount { get; set; }
            public Nullable<int> Childcount { get; set; }
            public string MembershipNo { get; set; }
            public string MembershipType { get; set; }
            public Nullable<bool> IsDepositAvailable { get; set; }
            public Nullable<bool> IsCardDetailPresent { get; set; }
            public Nullable<int> CardDetailID { get; set; }
            public Nullable<bool> IsSaavyPaid { get; set; }
            public Nullable<System.DateTime> DataInsertedDate { get; set; }
            public Nullable<System.DateTime> Dataupdateddate { get; set; }
            public string FlightNo { get; set; }
            public Nullable<System.DateTime> ETA { get; set; }
            public string ReservationSource { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string RoomNumber { get; set; }
            public string RequestStatus { get; set; }
            public int TotalRecords { get; set; }   
            public string ETATime { get; set; }
            public string RoomType { get; set; }

    }
}