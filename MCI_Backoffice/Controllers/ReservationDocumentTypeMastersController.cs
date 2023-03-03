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
    public class ReservationDocumentTypeMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: ReservationDocumentTypeMasters
        public ActionResult Index()
        {
            return View(db.tbReservationDocumentTypeMasters.ToList());
        }

        // GET: ReservationDocumentTypeMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbReservationDocumentTypeMaster tbReservationDocumentTypeMaster = db.tbReservationDocumentTypeMasters.Find(id);
            if (tbReservationDocumentTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbReservationDocumentTypeMaster);
        }

        // GET: ReservationDocumentTypeMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReservationDocumentTypeMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReseDocTypeMasterID,DocumentTypeCode,DocumentType")] tbReservationDocumentTypeMaster tbReservationDocumentTypeMaster)
        {
            if (ModelState.IsValid)
            {
                db.tbReservationDocumentTypeMasters.Add(tbReservationDocumentTypeMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbReservationDocumentTypeMaster);
        }

        // GET: ReservationDocumentTypeMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbReservationDocumentTypeMaster tbReservationDocumentTypeMaster = db.tbReservationDocumentTypeMasters.Find(id);
            if (tbReservationDocumentTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbReservationDocumentTypeMaster);
        }

        // POST: ReservationDocumentTypeMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReseDocTypeMasterID,DocumentTypeCode,DocumentType")] tbReservationDocumentTypeMaster tbReservationDocumentTypeMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbReservationDocumentTypeMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbReservationDocumentTypeMaster);
        }

        // GET: ReservationDocumentTypeMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbReservationDocumentTypeMaster tbReservationDocumentTypeMaster = db.tbReservationDocumentTypeMasters.Find(id);
            if (tbReservationDocumentTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbReservationDocumentTypeMaster);
        }

        // POST: ReservationDocumentTypeMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbReservationDocumentTypeMaster tbReservationDocumentTypeMaster = db.tbReservationDocumentTypeMasters.Find(id);
            db.tbReservationDocumentTypeMasters.Remove(tbReservationDocumentTypeMaster);
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
