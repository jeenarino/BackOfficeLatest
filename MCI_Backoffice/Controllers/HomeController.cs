using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.Helpers;
using DigiDoc.DataAccess.Models;
using MCIGrabberService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SideMenu()
        {

            int roleid = 0;
            if (Session["RoleID"] != null)
            {
                roleid = Convert.ToInt32(Session["RoleID"].ToString());
            }
            var spResponse = ETAReportHelper.getUserProfileDetails(roleid.ToString());
            if (spResponse != null && spResponse.result && spResponse.ResponseData != null)
            {

                return View((List<UserRoleModel>)spResponse.ResponseData);
            }

            return View();
        }

    }
}