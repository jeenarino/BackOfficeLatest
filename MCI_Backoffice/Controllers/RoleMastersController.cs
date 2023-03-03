using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CheckinPortal.BackOffice.Controllers.Base;
using CheckinPortal.BackOffice.DataAccess;

namespace CheckinPortal.BackOffice.Controllers
{
    public class RoleMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: RoleMasters
        public ActionResult Index()
        {
            return View(db.tbRoleMasters.ToList());
        }

        // GET: RoleMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbRoleMaster tbRoleMaster = db.tbRoleMasters.Find(id);
            if (tbRoleMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbRoleMaster);
        }

        // GET: RoleMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleID,RoleName,IsAdmin")] tbRoleMaster tbRoleMaster)
        {
            if (ModelState.IsValid)
            {
                db.tbRoleMasters.Add(tbRoleMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbRoleMaster);
        }

        // GET: RoleMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbRoleMaster tbRoleMaster = db.tbRoleMasters.Find(id);
            if (tbRoleMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbRoleMaster);
        }

        // POST: RoleMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleID,RoleName,IsAdmin")] tbRoleMaster tbRoleMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbRoleMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbRoleMaster);
        }

        // GET: RoleMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbRoleMaster tbRoleMaster = db.tbRoleMasters.Find(id);
            if (tbRoleMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbRoleMaster);
        }

        // POST: RoleMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbRoleMaster tbRoleMaster = db.tbRoleMasters.Find(id);
            db.tbRoleMasters.Remove(tbRoleMaster);
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
