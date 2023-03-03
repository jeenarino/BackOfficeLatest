using CheckinPortal.BackOffice.BusinessLogic;
using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CheckinPortal.BackOffice.Controllers
{
    public class PackageMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();
        private readonly CloudMastersLogic cloudMastersLogic = new CloudMastersLogic();
        // GET: PackageMasters
        public async Task<ActionResult> Index()
        {
            var Packages = await cloudMastersLogic.GetPackageMasterList(null);
            var RoomTypeList = await cloudMastersLogic.GetRoomTypeMaster();
            ViewBag.RoomTypeList = RoomTypeList;
            return View(Packages);
        }


        // GET: PackageMasters/Create
        public async Task<ActionResult> Create()
        {
            var RoomTypeList = await cloudMastersLogic.GetRoomTypeMaster();
            ViewBag.RoomTypeList = RoomTypeList;
            return View();
        }

        // POST: PackageMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PackageID,PackageCode,PackageName,PackageAmount,PackageDesc,isActive,isRoomUpsell,Units")] CloudPackageMasterModel cloudPackageMasterModel, HttpPostedFileBase PackageImage)
        {
            if (ModelState.IsValid)
            {
                byte[] pacImageByte;
                if (PackageImage != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        PackageImage.InputStream.CopyTo(ms);
                        pacImageByte = ms.ToArray();
                        cloudPackageMasterModel.PackageImage = Convert.ToBase64String(pacImageByte);
                    }
                }
                cloudPackageMasterModel.RoomTypeCode = new System.Collections.Generic.List<RoomTypeCode>();
                if (Request["RoomTypes"] != null)
                {
                    var RoomTypes = Request["RoomTypes"].ToString();
                    var typeList  = RoomTypes.Split(',');

                    foreach (var item in typeList)
                    {
                        cloudPackageMasterModel.RoomTypeCode.Add(new RoomTypeCode()
                        {
                            RoomCode = item
                        });
                    }
                }
                CloudPackageRequsetModel cloudPackageRequsetModel = new CloudPackageRequsetModel()
                {
                    IsFromCloud = true,
                    RequestObject = cloudPackageMasterModel
                };



                var PackageSaveresponse = await cloudMastersLogic.InsetUpdatePackageMaster(cloudPackageRequsetModel);
                if (PackageSaveresponse)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(cloudPackageMasterModel);
        }

        // GET: PackageMasters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Packages = (await cloudMastersLogic.GetPackageMasterList(id)).FirstOrDefault();
            var RoomTypeList = await cloudMastersLogic.GetRoomTypeMaster();
            ViewBag.RoomTypeList = RoomTypeList;

            if (Packages == null)
            {
                return HttpNotFound();
            }
            return View(Packages) ;
        }

        // POST: PackageMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PackageID,PackageCode,PackageName,PackageAmount,PackageDesc,isActive,isRoomUpsell,Units")] CloudPackageMasterModel cloudPackageMasterModel, HttpPostedFileBase PackageImage)
        {
            if (ModelState.IsValid)
            {
                byte[] pacImageByte;
                if (PackageImage != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        PackageImage.InputStream.CopyTo(ms);
                        pacImageByte = ms.ToArray();
                        cloudPackageMasterModel.PackageImage = Convert.ToBase64String(pacImageByte);
                    }
                }
                cloudPackageMasterModel.RoomTypeCode = new System.Collections.Generic.List<RoomTypeCode>();
                if (Request["RoomTypes"] != null)
                {
                    var RoomTypes = Request["RoomTypes"].ToString();
                    var typeList = RoomTypes.Split(',');

                    foreach (var item in typeList)
                    {
                        cloudPackageMasterModel.RoomTypeCode.Add(new RoomTypeCode()
                        {
                            RoomCode = item
                        });
                    }
                }
                CloudPackageRequsetModel cloudPackageRequsetModel = new CloudPackageRequsetModel()
                {
                    IsFromCloud = true,
                    RequestObject = cloudPackageMasterModel
                };

                var PackageSaveresponse = await cloudMastersLogic.InsetUpdatePackageMaster(cloudPackageRequsetModel);
                if (PackageSaveresponse)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(cloudPackageMasterModel);
        }

        // GET: PackageMasters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Packages = (await cloudMastersLogic.GetPackageMasterList(id)).FirstOrDefault();

            CloudPackageRequsetModel cloudPackageRequsetModel = new CloudPackageRequsetModel()
            {
                IsFromCloud = true,
                RequestObject = Packages
            };

            var isPackageDeleted = (await cloudMastersLogic.InsetUpdatePackageMaster(cloudPackageRequsetModel));

            return RedirectToAction("Index");
        }

        // POST: PackageMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbPackageMaster tbPackageMaster = db.tbPackageMasters.Find(id);
            db.tbPackageMasters.Remove(tbPackageMaster);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
