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
    public class CountryMastersController : BaseController
    {
        private PortalDBEntities db = new PortalDBEntities();

        // GET: CountryMasters
        public ActionResult Index()
        {
            return View(db.tbCountryMasters.ToList());
        }

        // GET: CountryMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCountryMaster tbCountryMaster = db.tbCountryMasters.Find(id);
            if (tbCountryMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbCountryMaster);
        }

        // GET: CountryMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CountryMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryMasterID,Country_Full_name,Country_3Char_code,Country_2Char_code")] tbCountryMaster tbCountryMaster)
        {
            if (ModelState.IsValid)
            {
                db.tbCountryMasters.Add(tbCountryMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbCountryMaster);
        }

        // GET: CountryMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCountryMaster tbCountryMaster = db.tbCountryMasters.Find(id);
            if (tbCountryMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbCountryMaster);
        }

        // POST: CountryMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CountryMasterID,Country_Full_name,Country_3Char_code,Country_2Char_code")] tbCountryMaster tbCountryMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbCountryMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbCountryMaster);
        }

        // GET: CountryMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbCountryMaster tbCountryMaster = db.tbCountryMasters.Find(id);
            if (tbCountryMaster == null)
            {
                return HttpNotFound();
            }
            return View(tbCountryMaster);
        }

        // POST: CountryMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbCountryMaster tbCountryMaster = db.tbCountryMasters.Find(id);
            db.tbCountryMasters.Remove(tbCountryMaster);
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
