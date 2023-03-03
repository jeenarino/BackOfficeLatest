using CheckinPortal.BackOffice.BusinessLogic;
using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Helpers;
using CheckinPortal.BackOffice.Models;
using CheckinPortal.BackOffice.Models.SmartTap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class GeneralSettingsController : BaseController
    {
        // GET: GeneralSettings
        SmartTapLogic smartlogic = new SmartTapLogic();

        public async Task<ActionResult> Create()
        {
           GeneralSetting general=new GeneralSetting();
            var generalcreateresponse = await smartlogic.GetGeneralSetings();
            if (generalcreateresponse != null)
            {
                general = (GeneralSetting)generalcreateresponse.Data;
               
            }
            return View(general);
        }

        [HttpPost]
        public async Task<ActionResult> Create(GeneralSetting  general)
        {
            if (ModelState.IsValid)
            {

                List<PackageTypeLevelSetting> packagelist = new List<PackageTypeLevelSetting>();
                PackageTypeLevelSetting setting = new PackageTypeLevelSetting();
                setting.PackageType = "BreakfastPackage";
                setting.IsMemberShipLevel = general.BIsMemberShipLevel;
                setting.IsPackageLevel = general.BIsPackageLevel;
                packagelist.Add(setting);
                PackageTypeLevelSetting setting2 = new PackageTypeLevelSetting();
                setting2.PackageType = "ComplementaryPackage";
                setting2.IsMemberShipLevel = general.CIsMemberShipLevel;
                setting2.IsPackageLevel = general.CIsPackageLevel;
                packagelist.Add(setting2);
                PackageTypeLevelSetting setting3 = new PackageTypeLevelSetting();
                setting3.PackageType = "GeneralPackageType";
                setting3.IsMemberShipLevel = general.GIsMemberShipLevel;
                setting3.IsPackageLevel = general.GIsPackageLevel;
                packagelist.Add(setting3);
                general.PackageTypeLevels = packagelist;

                var ouletcreateresponse = await smartlogic.InsertGeneralSettings(general);
                if (ouletcreateresponse != null)
                {
                    return RedirectToAction("Create");
                }

            }

            return RedirectToAction("Create");
        }

    }
}