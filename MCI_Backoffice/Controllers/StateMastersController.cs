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
    public class StateMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: StateMasters
        public ActionResult Index()
        {
            var tbStateMasters = db.tbStateMasters.Include(t => t.tbCountryMaster);
            return View(tbStateMasters.ToList());
        }

        // GET: StateMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbStateMaster tbStateMaster = db.tbStateMasters.Find(id);
            if (tbStateMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbStateMaster);
        }

        // GET: StateMasters/Create
        public ActionResult Create()
        {
            ViewBag.CountryMasterID = new SelectList(db.tbCountryMasters, "CountryMasterID", "Country_Full_name");
            return View();
        }

        // POST: StateMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StateMasterID,Statename,CountryMasterID")] tbStateMaster tbStateMaster)
        {
            if (ModelState.IsValid)
            {
                db.tbStateMasters.Add(tbStateMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CountryMasterID = new SelectList(db.tbCountryMasters, "CountryMasterID", "Country_Full_name", tbStateMaster.CountryMasterID);
            return View(tbStateMaster);
        }

        // GET: StateMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbStateMaster tbStateMaster = db.tbStateMasters.Find(id);
            if (tbStateMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryMasterID = new SelectList(db.tbCountryMasters, "CountryMasterID", "Country_Full_name", tbStateMaster.CountryMasterID);
            return View(tbStateMaster);
        }

        // POST: StateMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StateMasterID,Statename,CountryMasterID")] tbStateMaster tbStateMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbStateMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CountryMasterID = new SelectList(db.tbCountryMasters, "CountryMasterID", "Country_Full_name", tbStateMaster.CountryMasterID);
            return View(tbStateMaster);
        }

        // GET: StateMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbStateMaster tbStateMaster = db.tbStateMasters.Find(id);
            if (tbStateMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbStateMaster);
        }

        // POST: StateMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbStateMaster tbStateMaster = db.tbStateMasters.Find(id);
            db.tbStateMasters.Remove(tbStateMaster);
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
