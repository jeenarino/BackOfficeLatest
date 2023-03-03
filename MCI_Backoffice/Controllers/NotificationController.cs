using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Helpers;
using CheckinPortal.BackOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class NotificationController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();
        // GET: Notification
        public ActionResult Index(bool IsActionTaken=false)
        {
            
           ViewBag.IsActionTaken = IsActionTaken;
            ViewBag.Count = 0;
            var spResponse = new AuditReportHelper().fetchNotificationDetails("False", "", 1,10, "", "", "asc",0);

            if (spResponse != null && spResponse.result)
            {
                var notificationmodel = new List<NotificationModel>();

                notificationmodel = (List<NotificationModel>)spResponse.ResponseData;

                
                if (!IsActionTaken)
                {
                    ViewBag.Count = notificationmodel[0].TotalRecords;
                }
            }

            spResponse = new AuditReportHelper().fetchNotificationMaster();
            if(spResponse != null && spResponse.result)
            {
              var  notificationmaster = (List<NotificationMaster>)spResponse.ResponseData;
                ViewBag.NotificationTypeList = new SelectList(notificationmaster, "ID", "TYPEDESCRIPTION");
            }

                    return View();
        }

        public ActionResult GetNotificationListAjax(DataTableParameters model, Search search, string IsActionTaken = "False",int Notificationtype=0)
        {
            int start = 0;

            if (model.Start > 0)
            {
                start = model.Start / model.Length;
            }

            start += 1;

            string filterby = string.Empty;
            string soryOrder = "DESC";
            string sortBy = "";
            string sortColumn = "";

            if (Request.Params["search[value]"] != null)
            {
                filterby = Request.Params["search[value]"].ToString();
            }



            if (Request.Params["order[0][column]"] != null)
            {
                sortBy = Request.Params["order[0][column]"].ToString();
                
            }
            if (sortBy == "0")
            {
                sortColumn = "Created Date";
            }
            if (sortBy == "1")
            {
                sortColumn = "Notification Type";
            }
            else if (sortBy == "2")
            {
                sortColumn = "ReservationNameID";
            }
            else if(sortBy == "3")
            {
                sortColumn = "DeviceId";
            }
            else if (sortBy == "4")
            {
                sortColumn = "Message";
            }
           
            else if (sortBy == "5")
            {
                sortColumn = "Updated Date";
            }


            if (Request.Params["order[0][dir]"] != null)
            {
                
                soryOrder = Request.Params["order[0][dir]"].ToString();
               
            }
            var spResponse = new AuditReportHelper().fetchNotificationDetails(IsActionTaken,"",start, model.Length, filterby, sortColumn, soryOrder, Notificationtype);
            
            if (spResponse != null && spResponse.result)
            {
                var notificationmodel = new List<NotificationModel>();
                try
                {
                    notificationmodel = (List<NotificationModel>)spResponse.ResponseData;
                   
                    var TotalCount = notificationmodel[0].TotalRecords;
                       if(IsActionTaken=="False")
                    {
                        ViewBag.Count = notificationmodel.Count;
                        }
                    var response = new
                    {
                        draw = model.draw,
                        data = notificationmodel,
                        recordsFiltered = TotalCount,
                        recordsTotal = TotalCount
                    };

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
               

            }
            else
            {
                var response = new
                {
                    draw = model.draw,
                    data = spResponse,
                    recordsFiltered = 0,
                    recordsTotal = 0
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult ApproveNotification(NotificationRequestDTO model)
        {
            if (string
                .IsNullOrEmpty(model.notificationID))
            {
              
               
                var result = new Helpers.DapperHelper().ExecuteSP("Usp_InsertNotification", new
                {
                    TYPECODE = "",
                    MESSAGE = "",
                    ISACTIONTAKEN = true,
                    ID = model.ID,
                    RESERVATIONNAME = "",
                    USERNAME = Session["UserName"].ToString(),
                    DEVICEID = ""
                });
            }
            else
            {
                var s = model.notificationID.Split(',');
                foreach (var item in s)
                {
                
                    var result = new Helpers.DapperHelper().ExecuteSP("Usp_InsertNotification", new
                    {
                        TYPECODE = "",
                        MESSAGE = "",
                        ISACTIONTAKEN = true,
                        ID = item,
                        RESERVATIONNAME = "",
                        USERNAME = Session["UserName"].ToString(),
                        DEVICEID = ""
                    });

                }

            }
            return RedirectToAction("Index", new { IsActionTaken = model.ISACTIONTAKEN }) ;
           
        }
    }
}