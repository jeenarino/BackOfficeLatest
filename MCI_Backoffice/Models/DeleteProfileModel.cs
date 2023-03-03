using System.Collections.Generic;

namespace CheckinPortal.BackOffice.Models
{
    public class DeleteProfileModel
    {
        public int ReservationDetailID { get; set; }
        public string ProfileDeleteReason { get; set; }
    }



    public class AssignRoomModel
    {
        public string ReservationNameID { get; set; }
        public string RoomNumber { get; set; }
    }


    public class ProfileSwapModel
    {
        public int fromProfileDetailId { get; set; }
        public int toProfileDetailId { get; set; }
    }



    public class UpdateProfileModel
    {
        public int ReservationDetailID { get; set; }
        public string EmailID { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string Nationality { get; set; }
        public int NationalityID { get; set; }
        public string AddressLine1 { get; set; }
        public int ProfileDetailID { get; set; }
        public string PMSProfileID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int StateMasterID { get; set; }

        public int CountryMasterID { get; set; }
        public string Country { get; set; }

        public string PostalCode { get; set; }

        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public string IssueDate { get; set; }
        public string ExpiryDate { get; set; }

        public string IssueCountry { get; set; }
    }

    public class AccpetRejectRequestModel
    {
        public int ReservationDetailID { get; set; }
        public string RoomNumber { get; set; }
        public int ReqDetailId { get; set; }

        public string Comments { get; set; }
    }
}