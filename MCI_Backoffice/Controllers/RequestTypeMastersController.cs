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
    public class RequestTypeMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: RequestTypeMasters
        public ActionResult Index()
        {
            return View(db.tbRequestTypeMasters.ToList());
        }

        // GET: RequestTypeMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbRequestTypeMaster tbRequestTypeMaster = db.tbRequestTypeMasters.Find(id);
            if (tbRequestTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbRequestTypeMaster);
        }

        // GET: RequestTypeMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestTypeMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestTypeMasterID,RequestType")] tbRequestTypeMaster tbRequestTypeMaster)
        {
            if (ModelState.IsValid)
            {
                db.tbRequestTypeMasters.Add(tbRequestTypeMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbRequestTypeMaster);
        }

        // GET: RequestTypeMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbRequestTypeMaster tbRequestTypeMaster = db.tbRequestTypeMasters.Find(id);
            if (tbRequestTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbRequestTypeMaster);
        }

        // POST: RequestTypeMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestTypeMasterID,RequestType")] tbRequestTypeMaster tbRequestTypeMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbRequestTypeMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbRequestTypeMaster);
        }

        // GET: RequestTypeMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbRequestTypeMaster tbRequestTypeMaster = db.tbRequestTypeMasters.Find(id);
            if (tbRequestTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbRequestTypeMaster);
        }

        // POST: RequestTypeMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbRequestTypeMaster tbRequestTypeMaster = db.tbRequestTypeMasters.Find(id);
            db.tbRequestTypeMasters.Remove(tbRequestTypeMaster);
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
