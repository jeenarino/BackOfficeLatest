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
    public class PaymentTypeMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: PaymentTypeMasters
        public ActionResult Index()
        {
            return View(db.tbPaymentTypeMasters.ToList());
        }

        // GET: PaymentTypeMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbPaymentTypeMaster tbPaymentTypeMaster = db.tbPaymentTypeMasters.Find(id);
            if (tbPaymentTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbPaymentTypeMaster);
        }

        // GET: PaymentTypeMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentTypeMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaymentTypeMasterID,PaymentType")] tbPaymentTypeMaster tbPaymentTypeMaster)
        {
            if (ModelState.IsValid)
            {
                db.tbPaymentTypeMasters.Add(tbPaymentTypeMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbPaymentTypeMaster);
        }

        // GET: PaymentTypeMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbPaymentTypeMaster tbPaymentTypeMaster = db.tbPaymentTypeMasters.Find(id);
            if (tbPaymentTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbPaymentTypeMaster);
        }

        // POST: PaymentTypeMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaymentTypeMasterID,PaymentType")] tbPaymentTypeMaster tbPaymentTypeMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbPaymentTypeMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbPaymentTypeMaster);
        }

        // GET: PaymentTypeMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbPaymentTypeMaster tbPaymentTypeMaster = db.tbPaymentTypeMasters.Find(id);
            if (tbPaymentTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbPaymentTypeMaster);
        }

        // POST: PaymentTypeMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbPaymentTypeMaster tbPaymentTypeMaster = db.tbPaymentTypeMasters.Find(id);
            db.tbPaymentTypeMasters.Remove(tbPaymentTypeMaster);
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
