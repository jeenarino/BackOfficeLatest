using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class SessionController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();
        // GET: Session
        public ActionResult Index()
        {
            var config = Helpers.ConfigurationHelper.Instance.OWSConfig.BaseURL;
            return View();
        }

        public async Task<ActionResult> Login(Models.LoginModel loginModel)
        {

            if (ModelState.IsValid)
            {
                
                    var user = db.tbUserMasters.Where(x => x.UserName == loginModel.UserName && x.Password == loginModel.Password && x.IsActive).FirstOrDefault();

                    if (user != null)
                    {
                        Session["LoggedInUser"] = user;
                        Session["UserName"] = user.DisplayName;
                        Session["UserID"] = user.UserID;
                        Session["RoleID"] = user.RoleID;
                    if (user.tbRoleMaster.RoleName == "SmartTap User")
                    {
                        return RedirectToAction("Index", "OutletMaster");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Reservations");
                    }
                    }
                    else
                    {
                    if (loginModel.UserName.ToUpper() == "ADMIN" || loginModel.UserName.ToUpper() == "CHAMPION")
                    {
                        ModelState.AddModelError("Password", "Invalid user name or password");
                        return View("Index"); }
                else if(!string.IsNullOrEmpty(loginModel.UserName) && !string.IsNullOrEmpty(loginModel.Password))
                {
                    // check user name using lov query
                    loginModel.UserName = loginModel.UserName.ToUpper();
                    loginModel.Password = loginModel.Password.ToUpper();
                    BusinessLogic.ReservationLogics reservationLogics = new BusinessLogic.ReservationLogics();

                    if (await reservationLogics.GetOperaBusinessDate(loginModel.UserName, loginModel.Password))
                    {
                        Session["LoggedInUser"] = new CheckinPortal.BackOffice.DataAccess.tbUserMaster { 
                            DisplayName = loginModel.UserName, UserName = loginModel.UserName, IsActive = true,
                             tbRoleMaster = new tbRoleMaster { IsAdmin = false } 

                        };
                        var roleid = db.tbRoleMasters.Where(x => x.RoleName.ToUpper() == "Guest").FirstOrDefault().RoleID;
                        Session["UserName"] = loginModel.UserName;
                        Session["UserID"] = loginModel.UserName;
                        Session["RoleID"] = roleid;
                        return RedirectToAction("Index", "Reservations");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Invalid opera user name or password");
                        return View("Index");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid opera user name or password");
                    return View("Index");
                }
                    }
               
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}