using BasicMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasicMVC.Controllers
{
    public class EnrollmentDataController : Controller
    {
        // GET: EnrollmentData
        private BasicMVCEntities BDB = new BasicMVCEntities();
        public ActionResult Index()
        {
            var dataForm = this.BDB.UserTicketForms;

            return this.View(dataForm);
        }
        //public ActionResult Browse(string userData)
        //{
        //    var dataModel = new UserData {
        //        name = userData
        //    }
        //    return View(dataModel);
        //}
        [ChildActionOnly]
        public ActionResult TicketFormList()
        {
            var dataForm = this.BDB.UserTicketForms.Take(14).ToList();

            return this.PartialView(dataForm);
        }
    }
}