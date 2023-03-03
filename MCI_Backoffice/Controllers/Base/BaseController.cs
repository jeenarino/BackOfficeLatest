using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CheckinPortal.BackOffice.Controllers.Base
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (controller == "Session")
            {
                //if its session controller, if user is already logged in redirect to home page                
                if (Session["LoggedInUser"] != null)
                {
                    if (action == "Logout")
                    {
                        //if logout call do nothing.
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "Controller", "Reservations" },
                            { "Action", "Index" }
                        });
                    }
                    
                }                
            }
            else
            {
                
                if (session.IsNewSession || Session["LoggedInUser"] == null)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = Json("Session Timeout", "appliation/json");
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", string.Empty);
                        ModelState.AddModelError("Password", "Session expired");
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                            { "Controller", "Session" },
                            { "Action", "Index" }
                        });
                    }
                }
            }
        }
    }
}