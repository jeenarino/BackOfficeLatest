using CheckinPortal.BackOffice.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class GrabberController : Controller
    {
        // GET: Grabber
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendPrecheckinLink(string ConfirmationNo)
        {
            BusinessLogic.GrabberLogics pushReservationLogic = new GrabberLogics();
            var result = await GrabberLogics.PushDueInReservation(ConfirmationNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SendPrecheckoutLink(string ConfirmationNo)
        {
            BusinessLogic.GrabberLogics pushReservationLogic = new GrabberLogics();
            var result = await GrabberLogics.PushDueOutReservation(ConfirmationNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> FetchReservation(string ConfirmationNo)
        {
            //BusinessLogic.GrabberLogics pushReservationLogic = new GrabberLogics();
            //await GrabberLogics.FetchPreCheckedInReservation();
            return Json(new { result=true, message ="" }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> FetchPreCheckinReservationStatus(string ConfirmationNo)
        {
            BusinessLogic.GrabberLogics pushReservationLogic = new GrabberLogics();
            new MCIGrabberService.Helper.LogHelper().Log($"Fetching Precheckin reservation status for Confirmation no {ConfirmationNo}", ConfirmationNo, "Fetch PreCheckin Status", "Backoffice", "PreCheckin Fetch");
            var Response = await GrabberLogics.FetchPreCheckionReservationStatus(ConfirmationNo);

            new MCIGrabberService.Helper.LogHelper().Log($"Fetching Precheckin reservation status response for Confirmation No : {ConfirmationNo} is {Response.result}", ConfirmationNo, "Fetch PreCheckin Status", "Backoffice", "PreCheckin Fetch");
            new MCIGrabberService.Helper.LogHelper().Debug($"Fetching Precheckin reservation status response is {Newtonsoft.Json.JsonConvert.SerializeObject(Response)}", ConfirmationNo, "Fetch PreCheckin Status", "Backoffice", "PreCheckin Fetch");

            return Json(new { result = Response.result, data = Response.responseData }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> FetchPreCheckinReservation(string ConfirmationNo)
        {
            BusinessLogic.GrabberLogics pushReservationLogic = new GrabberLogics();
            new MCIGrabberService.Helper.LogHelper().Log($"Fetching Precheckin reservation for Confirmation no {ConfirmationNo}", ConfirmationNo, "Fetch PreCheckin", "Backoffice", "PreCheckin Fetch");
            var Response = await GrabberLogics.FetchPreCheckinReservation(ConfirmationNo);
            new MCIGrabberService.Helper.LogHelper().Log($"Fetching Precheckin reservation response for Confirmation No : {ConfirmationNo} is {Response.result}", ConfirmationNo, "Fetch PreCheckin", "Backoffice", "PreCheckin Fetch");
            new MCIGrabberService.Helper.LogHelper().Debug($"Fetching Precheckin reservation response is {Newtonsoft.Json.JsonConvert.SerializeObject(Response)}", ConfirmationNo, "Fetch PreCheckin", "Backoffice", "PreCheckin Fetch");

            return Json(new { result = Response.result, data = Response.responseData }, JsonRequestBehavior.AllowGet);
        }

        
        public async Task<ActionResult> FetchPreCheckoutReservationStatus(string ConfirmationNo)
        {
            BusinessLogic.GrabberLogics pushReservationLogic = new GrabberLogics();
            new MCIGrabberService.Helper.LogHelper().Log($"Fetching Precheckout reservation for Confirmation no {ConfirmationNo}", ConfirmationNo, "Fetch PreCheckout", "Backoffice", "PreCheckout Fetch");
            var Response = await GrabberLogics.FetchPreCheckoutReservation(ConfirmationNo);
            new MCIGrabberService.Helper.LogHelper().Log($"Fetching Precheckout reservation response for Confirmation No : {ConfirmationNo} is {Response.result}", ConfirmationNo, "Fetch PreCheckout", "Backoffice", "PreCheckout Fetch");
            new MCIGrabberService.Helper.LogHelper().Debug($"Fetching Precheckout reservation response is {Newtonsoft.Json.JsonConvert.SerializeObject(Response)}", ConfirmationNo, "Fetch PreCheckout", "Backoffice", "PreCheckout Fetch");

            return Json(new { result = Response.result, data = Response.responseData }, JsonRequestBehavior.AllowGet);
        }


    }
}