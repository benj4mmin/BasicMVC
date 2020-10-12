using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasicMVC.Models;


namespace BasicMVC.Controllers
{
    public class HomeController : Controller
    {

        // Declare Database Entity.
        BasicMVCEntities db = new BasicMVCEntities();

        // Delcare DTO Class
        private class HomeInformationDTO : HomeInformation { }

        public ActionResult Index()
        {
            // Declare Home Information List.
            List<HomeInformation> list = db.HomeInformations.ToList();

            // Declare ViewBag using list.
            ViewBag.HomeList = new SelectList(list, "HomeInformationID", "Message");

            // Declare new Home Information list and add values from database into the list. (use temporary DTO instead of og class instance)
            List<HomeInformationDTO> HomeInfo = db.HomeInformations.Select(x => new HomeInformationDTO { HomeInformationID = x.HomeInformationID, Message = x.Message }).ToList();

            // Set the ViewBag equal to our new Home Information list that contains values from the database.
            ViewBag.HomeList = HomeInfo;

            // Now that our ViewBag is set. Go to the view.
            return View();
        }


        /*
            Name: Contact()
            Description: Sets ViewBag with all the current Contact Information values and so that they can be used to create the Contact View.
        */
        public ActionResult Contact()
        {

            // Declare Contact Information List.
            List<ContactInformation> list = db.ContactInformations.ToList();

            // Declare ViewBag using list.
            ViewBag.ContactList = new SelectList(list, "ContactInformationID", "Message");

            // Declare new Contact Information list and add values from database into the list. (use temporary DTO instead of og class instance)
            List<ContactInformationDTO> contactInfo = db.ContactInformations.Select(x => new ContactInformationDTO { ContactInformationID = x.ContactInformationID, Message = x.Message, Phone = x.Phone, Email = x.Email, ticketLink = x.ticketLink, ticketLinkMessage = x.ticketLinkMessage, ticketInstructions = x.ticketInstructions }).ToList();

            // Set the ViewBag equal to our new Contact Information list that contains values from the database.
            ViewBag.ContactList = contactInfo;

            // Now that our ViewBag is set. Go to the view.
            return View();

        }


        private class ContactInformationDTO : ContactInformation { }


        //[Authorize(UserTicketFormsController ="Admin")]
        public ActionResult Admin()
        {
            ViewBag.Message = "Your application admin page.";

            return View();
        }
    }
}