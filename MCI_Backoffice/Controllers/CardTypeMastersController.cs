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
    public class CardTypeMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: CardTypeMasters
        public ActionResult Index()
        {
            return View(db.tbCardTypeMasters.ToList());
        }

        // GET: CardTypeMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCardTypeMaster tbCardTypeMaster = db.tbCardTypeMasters.Find(id);
            if (tbCardTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbCardTypeMaster);
        }

        // GET: CardTypeMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CardTypeMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CardTypeMasterID,CardTypeCode")] tbCardTypeMaster tbCardTypeMaster)
        {
            if (ModelState.IsValid)
            {
                db.tbCardTypeMasters.Add(tbCardTypeMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbCardTypeMaster);
        }

        // GET: CardTypeMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCardTypeMaster tbCardTypeMaster = db.tbCardTypeMasters.Find(id);
            if (tbCardTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbCardTypeMaster);
        }

        // POST: CardTypeMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CardTypeMasterID,CardTypeCode")] tbCardTypeMaster tbCardTypeMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbCardTypeMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbCardTypeMaster);
        }

        // GET: CardTypeMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCardTypeMaster tbCardTypeMaster = db.tbCardTypeMasters.Find(id);
            if (tbCardTypeMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbCardTypeMaster);
        }

        // POST: CardTypeMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbCardTypeMaster tbCardTypeMaster = db.tbCardTypeMasters.Find(id);
            db.tbCardTypeMasters.Remove(tbCardTypeMaster);
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
