using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDoc.DataAccess.Models
{
    public class UserRoleModel
    {
       
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string ModuleMenuName { get; set; }
        public string ControllerName { get; set; }
        public string FunctionName { get; set; }
        public string ParentMenu { get; set; }
        public bool Status { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public string MenuIcon { get; set; }

    }
}
