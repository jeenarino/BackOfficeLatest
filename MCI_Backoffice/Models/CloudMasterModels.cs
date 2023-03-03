using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckinPortal.BackOffice.Models
{

    public class CloudResponseModel
    {
        public object responseData { get; set; }
        public bool result { get; set; }
        public string responseMessage { get; set; }
        public int statusCode { get; set; }
    }

    public class RoomTypeMasterModel
    {
        public int RoomTypeID { get; set; }

        [Required(ErrorMessage ="Please enter room type code")]
        public string RoomTypeCode { get; set; }
        public string RoomTypeDescription { get; set; }
    }

    public class CloudPackageRequsetModel
    {
        public bool IsFromCloud { get; set; }
        public object RequestObject { get; set; }
    }

    public class CloudPackageListRequsetModel
    {
        public bool IsFromCloud { get; set; }
        public object RequestObject { get; set; }
    }


    public class FetchResrvationStatusModel
    {
        public string MessageCode { get; set; }
        public bool MessageStatus { get; set; }
        public string MessageDescription { get; set; }
    }

    public class CloudPackageMasterModel
    {
        public int PackageID { get; set; }

        [Display(Name ="Package Code (Max 10 Char)")]
        [Required(ErrorMessage ="Please enter valid package code")]
        public string PackageCode { get; set; }
        [Display(Name = "Package Name (Max 100 Char)")]
        [Required(ErrorMessage = "Please enter valid package name")]
        public string PackageName { get; set; }
        [Display(Name = "Package Amount")]
        [Required(ErrorMessage = "Please enter valid package Amount")]
        [DataType(DataType.Currency)]
        public decimal PackageAmount { get; set; }
        [Display(Name ="Package Description")]
        public string PackageDesc { get; set; }
        [Display(Name = "Package Image (270 x 170)")]
        public string PackageImage { get; set; }

        [Display(Name ="Is Active?")]
        public bool isActive { get; set; }

        [Display(Name ="Upsell Allowed Rooms Types")]
        public List<RoomTypeCode> RoomTypeCode { get; set; }

        [Display(Name ="Is Room Upsell Package ?")]
        public bool? isRoomUpsell { get; set; }

        [Display(Name ="Unit Description")]
        [Required(ErrorMessage ="Please enter description of the unit")]
        public string Units { get; set; }

    }

    public class RoomTypeCode
    {
        public string RoomCode { get; set; }
    }



}