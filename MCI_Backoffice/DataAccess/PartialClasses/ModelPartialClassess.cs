using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.DataAccess
{
    [MetadataType(typeof(tbCardTypeMasterMetadata))]
    public partial class tbCardTypeMaster
    {

    }

    public class tbCardTypeMasterMetadata
    {
        [Display(Name ="Card Type")]
        [Required(ErrorMessage ="Please enter valid Card Type")]
        public string CardTypeCode { get; set; }
    }






    public partial class EditUserMaster
    {
        [Required]
        public int UserID { get; set; }
        [Required(ErrorMessage ="Please select Role")]
        [Display(Name ="Role")]
        public int RoleID { get; set; }
        [Display(Name ="User Name")]
        [Required(ErrorMessage ="Please enter user name")]
        public string UserName { get; set; }

        //[Display(Name = "Password")]
        //[Required(ErrorMessage = "Please enter password")]
        //public string Password { get; set; }

        [Display(Name ="Display Name")]
        [Required(ErrorMessage ="Please enter valid display name")]
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

        public string QrCode { get; set; }
        public byte[] QrCodeImage { get; set; }
        public string QrCodeImageStr { get; set; }
    }




    [MetadataType(typeof(tbUserMasterMetadata))]
    public partial class tbUserMaster
    {
        public string ConfirmPassword { get; set; }
        

       // public byte[] QrCodeImage { get; set; }
        public string QrCodeImageStr { get; set; }

    }
    public class tbUserMasterMetadata
    {
        [Display(Name ="Role Type")]
        [Required(ErrorMessage ="Please select role type")]
        public int RoleID { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter user name")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        //[Required(ErrorMessage ="Please enter password")]
        [Display(Name ="Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        //[Required(ErrorMessage = "Please enter confirm password")]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Password dose not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name ="Display Name")]
        [Required(ErrorMessage ="Please enter valid display name")]
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Please enter valid QrCode")]
        public string QrCode { get; set; }
    }




   [MetadataType(typeof(tbRoleMastersMetadata))]
    public partial class tbRoleMaster
    {

    }

    public class tbRoleMastersMetadata
    {
        [Required(ErrorMessage ="Please enter valid role name")]
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }

        public bool IsAdmin { get; set; }
    }



    [MetadataType(typeof(tbStateMasterMetadata))]
    public partial class tbStateMaster
    {

    }

    public partial class tbStateMasterMetadata
    {
        [Required(ErrorMessage ="Please enter state name")]
        [Display(Name ="State Name")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "Please select country")]
        [Display(Name ="Country")]
        public Nullable<int> CountryMasterID { get; set; }

    }



    [MetadataType(typeof(tbRequestTypeMasterMetadata))]
    public partial class tbRequestTypeMaster
    {

    }

    public partial class tbRequestTypeMasterMetadata
    {
        [Required(ErrorMessage ="Please enter valid Request Type")]
        [Display(Name ="Request Type")]
        public string RequestType { get; set; }
    }


    [MetadataType(typeof(tbPaymentTypeMasterMetadata))]
    public partial class tbPaymentTypeMaster
    {
    }
    public partial class tbPaymentTypeMasterMetadata
    {
        [Display(Name ="Payment Type")]
        [Required(ErrorMessage ="Please enter valid payment type")]
        public string PaymentType { get; set; }
    }




    [MetadataType(typeof(tbReservationDocumentTypeMasterMetadata))]
    public partial class tbReservationDocumentTypeMaster
    {
    }
    public partial class tbReservationDocumentTypeMasterMetadata
    {
        [Required(ErrorMessage ="Please enter valid document type code")]
        [Display(Name ="Document Type Code")]
        public string DocumentTypeCode { get; set; }

        [Required(ErrorMessage = "Please enter valid document type")]
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; }
    }





    [MetadataType(typeof(tbDocumentMasterMetadata))]
    public partial class tbDocumentMaster
    {

    }

    public class tbDocumentMasterMetadata
    {
        [Display(Name ="Document Type Code")]
        [Required(ErrorMessage ="Please enter valid Document Type Code")]
        public string DocumentCode { get; set; }

        [Display(Name = "Document Type Name")]
        [Required(ErrorMessage = "Please enter valid Document Type Name")]
        public string DocumentType { get; set; }
    }

  [MetadataType(typeof(tbCountryMasterMetadata))]
    public partial class tbCountryMaster
    {

    }

    public class tbCountryMasterMetadata
    {
        [Display(Name ="Country Name")]
        [Required(ErrorMessage ="Please enter valid country name")]
        public string Country_Full_name { get; set; }

        [Display(Name = "Country Three Code")]
        [Required(ErrorMessage = "Please enter valid three char code")]
        [MinLength(3, ErrorMessage = "Must be three char")]
        [MaxLength(3, ErrorMessage = "Must be three char")]
        public string Country_3Char_code { get; set; }
        [Display(Name = "Country Two Code")]
        [Required(ErrorMessage = "Please enter valid two char code")]
        [MinLength(2,ErrorMessage ="Must be two char")]
        [MaxLength(2,ErrorMessage ="Must be two char")]
        public string Country_2Char_code { get; set; }
    }


}