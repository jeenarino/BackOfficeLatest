using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckinPortal.BackOffice.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Please enter user name")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberPassword { get; set; }
    }
}