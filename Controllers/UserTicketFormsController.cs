using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using OfficeOpenXml;
using System.Web.Mvc;
using BasicMVC.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Data.SqlClient;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace BasicMVC.Controllers
{
    public class UserTicketFormsController : Controller
    {
        private class ContactInformationDTO : ContactInformation { }
        // Declare Database Entity.
        private BasicMVCEntities db = new BasicMVCEntities();


        // GET: UserTicketForms
        public ActionResult Index()
        {
            return View(db.UserTicketForms.ToList());
        }
        //public ActionResult Index()
        //{
        //    IQueryable<UserTicketForm> userSearch;
        //}


        /*
            Name: Search(string searchValue)
            Description: Uses value entered in search box to query UserTicketForm table. Returns result to view as a list. Search value can be name or userID.
        */
        public ActionResult Search(string searchValue)
        {

            // Declare IQueryable 
            IQueryable<UserTicketForm> userSearch = db.UserTicketForms;

            // Did the user provide a search value?
            if (!string.IsNullOrEmpty(searchValue))
            {
                // Yes, set IQueryable to name search query.
                userSearch = userSearch.Where(p => p.FirstName.Contains(searchValue) || p.LastName.Contains(searchValue));

                // Did the name search query return results?
                if (userSearch.ToList().Count == 0)
                {
                    // No, let's try an UserID search then.
                    //
                    // Clear name search query out of IQueryable.
                    userSearch = db.UserTicketForms;

                    // Set IQueryable to UserID search query.
                    userSearch = userSearch.Where(p => p.UserId.Equals(searchValue));
                }
            }

            // Return IQueryable back to the view as a list.
            return View("Index", userSearch.ToList());

        }


        // GET: UserTicketForms/Details/5

        // select list items
        //private IEnumerable<SelectListItem> GetRoles()
        //{
        //    var dbUserRoles = new DbUserRoles();
        //    var roles = dbUserRoles
        //            .GetRoles()
        //            .Select(x =>
        //                    new SelectListItem
        //                    {
        //                        Value = x.UserRoleId.ToString(),
        //                        Text = x.UserRole
        //                    });
        //    return new SelectList(roles, "Value", "Text");
        //}


        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTicketForm userTicketForm = db.UserTicketForms.Find(id);
            if (userTicketForm == null)
            {
                return HttpNotFound();
            }
            return View(userTicketForm);
        }


        // GET: UserTicketForms/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: UserTicketForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,Email,Address1,Address2,City,State,Zip,Country,MaxTickets,RequestTickets,RequestDate,Retiree,RequireTransportation")] UserTicketForm userTicketForm)
        {
            if (ModelState.IsValid)
            {
                db.UserTicketForms.Add(userTicketForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userTicketForm);
        }


        // GET: UserTicketForms/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTicketForm userTicketForm = db.UserTicketForms.Find(id);
            if (userTicketForm == null)
            {
                return HttpNotFound();
            }
            return View(userTicketForm);
        }


        // POST: UserTicketForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Email,Address1,Address2,City,State,Zip,Country,MaxTickets, RequestTickets, RequestDate, Retiree,RequireTransportation")] UserTicketForm userTicketForm)
        {
            if (ModelState.IsValid)
            {

                try
                {


                    db.Entry(userTicketForm).State = EntityState.Modified;
                    db.SaveChanges();
                    var senderEmail = new MailAddress("pgboston@aerofulfillment.com", "P&G Boston");
                    var receiverEmail = new MailAddress(userTicketForm.Email, "Receiver");
                    //var password = "Your Email Password here";
                    var sub = "2019 Gillette Family Picnic Ticket Confirmation";
                    var body = "<HTML> <HEAD> <TITLE> Registration is completed</Title></HEAD><BODY><P> Hello " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userTicketForm.FirstName.ToLower()) + ", <br/> <br/>" +
                        "Here is your information that you submitted: </P> <br/> <br/>" +
                        "<table border=\"1\"><tr><th>Full Name</th><th>Additional Ticket Requested</th><th>Max Tickets</th><th>Request Date</th></tr><tr><th>" + userTicketForm.FirstName + " " + userTicketForm.LastName + "</th><th>" + userTicketForm.RequestTickets.ToString() + "</th><th>" + userTicketForm.MaxTickets.ToString() + "</th><th>" + userTicketForm.RequestDate.ToString() + "</th></tr></table>"
                        + "</BODY></HTML>";
                    var smtp = new SmtpClient
                    {
                        Host = "aero-office.aerofulfillment.com",
                        Port = 25,
                        EnableSsl = false
                        // DeliveryMethod = SmtpDeliveryMethod.Network,
                        // UseDefaultCredentials = false,
                        // Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body,
                        IsBodyHtml = true
                    })
                    {
                        smtp.Send(mess);
                        return RedirectToAction("Confirmation");
                    }
                }
                catch (DbEntityValidationException e)
                {
                    ViewBag.ErrorMessage = e.Message;
                }


            }
            return View(userTicketForm);
        }


        // GET: UserTicketForms/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTicketForm userTicketForm = db.UserTicketForms.Find(id);
            if (userTicketForm == null)
            {
                return HttpNotFound();
            }
            return View(userTicketForm);
        }


        // POST: UserTicketForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserTicketForm userTicketForm = db.UserTicketForms.Find(id);
            db.UserTicketForms.Remove(userTicketForm);
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
        //public class IList <PGBOSTON.Models.UserTicketForm> GetExportList(){
        //    var exportList = db.UserTicketForms.ToList<UserTicketForm>();
        //    return exportList;
        //    }


        /*
            Name: ExportToExcel()
            Description: Exports values from User Ticket Form to Excel spreadsheet.
        */
        //public ActionResult ExportToExcel()
        //{

        //    // Declare Gridview .
        //    var gv = new GridView();

        //    // Set Gridview Datasource to User Ticket Form List.
        //    gv.DataSource = db.UserTicketForms.ToList();

        //    // Bind Datasource to Gridview.
        //    gv.DataBind();

        //    // Convert Gridview Checkboxes to "Yes" or "No" values to be displayed on spreadsheet.
        //    ConvertCheckboxControls(gv);

        //    // Configure Response.
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=UserForm.xls");
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Charset = "";

        //    // Declare StringWriter.
        //    StringWriter stringWriter = new StringWriter();

        //    // Declare htmlTextWriter.
        //    HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

        //    // Render Gridview using htmlTextWriter.
        //    gv.RenderControl(htmlTextWriter);

        //    // Write stringWriter to the Output of the Response.
        //    Response.Output.Write(stringWriter.ToString());

        //    // Flush Response.
        //    Response.Flush();

        //    // End Response.
        //    Response.End();

        //    // Nothing to return.
        //    return null;

        //}
        private class UserTicketFormDTO : UserTicketForm { }


        /*
             Name: ExportToExcel()
             Description: Exports values from User Ticket Form to Excel spreadsheet.
         */
        public ActionResult ExportToExcel()
        {

            // Get Ticket Form Values from Database and place them into a list of the ticket form class.
            List<UserTicketFormDTO> ticketFormList = db.UserTicketForms.Select(x => new UserTicketFormDTO
            {
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                State = x.State,
                Zip = x.Zip,
                Country = x.Country,
                Tickets = x.Tickets,
                MaxTickets = x.MaxTickets,
                RequestTickets = x.RequestTickets,
                RequestDate = x.RequestDate,
                Retiree = x.Retiree,
                RequireTransportation = x.RequireTransportation,
                Comments = x.Comments,
                Status = x.Status,
                FollowUp = x.FollowUp
            }
            ).ToList();

            // Declare Excel Package.
            ExcelPackage pck = new ExcelPackage();

            // Declare Excel Worksheet.
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("TicketUsers");

            // Assign headers to first row of cells.
            ws.Cells["A1"].Value = "UserID";
            ws.Cells["B1"].Value = "FirstName";
            ws.Cells["C1"].Value = "LastName";
            ws.Cells["D1"].Value = "Email";
            ws.Cells["E1"].Value = "Address1";
            ws.Cells["F1"].Value = "Address2";
            ws.Cells["G1"].Value = "City";
            ws.Cells["H1"].Value = "State";
            ws.Cells["I1"].Value = "Zip";
            ws.Cells["J1"].Value = "Country";
            ws.Cells["K1"].Value = "Tickets";
            ws.Cells["L1"].Value = "MaxTickets";
            ws.Cells["M1"].Value = "RequestTickets";
            ws.Cells["N1"].Value = "RequestDate";
            ws.Cells["O1"].Value = "Retiree";
            ws.Cells["P1"].Value = "RequireTransportation";
            ws.Cells["Q1"].Value = "Comments";
            ws.Cells["R1"].Value = "Status";
            ws.Cells["S1"].Value = "FollowUp";

            // Set what row data will start to be placed in.
            int rowstart = 2;

            // For each record in the ticket form list...
            foreach (var item in ticketFormList)
            {

                // Add values to cells.
                ws.Cells[string.Format("A{0}", rowstart)].Value = item.UserId;
                ws.Cells[string.Format("B{0}", rowstart)].Value = item.FirstName;
                ws.Cells[string.Format("C{0}", rowstart)].Value = item.LastName;
                ws.Cells[string.Format("D{0}", rowstart)].Value = item.Email;
                ws.Cells[string.Format("E{0}", rowstart)].Value = item.Address1;
                ws.Cells[string.Format("F{0}", rowstart)].Value = item.Address2;
                ws.Cells[string.Format("G{0}", rowstart)].Value = item.City;
                ws.Cells[string.Format("H{0}", rowstart)].Value = item.State;
                ws.Cells[string.Format("I{0}", rowstart)].Value = item.Zip;
                ws.Cells[string.Format("J{0}", rowstart)].Value = item.Country;
                ws.Cells[string.Format("K{0}", rowstart)].Value = item.Tickets;
                ws.Cells[string.Format("L{0}", rowstart)].Value = item.MaxTickets;
                ws.Cells[string.Format("M{0}", rowstart)].Value = item.RequestTickets;
                ws.Cells[string.Format("N{0}", rowstart)].Value = item.RequestDate;
                ws.Cells[string.Format("O{0}", rowstart)].Value = item.Retiree;
                ws.Cells[string.Format("P{0}", rowstart)].Value = item.RequireTransportation;
                ws.Cells[string.Format("Q{0}", rowstart)].Value = item.Comments;
                ws.Cells[string.Format("R{0}", rowstart)].Value = item.Status;
                ws.Cells[string.Format("S{0}", rowstart)].Value = item.FollowUp;

                // Set Request Date to date format.
                ws.Cells[string.Format("N{0}", rowstart)].Style.Numberformat.Format = "dd-MM-yyyy HH:mm";

                // Go to next row.
                rowstart++;
            }

            // Auto fit columns.
            ws.Cells["A:AZ"].AutoFitColumns();

            // Clear Response.
            Response.Clear();

            // Set Response Content Type.
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Add Response Header. (including workbook name)
            Response.AddHeader("content-disposition", "attachment: filename=" + "TicketUsers.xlsx");

            // Write to Response.
            Response.BinaryWrite(pck.GetAsByteArray());

            // End Reponse.
            Response.End();

            // All done. Nothing to return.
            return null;

        }
        /*
            Name: ConvertCheckboxControls
            Description: Converts checkboxes in the given Gridview to "Yes" or "No" values.
        */
        //private void ConvertCheckboxControls(GridView gv)
        //{

        //    // For each row in the Gridview
        //    for (int i = 0; i < gv.Rows.Count; i++)
        //    {

        //        // Declare Gridview row equal to the current row.
        //        GridViewRow row = gv.Rows[i];

        //        // For each cell in the current row.
        //        foreach (TableCell cell in row.Cells)
        //        {

        //            // Does the current cell have controls?
        //            if (cell.HasControls() == true)
        //            {

        //                //  Yes, is the control a checkbox?
        //                if (cell.Controls[0].GetType().ToString() == "System.Web.UI.WebControls.CheckBox")
        //                {

        //                    // Yes, declare checkbox equal to checkbox in current cell.
        //                    CheckBox chk = (CheckBox)cell.Controls[0];

        //                    // Is the checkbox checked?
        //                    if (chk.Checked)
        //                    {

        //                        // Yes, set cell text equal to "Yes".
        //                        cell.Text = "Yes";

        //                    }
        //                    else
        //                    {

        //                        // No, set cell text euql to "No".
        //                        cell.Text = "No";

        //                    }

        //                }

        //            }

        //        }

        //    }

        //}

        //private void ExportToExcel(string strFileName, GridView gv)
        //{
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.Charset = "";
        //    //byte[] fileStream = data.ToList<UserTicketForm>();
        //    //string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    StringWriter stringWriter = new StringWriter();
        //    HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
        //    gv.RenderControl(htmlTextWriter);
        //    Response.Output.Write(stringWriter.ToString());
        //    Response.Flush();
        //    Response.End();
        //    //return File(fileStream, mimeType);
        //}


        //protected void ExportButton(object sender, EventArgs e)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection objcon = new
        //    SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ToString());
        //    SqlDataAdapter objdata = new SqlDataAdapter("select * from UserTicketForm", objcon);
        //    objdata.Fill(dt);
        //    gvreport.DataSource = dt;
        //    gvreport.DataBind();
        //    ExportToExcel("UserForm.xls", gvreport);
        //    gvreport = null;
        //    gvreport.Dispose();
        //}
        //    var stringify = db.UserTicketForms.Select(x => x.Retiree.ToString()).ToList();
        //    var gv = new GridView();
        //    gv.DataSource = stringify.ToList();
        //    gv.DataBind();
        //}


    }


}