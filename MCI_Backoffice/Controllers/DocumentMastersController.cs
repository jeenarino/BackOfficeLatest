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
    public class DocumentMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: DocumentMasters
        public ActionResult Index()
        {
            return View(db.tbDocumentMasters.ToList());
        }

        // GET: DocumentMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDocumentMaster tbDocumentMaster = db.tbDocumentMasters.Find(id);
            if (tbDocumentMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbDocumentMaster);
        }

        // GET: DocumentMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocumentMasterID,DocumentCode,DocumentType")] tbDocumentMaster tbDocumentMaster)
        {
            if (ModelState.IsValid)
            {
                db.tbDocumentMasters.Add(tbDocumentMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbDocumentMaster);
        }

        // GET: DocumentMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDocumentMaster tbDocumentMaster = db.tbDocumentMasters.Find(id);
            if (tbDocumentMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbDocumentMaster);
        }

        // POST: DocumentMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocumentMasterID,DocumentCode,DocumentType")] tbDocumentMaster tbDocumentMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbDocumentMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbDocumentMaster);
        }

        // GET: DocumentMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDocumentMaster tbDocumentMaster = db.tbDocumentMasters.Find(id);
            if (tbDocumentMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbDocumentMaster);
        }

        // POST: DocumentMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbDocumentMaster tbDocumentMaster = db.tbDocumentMasters.Find(id);
            db.tbDocumentMasters.Remove(tbDocumentMaster);
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
