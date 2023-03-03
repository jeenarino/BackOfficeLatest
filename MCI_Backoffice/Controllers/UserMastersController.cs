using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;
using CheckinPortal.BackOffice.Models;
using QRCoder;

namespace CheckinPortal.BackOffice.Controllers
{
    public class UserMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: UserMasters
        public ActionResult Index()
        {
            var tbUserMasters = new Helpers.DapperHelper().ExecuteSP<tbUserMaster>("usp_GetUserList");
            if (tbUserMasters != null)
            {
                tbUserMasters.ToList().ForEach(x => {
                    x.QrCodeImageStr = x.QrCodeImage==null?null:"data:image/png;base64," + Convert.ToBase64String(x.QrCodeImage);
                   



                });
            }

            return View(tbUserMasters.ToList());          
        }

        // GET: UserMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUserMaster tbUserMaster = db.tbUserMasters.Find(id);
            if (tbUserMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbUserMaster);
        }

        // GET: UserMasters/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.tbRoleMasters, "RoleID", "RoleName");
            tbUserMaster tb = new tbUserMaster();
            tb.RoleID = db.tbRoleMasters.Where(X => X.RoleName.ToUpper() == "Guest").FirstOrDefault().RoleID;

            tb.tbRoleMaster = new tbRoleMaster();
            tb.tbRoleMaster.RoleName = db.tbRoleMasters.Where(x => x.RoleName.ToUpper() == "Guest").FirstOrDefault().RoleName;

            tb.IsActive = true;
            return View(tb);
        }

        // POST: UserMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,RoleID,UserName,DisplayName,Password,ConfirmPassword,IsActive,QrCode")] tbUserMaster tbUserMaster)
        {
            if (ModelState.IsValid)
            {
                QRCodeGenerator ObjQr = new QRCodeGenerator();

                QRCodeData qrCodeData = ObjQr.CreateQrCode(tbUserMaster.QrCode, QRCodeGenerator.ECCLevel.Q);

                Bitmap bitMap = new QRCode(qrCodeData).GetGraphic(20);
                byte[] byteImage = null;
                using (MemoryStream ms = new MemoryStream())

                {

                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                   byteImage = ms.ToArray();

                    ViewBag.Url = "data:image/png;base64," + Convert.ToBase64String(byteImage);

                }

                string Uerid = null;
                var result = new Helpers.DapperHelper().ExecuteSP<UserResponse>("Usp_InsertUpdateUser", new
                {
                    
                    UserID = Uerid,
                    RoleID = tbUserMaster.RoleID,
                    UserName = tbUserMaster.UserName,
                    DisplayName = tbUserMaster.DisplayName,
                    Password = string.IsNullOrEmpty(tbUserMaster.Password)?"": tbUserMaster.Password,
                    IsActive = tbUserMaster.IsActive,
                    QrCode = tbUserMaster.QrCode,
                    QrCodeImage= byteImage

                }) ;

                var tbUserMasters = new Helpers.DapperHelper().ExecuteSP<tbUserMaster>("usp_GetUserList");
                if (tbUserMasters != null)
                {
                    tbUserMasters.ToList().ForEach(x => {
                        x.QrCodeImageStr = x.QrCodeImage == null ? null : "data:image/png;base64," + Convert.ToBase64String(x.QrCodeImage);




                    });
                }
                if (result.FirstOrDefault().userid != "0")
                {
                    ViewBag.userSucess = result.FirstOrDefault().userid;
                }
                return View("Index",tbUserMasters.ToList());
                
            }
            tbUserMaster.tbRoleMaster.RoleName = db.tbRoleMasters.Where(x => x.RoleID == tbUserMaster.RoleID).FirstOrDefault().RoleName;
            ViewBag.RoleID = new SelectList(db.tbRoleMasters, "RoleID", "RoleName", tbUserMaster.RoleID);
            return View(tbUserMaster);
        }

        // GET: UserMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tbUserMaster = new Helpers.DapperHelper().ExecuteSP<tbUserMaster>("usp_GetUserList", new { UserID =id}).FirstOrDefault();
            if (tbUserMaster != null)
            {

                tbUserMaster.QrCodeImageStr = tbUserMaster.QrCodeImage == null ? null : "data:image/png;base64," + Convert.ToBase64String(tbUserMaster.QrCodeImage);




               
            }
            if (tbUserMaster == null)
            {
                return HttpNotFound();
            }
            //tbUserMaster.ConfirmPassword = tbUserMaster.Password;
            ViewBag.RoleID = new SelectList(db.tbRoleMasters, "RoleID", "RoleName", tbUserMaster.RoleID);
            //ViewBag.Pswd = tbUserMaster.Password;
            return View(tbUserMaster);
        }

        // POST: UserMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,RoleID,UserName,DisplayName,IsActive,Password,QrCode")] tbUserMaster tbUserMaster)
        {
            var user = db.tbUserMasters.Where(x => x.UserID == tbUserMaster.UserID).FirstOrDefault();
            if (user != null)
            {
                try
                {

                    QRCodeGenerator ObjQr = new QRCodeGenerator();

                    QRCodeData qrCodeData = ObjQr.CreateQrCode(tbUserMaster.UserName, QRCodeGenerator.ECCLevel.Q);

                    Bitmap bitMap = new QRCode(qrCodeData).GetGraphic(20);
                    byte[] byteImage = null;
                    using (MemoryStream ms = new MemoryStream())

                    {

                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                        byteImage = ms.ToArray();

                        ViewBag.Url = "data:image/png;base64," + Convert.ToBase64String(byteImage);

                    }
                    user.DisplayName = tbUserMaster.DisplayName;
                    user.IsActive = tbUserMaster.IsActive;
                    user.UserName = tbUserMaster.UserName;
                    user.RoleID = tbUserMaster.RoleID;
                    user.Password = tbUserMaster.Password;
                    user.ConfirmPassword = user.Password;
                    user.QrCode = tbUserMaster.QrCode;
                    //db.Entry(tbUserMaster).State = EntityState.Modified;
                    var result = new Helpers.DapperHelper().ExecuteSP("Usp_InsertUpdateUser", new
                    {
                        UserID = user.UserID,
                        RoleID = user.RoleID,
                        UserName = user.UserName,
                        DisplayName = user.DisplayName,
                        Password = user.Password,
                        IsActive = user.IsActive,
                        QrCode = user.QrCode,
                        QrCodeImage = byteImage
                    });
                    var tbUserMasters = new Helpers.DapperHelper().ExecuteSP<tbUserMaster>("usp_GetUserList");
                    if (tbUserMasters != null)
                    {
                        tbUserMasters.ToList().ForEach(x => {
                            x.QrCodeImageStr = x.QrCodeImage == null ? null : "data:image/png;base64," + Convert.ToBase64String(x.QrCodeImage);




                        });
                    }
                   
                        ViewBag.userSucess = user.UserID;
                    
                    return View("Index", tbUserMasters.ToList());
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {

                    throw;
                }
            }
            else
            {
                ViewBag.RoleID = new SelectList(db.tbRoleMasters, "RoleID", "RoleName", tbUserMaster.RoleID);
                return View(tbUserMaster);
            }

        }

        // GET: UserMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUserMaster tbUserMaster = db.tbUserMasters.Find(id);
            if (tbUserMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbUserMaster);
        }

        // POST: UserMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbUserMaster tbUserMaster = db.tbUserMasters.Find(id);
            db.tbUserMasters.Remove(tbUserMaster);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       
        public async Task<ActionResult> QrDownload()
        {
            var tbUserMaster = new Helpers.DapperHelper().ExecuteSP<tbUserMaster>("usp_GetUserList", new { UserID = 22 }).FirstOrDefault();
            return File(tbUserMaster.QrCode, "application/pdf", tbUserMaster.UserName);

            
        }
        
        public ActionResult GetDownload(string userid)
        {
            var tbUserMaster = new Helpers.DapperHelper().ExecuteSP<tbUserMaster>("usp_GetUserList", new { UserID = userid }).FirstOrDefault();
            string fileName = tbUserMaster.UserName+".jpg";
            if (tbUserMaster.QrCodeImage != null || tbUserMaster.QrCodeImage != null)
            {
                var s = File(tbUserMaster.QrCodeImage, "System.Net.Mime.MediaTypeNames.Application.Octet", fileName);
                return s;
            }
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
