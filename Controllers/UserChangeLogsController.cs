using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BasicMVC.Models;

namespace BasicMVC.Controllers
{
    public class UserChangeLogsController : Controller
    {
        private BasicMVCEntities db = new BasicMVCEntities();

        // GET: UserChangeLogs
        public ActionResult Index()
        {
            return View(db.UserChangeLogs.ToList());
        }

        // GET: UserChangeLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserChangeLog userChangeLog = db.UserChangeLogs.Find(id);
            if (userChangeLog == null)
            {
                return HttpNotFound();
            }
            return View(userChangeLog);
        }

        // GET: UserChangeLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserChangeLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeID,FullNameOld,FullNameNew,EmailOld,EmailNew,PhoneOld,PhoneNew,Address1Old,Address1New,Address2Old,Address2New,CityOld,CityNew,StateOld,StateNew,ZipOld,ZipNew,CountryOld,CountryNew,TicketsOld,TicketsNew,MaxTicketsOld,MaxTicketsNew,RequestTicketsOld,RequestTicketsNew,RequestDateOld,RequestDateNew,RetireeOld,RetireeNew,RequireTransportationOld,RequireTransportationNew,AddDate,AddWho")] UserChangeLog userChangeLog)
        {
            if (ModelState.IsValid)
            {
                db.UserChangeLogs.Add(userChangeLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userChangeLog);
        }

        // GET: UserChangeLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserChangeLog userChangeLog = db.UserChangeLogs.Find(id);
            if (userChangeLog == null)
            {
                return HttpNotFound();
            }
            return View(userChangeLog);
        }

        // POST: UserChangeLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeID,FullNameOld,FullNameNew,EmailOld,EmailNew,PhoneOld,PhoneNew,Address1Old,Address1New,Address2Old,Address2New,CityOld,CityNew,StateOld,StateNew,ZipOld,ZipNew,CountryOld,CountryNew,TicketsOld,TicketsNew,MaxTicketsOld,MaxTicketsNew,RequestTicketsOld,RequestTicketsNew,RequestDateOld,RequestDateNew,RetireeOld,RetireeNew,RequireTransportationOld,RequireTransportationNew,AddDate,AddWho")] UserChangeLog userChangeLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userChangeLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userChangeLog);
        }

        // GET: UserChangeLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserChangeLog userChangeLog = db.UserChangeLogs.Find(id);
            if (userChangeLog == null)
            {
                return HttpNotFound();
            }
            return View(userChangeLog);
        }

        // POST: UserChangeLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserChangeLog userChangeLog = db.UserChangeLogs.Find(id);
            db.UserChangeLogs.Remove(userChangeLog);
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