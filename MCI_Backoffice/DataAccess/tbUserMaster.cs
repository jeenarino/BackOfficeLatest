//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CheckinPortal.BackOffice.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbUserMaster
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public byte[] QrCodeImage { get; set; }
        public string QrCode { get; set; }
        public virtual tbRoleMaster tbRoleMaster { get; set; }
    }
}
