using CheckinPortal.BackOffice.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    
    public class RoomTypeMasterController : Controller
    {
        private readonly CloudMastersLogic cloudMastersLogic = new CloudMastersLogic();

        // GET: RoomTypeMaster
        public async Task<ActionResult> Index()
        {
            var RoomTypeList = await cloudMastersLogic.GetRoomTypeMaster();
            return View(RoomTypeList);
        }

        public async Task<ActionResult> GetRoomTypeList()
        {
            var RoomTypeList =await cloudMastersLogic.GetRoomTypeMaster();
            return Json(new { Result = RoomTypeList.Count > 0, RoomTypeList = RoomTypeList },JsonRequestBehavior.AllowGet);
        }
    }
}