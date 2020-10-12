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
    public class UserLoginsController : Controller
    {
        // Declare Database Entity
        private BasicMVCEntities db = new BasicMVCEntities();

        // Declare DTO Class
        private class HomeInformationDTO : HomeInformation { }

        // GET: Userlogins
        public ActionResult Index()
        {
            return View();
        }

        // GET: Userlogins/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userlogin userlogin = db.Userlogins.Find(id);
            if (userlogin == null)
            {
                return HttpNotFound();
            }
            return View(userlogin);
        }


        // GET: Userlogins/Create
        public ActionResult Create()
        {
            // Declare Home Information List.
            List<HomeInformation> list = db.HomeInformations.ToList();

            // Declare ViewBag using list.
            ViewBag.HomeList = new SelectList(list, "HomeInformationID", "Message");

            // Declare new Home Information list and add values from database into the list. (use temporary DTO instead of og class instance)
            List<HomeInformationDTO> HomeInfo = db.HomeInformations.Select(x => new HomeInformationDTO { HomeInformationID = x.HomeInformationID, Message = x.Message }).ToList();

            // Set the ViewBag equal to our new Home Information list that contains values from the database.
            ViewBag.HomeList = HomeInfo;

            // Now that our ViewBag is set. Go back to view.
            return View();
        }


        // POST: Userlogins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Password")] Userlogin userlogin)
        {
            // Declare Home Information List.
            List<HomeInformation> list = db.HomeInformations.ToList();

            // Declare ViewBag using list.
            ViewBag.HomeList = new SelectList(list, "HomeInformationID", "Message");

            // Declare new Home Information list and add values from database into the list. (use temporary DTO instead of og class instance)
            List<HomeInformationDTO> HomeInfo = db.HomeInformations.Select(x => new HomeInformationDTO { HomeInformationID = x.HomeInformationID, Message = x.Message }).ToList();

            // Set the ViewBag equal to our new Home Information list that contains values from the database.
            ViewBag.HomeList = HomeInfo;

            if (ModelState.IsValid)
            {
                //db.Userlogins.Add(userlogin);
                // is admin?
                if (db.Userlogins.ToList().Where(x => x.IsAdmin == true && x.UserId == userlogin.UserId && x.AdminPass == userlogin.Password).Count() > 0)
                    //return HttpNotFound();
                    return RedirectToAction("Index", "UserTicketForms", new { area = "" });
                else
                    if (db.Userlogins.ToList().Where(x => x.LockoutEnabled == false && x.UserId == userlogin.UserId && x.Password == userlogin.Password).Count() > 0)
                    return RedirectToAction("Edit", "UserTicketForms", new { id = userlogin.UserId });
                else if (db.Userlogins.ToList().Where(x => x.LockoutEnabled == true && x.UserId == userlogin.UserId && x.Password == userlogin.Password).Count() > 0)
                    ViewBag.ErrorMessage = "You cannot request more than once.";
                else
                    ViewBag.ErrorMessage = "Invalid Employee ID or Zip Code.";
            }

            return View(userlogin);
        }

        // GET: Userlogins/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userlogin userlogin = db.Userlogins.Find(id);
            if (userlogin == null)
            {
                return HttpNotFound();
            }
            return View(userlogin);
        }

        // POST: Userlogins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Password,UserName,Email,EmailConfirmed,LockoutStartDate,LockoutEndDate,LockoutEnabled")] Userlogin userlogin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userlogin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Confirmation");
            }
            return View(userlogin);
        }

        // GET: Userlogins/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userlogin userlogin = db.Userlogins.Find(id);
            if (userlogin == null)
            {
                return HttpNotFound();
            }
            return View(userlogin);
        }

        // POST: Userlogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Userlogin userlogin = db.Userlogins.Find(id);
            db.Userlogins.Remove(userlogin);
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
        public ActionResult Confirmation()
        {
            return View();
        }
    }
}