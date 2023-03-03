using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{
    public class UpdateRequestSstatusModel
    {
        public string Comments { get; set; }
        public int ReqDetailId { get; set; }
    }

    public class ChargeReservationModel
    {
        public string ReservationNumber { get; set; }
        public int RequestID { get; set; }
    }


}