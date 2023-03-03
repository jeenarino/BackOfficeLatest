using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models.SmartTap
{
    public class OutletUser
    {
        public int? ID { get; set; }
        [Required(ErrorMessage = "User Name is required",AllowEmptyStrings =false)]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9- ]+$", ErrorMessage = "Name accepts only Alphabets")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required",AllowEmptyStrings = false)]
       
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and maximum of 15 characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,16}$", ErrorMessage = "Password must contain 8 to 16 characters with 1 lower case, 1 upper case, 1 number,1 special character.")]
        public string Password {get; set; }
        [Required(ErrorMessage = "ConfirmPassword is required", AllowEmptyStrings = false)]
       
        [Compare("Password", ErrorMessage = "Password does not match")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,16}$", ErrorMessage = "Password must contain 8 to 16 characters with 1 lower case, 1 upper case, 1 number,1 special character.")]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} and maximum of 15 characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public  List<Outlet> Outlet { get; set; }
       
        public string SelectedOutlets { get; set; } 
        public int TotalRecords { get; set; }
        public bool? IsActive { get; set; }
    }
}