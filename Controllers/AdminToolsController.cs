using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasicMVC.Models;
using DevExpress.Web;
using ClosedXML.Excel;
using DevExpress.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Web.Mvc.BinderSettings;
using DevExpress.DataAccess.Excel;
using System.Data.Entity;
using System.ComponentModel;
using System.Collections;

namespace BasicMVC.Controllers
{
    // Declare Database Entity.
    private PGBOSTONEntities db = new PGBOSTONEntities();

    // Declare DTO Classes
    private class ContactInformationDTO : ContactInformation { }
    private class HomeInformationDTO : HomeInformation { }

    [HttpGet]
    // GET: AdminTools
    public ActionResult Index()
    {
        //if (Session["DataTableModel"] == null)
        //    Session["DataTableModel"] = InMemoryModel.OpenExcelFile("");

        return View();
    }


    /*
        Name: ContactInformation() 
        Description: Sets ViewBag with all the current Contact Information values in the database.
    */
    public ActionResult ContactInformation()
    {

        // Declare Contact Information List.
        List<ContactInformation> list = db.ContactInformations.ToList();

        // Declare ViewBag using list.
        ViewBag.ContactList = new SelectList(list, "ContactInformationID", "Message");

        // Declare new Contact Information list and add values from database into the list. (use temporary DTO instead of og class instance)
        List<ContactInformationDTO> contactInfo = db.ContactInformations.Select(x => new ContactInformationDTO { ContactInformationID = x.ContactInformationID, Message = x.Message, Phone = x.Phone, Email = x.Email, ticketLink = x.ticketLink, ticketLinkMessage = x.ticketLinkMessage, ticketInstructions = x.ticketInstructions }).ToList();

        // Set the ViewBag equal to our new Contact Information list that contains values from the database.
        ViewBag.ContactList = contactInfo;

        // Now that our ViewBag is set. Go back to view.
        return View();

    }


    /*
        Name: ContactInformation(ContactInformation model) 
        Description: Uses the Contact Information model values set in the view by the user to update the Contact Information table in the database.
    */
    [HttpPost]
    public ActionResult ContactInformation(ContactInformation model)
    {

        try
        {

            // Declare Contact Information List.
            List<ContactInformation> list = db.ContactInformations.ToList();

            // Declare ViewBag using list.
            ViewBag.ContactList = new SelectList(list, "ContactInformationID", "Message");

            // Declare Database Model 
            ContactInformation contact = db.ContactInformations.SingleOrDefault(x => x.ContactInformationID == model.ContactInformationID);

            // Set Contact Database Model values equal to the values in the model passed in by the user form.
            contact.ContactInformationID = model.ContactInformationID;
            contact.Message = model.Message;
            contact.Phone = model.Phone;
            contact.Email = model.Email;
            contact.ticketLink = model.ticketLink;
            contact.ticketLinkMessage = model.ticketLinkMessage;
            contact.ticketInstructions = model.ticketInstructions;

            // Save changes to the table in the database.
            db.SaveChanges();

            // Declare new Contact Information list and add updated values from database into the list. (use temporary DTO instead of og class instance) 
            List<ContactInformationDTO> contactInfo = db.ContactInformations.Select(x => new ContactInformationDTO { ContactInformationID = x.ContactInformationID, Message = x.Message, Phone = x.Phone, Email = x.Email, ticketLink = x.ticketLink, ticketLinkMessage = x.ticketLinkMessage, ticketInstructions = x.ticketInstructions }).ToList();

            // Set the ViewBag equal to our new Contact Information list that contains values from the database.
            ViewBag.ContactList = contactInfo.ToList();

            // Now that the database has been updated and our ViewBag is set with the udpated values. Go back to view.
            return View();

        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


    /*
        Name: ddEditContact(int ContactInformationID)
        Description: Uses ContactInformationID to add Contact Information values from the database to a class model and then returns the model to a view to be edited.
    */
    public ActionResult AddEditContact(int ContactInformationID)
    {

        // Declare Contact Information List.
        List<ContactInformation> list = db.ContactInformations.ToList();

        // Declare ViewBag using list.
        ViewBag.ContactList = new SelectList(list, "ContactInformationID", "Message");

        // Declare class model
        ContactInformation model = new ContactInformation();

        // Do we have a ContactInformationID?
        if (ContactInformationID > 0)
        {

            // Yes, declare Database Model
            ContactInformation contact = db.ContactInformations.SingleOrDefault(x => x.ContactInformationID == ContactInformationID);

            // Set class model values equal to values from Database model.
            model.ContactInformationID = contact.ContactInformationID;
            model.Message = contact.Message;
            model.Phone = contact.Phone;
            model.Email = contact.Email;
            model.ticketLink = contact.ticketLink;
            model.ticketLinkMessage = contact.ticketLinkMessage;
            model.ticketInstructions = contact.ticketInstructions;

        }

        // Go to view with our class model values that will be edited.
        return View("ContactInformationEdit", model);

    }


    /*
       Name: HomeInformation() 
       Description: Sets ViewBag with all the current Home Information values in the database.
   */
    public ActionResult HomeInformation()
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


    /*
        Name: HomeInformation(HomeInformation model) 
        Description: Uses the Home Information model values set in the view by the user to update the Home Information table in the database.
    */
    [HttpPost]
    public ActionResult HomeInformation(HomeInformation model)
    {

        try
        {

            // Declare Home Information List.
            List<HomeInformation> list = db.HomeInformations.ToList();

            // Declare ViewBag using list.
            ViewBag.HomeList = new SelectList(list, "HomeInformationID", "Message");

            // Declare Database Model 
            HomeInformation Home = db.HomeInformations.SingleOrDefault(x => x.HomeInformationID == model.HomeInformationID);

            // Set Home Database Model values equal to the values in the model passed in by the user form.
            Home.HomeInformationID = model.HomeInformationID;
            Home.Message = model.Message;

            // Save changes to the table in the database.
            db.SaveChanges();

            // Declare new Home Information list and add updated values from database into the list. (use temporary DTO instead of og class instance) 
            List<HomeInformationDTO> HomeInfo = db.HomeInformations.Select(x => new HomeInformationDTO { HomeInformationID = x.HomeInformationID, Message = x.Message }).ToList();

            // Set the ViewBag equal to our new Home Information list that contains values from the database.
            ViewBag.HomeList = HomeInfo.ToList();

            // Now that the database has been updated and our ViewBag is set with the udpated values. Go back to view.
            return View();

        }
        catch (Exception ex)
        {

            throw ex;
        }

    }


    /*
        Name: EditHome(int HomeInformationID)
        Description: Uses HomeInformationID to add Home Information values from the database to a class model and then returns the model to a view to be edited.
    */
    public ActionResult EditHome(int HomeInformationID)
    {

        // Declare Home Information List.
        List<HomeInformation> list = db.HomeInformations.ToList();

        // Declare ViewBag using list.
        ViewBag.HomeList = new SelectList(list, "HomeInformationID", "Message");

        // Declare class model
        HomeInformation model = new HomeInformation();

        // Do we have a HomeInformationID?
        if (HomeInformationID > 0)
        {

            // Yes, declare Database Model
            HomeInformation Home = db.HomeInformations.SingleOrDefault(x => x.HomeInformationID == HomeInformationID);

            // Set class model values equal to values from Database model.
            model.HomeInformationID = Home.HomeInformationID;
            model.Message = Home.Message;

        }

        // Go to view with our class model values that will be edited.
        return View("HomeInformationEdit", model);

    }
    [HttpPost]
    // GET: AdminTools/gridview
    public ActionResult GridViewPartial(string path)
    {
        var model = Session["DataTableModel"];
        if (!string.IsNullOrEmpty(path))
        {
            model = InMemoryModel.OpenExcelFile(path);
            Session["DataTableModel"] = model;
        }
        return PartialView(model);
    }

    //  AdminTools/upload control
    public ActionResult UploadControlUpload()
    {
        UploadControlExtension.GetUploadedFiles("UploadControlFile", UploadControlSettings.UploadValidationSettings, UploadControlSettings.FileUploadComplete);
        return null;
    }

    protected void uploadbutton_Click(object sender, EventArgs e)
    {
        var dirPath = System.Web.HttpContext.Current.Server.MapPath("~/Images/");


    }


    // update message displayed on site closed or open page
    public ActionResult SiteMsg()
    {
        //var textMessage = string"";
        ViewData["JavaScriptFunction"] = "SiteHeaderMsg();";
        // 
        return View("TextBox");
    }

    // switch site login on or off by changing which html view is named create (which is used to login)
    public ActionResult SiteOn(string on)
    {
        System.IO.File.Move("~/Views/UserLogins/SiteOff.chtml", "~/Views/UserLogins/Create.chtml");
        return null;
    }

    // off switch
    public ActionResult SiteOff(string off)
    {
        System.IO.File.Move("~/Views/UserLogins/Create.chtml", "~/Views/UserLogins/SiteOff.chtml");

        //
        return null;
    }

    //public void ToggleSite()
    //{
    //    try {
    //        if ()
    //        {

    //        }

    //    }
    //}
}

public class InMemoryModel
{

    public static object OpenExcelFile(string path)
    {
        if (path == string.Empty)
            path = "~/UploadedFiles/UserForm.xlsx";

        string fileName = path.StartsWith("~") ? System.Web.HttpContext.Current.Server.MapPath(path) : path;

        ExcelDataSource excelDataSource = new ExcelDataSource();
        excelDataSource.FileName = fileName;
        ExcelWorksheetSettings excelWorksheetSettings = new ExcelWorksheetSettings();
        excelWorksheetSettings.WorksheetName = "Sheet1";

        ExcelSourceOptions excelSourceOptions = new ExcelSourceOptions();
        excelSourceOptions.ImportSettings = excelWorksheetSettings;
        excelSourceOptions.SkipHiddenRows = false;
        excelSourceOptions.SkipHiddenColumns = false;
        excelSourceOptions.UseFirstRowAsHeader = true;
        excelDataSource.SourceOptions = excelSourceOptions;

        excelDataSource.Fill();

        DataTable table = excelDataSource.ToDataTable();
        return table;
    }
}

public static class ExcelDataSourceExtension
{
    public static DataTable ToDataTable(this ExcelDataSource excelDataSource)
    {
        IList list = ((IListSource)excelDataSource).GetList();
        DevExpress.DataAccess.Native.Excel.DataView dataView = (DevExpress.DataAccess.Native.Excel.DataView)list;
        List<PropertyDescriptor> properties = dataView.Columns.ToList<PropertyDescriptor>();

        DataTable table = new DataTable();
        for (int i = 0; i < properties.Count; i++)
        {
            PropertyDescriptor prop = properties[i];
            table.Columns.Add(prop.Name, prop.PropertyType);
        }
        object[] values = new object[properties.Count];
        foreach (DevExpress.DataAccess.Native.Excel.ViewRow item in list)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = properties[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }
}


public partial class UploadControlController : Controller
{

    public override string ToString() { return "UploadControl"; }
    // GET: ImageUploader
    public ActionResult Index()
    {
        return MultiFileSelection();
    }
    public ActionResult UploadedFilesContainer(bool useExtendedPopup)
    {
        ViewData["UseExtendedPopup"] = useExtendedPopup;
        return PartialView("UploadedFilesContainer");
    }
    public ActionResult MultiFileSelection()
    {
        UploadControlExtension.GetUploadedFiles("MultiFileSelection", UploadControlSettings.UploadValidationSettings, UploadControlSettings.FormUploadComplete);
        return null;
    }
    // logo upload
    public ActionResult UploadControlUpload()
    {
        UploadControlExtension.GetUploadedFiles("UploadControl", UploadControlSettings.UploadImgValidationSettings, UploadControlSettings.FileUploadComplete);
        return null;
    }
    // banner upload
    public ActionResult UploadBannerUpload()
    {
        UploadControlExtension.GetUploadedFiles("UploadBanner", UploadControlSettings.UploadImgValidationSettings, UploadControlSettings.BannerUploadComplete);
        return null;
    }
    // Contact upload
    public ActionResult UploadContactUpload()
    {
        UploadControlExtension.GetUploadedFiles("UploadContact", UploadControlSettings.UploadImgValidationSettings, UploadControlSettings.ContactUploadComplete);
        return null;
    }

}

public class UploadControlSettings
{
    private static PGBOSTONEntities db = new PGBOSTONEntities();
    public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
    {
        AllowedFileExtensions = new string[] { ".xlsx", ".xls" },
        MaxFileSize = 40000000
    };
    public static DevExpress.Web.UploadControlValidationSettings UploadImgValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
    {
        AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" },
        MaxFileSize = 4000000
    };
    //translated from navigator
    public static void UploadExcel(UploadedFile uploadedFile)
    {
        try
        {
            XLWorkbook Book;
            IXLWorksheet Sheets;
            Book = new XLWorkbook(System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedFiles/") + uploadedFile.FileName);
            Sheets = Book.Worksheet(1);
            IXLRange range = Sheets.RangeUsed();
            bool firstRow = true;
            DataTable dt = new DataTable();
            foreach (IXLRow row in Sheets.Rows())
            {
                // Use the first row to add columns to DataTable.
                if (firstRow)
                {
                    foreach (IXLCell cell in row.Cells())
                        dt.Columns.Add(cell.Value.ToString());
                    firstRow = false;
                }
                else
                {
                    UserTicketForm userTicketForm = new UserTicketForm();
                    userTicketForm.UserId = row.Cell(1).GetValue<String>();
                    userTicketForm.FirstName = row.Cell(2).GetValue<String>();
                    userTicketForm.LastName = row.Cell(3).GetValue<String>();
                    userTicketForm.Email = row.Cell(4).GetValue<String>();
                    //userTicketForm.Country = row.Cell(10).GetValue<String>();
                    // tickets is not used on import only when tickets start being requested
                    //if (row.Cell(5).GetValue<Int32>() == null)
                    //    {
                    //        userTicketForm.Tickets = Convert.ToInt32(row.Cell(5).GetValue<Int32>());
                    //    }
                    //    else
                    //    {
                    //        userTicketForm.Tickets = Convert.ToInt32(row.Cell(5).GetValue<Int32>() ?? 0);
                    //    }
                    //if (row.Cell(5).GetValue<Nullable<Int32>>() <= 10)
                    //{
                    //    int n = row.Cell(5).GetValue<Int32>();
                    //    do
                    //    {
                    //        userTicketForm.MaxTickets = n++;
                    //    } while (n <= 10);
                    //}
                    //else
                    //{
                    //    userTicketForm.MaxTickets = 0;
                    //}
                    //userTicketForm.MaxTickets = 0;
                    if (row.Cell(5).GetValue<String>() == "null")
                    {
                        userTicketForm.MaxTickets = 0;
                    }
                    else if (row.Cell(5).GetValue<String>() == "")
                    {
                        userTicketForm.MaxTickets = 0;
                    }
                    else
                    {
                        int n = row.Cell(5).GetValue<Int32>();
                        if (n <= 10)
                        {
                            userTicketForm.MaxTickets = row.Cell(5).GetValue<Int32>();
                        }
                        else
                        {
                            userTicketForm.MaxTickets = 0;
                        }
                    }
                    userTicketForm.RequestTickets = row.Cell(6).GetValue<Int32>();
                    // FIX - change so it checks if by date then row.Cell(7).GetValue<DateTime>(); else force to 1999
                    //if (row.Cell(7).GetValue<DateTime>() > )
                    //{
                    //    userTicketForm.RequestDate = DateTime.MinValue;
                    //}
                    //else
                    //{
                    //    userTicketForm.RequestDate = DateTime.MinValue;
                    //}
                    userTicketForm.RequestDate = row.Cell(7).GetValue<DateTime>();
                    // retiree gets set from customer as 0 or 1 
                    if (row.Cell(8).GetValue<Int32>() <= 1)
                    {
                        if (row.Cell(8).GetValue<Int32>() == 1)
                        {
                            userTicketForm.Retiree = true;
                        }
                        else
                        {
                            userTicketForm.Retiree = false;
                        }
                    }
                    else if (row.Cell(8).GetValue<String>() == "yes")
                    {
                        userTicketForm.Retiree = true;
                    }
                    else if (row.Cell(8).GetValue<String>() == "no")
                    {
                        userTicketForm.Retiree = false;
                    }
                    else if (row.Cell(8).GetValue<Boolean>() == true)
                    {
                        userTicketForm.Retiree = true;
                    }
                    else
                    {
                        userTicketForm.Retiree = false;
                    }
                    userTicketForm.Address1 = row.Cell(9).GetValue<String>();
                    userTicketForm.Address2 = row.Cell(10).GetValue<String>();
                    userTicketForm.City = row.Cell(11).GetValue<String>();
                    userTicketForm.State = row.Cell(12).GetValue<String>();
                    userTicketForm.Zip = row.Cell(13).GetValue<String>();
                    // customer sends req trans as 0 for false or 1 true otherwise we expect yes/no true/false
                    if (row.Cell(14).GetValue<Int32>() <= 1)
                    {
                        if (row.Cell(14).GetValue<Int32>() == 1)
                        {
                            userTicketForm.RequireTransportation = true;
                        }
                        else
                        {
                            userTicketForm.RequireTransportation = false;
                        }
                    }
                    else if (row.Cell(14).GetValue<String>() == "yes")
                    {
                        userTicketForm.RequireTransportation = true;
                    }
                    else if (row.Cell(14).GetValue<String>() == "no")
                    {
                        userTicketForm.RequireTransportation = false;
                    }
                    else if (row.Cell(14).GetValue<Boolean>() == true)
                    {
                        userTicketForm.RequireTransportation = true;
                    }
                    else
                    {
                        userTicketForm.RequireTransportation = false;
                    }
                    //userTicketForm.RequireTransportation = row.Cell(14).GetValue<Boolean>();
                    userTicketForm.Comments = row.Cell(15).GetValue<String>();
                    userTicketForm.Status = row.Cell(16).GetValue<String>();
                    userTicketForm.FollowUp = row.Cell(17).GetValue<String>();
                    db.UserTicketForms.Add(userTicketForm);
                    db.SaveChanges();
                }
            }
        }
        catch (Exception e) { string error = e.Message; }
    }
    public static void FormUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {
        const string UploadDirectory = "~/Content/UploadedFiles/";
        if (e.UploadedFile.IsValid)
        {
            MemoryStream ms = new MemoryStream();
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
            string resultFileUrl = UploadDirectory + resultFileName;
            string resultFilePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/UploadedFiles/") + e.UploadedFile.FileName;
            e.UploadedFile.SaveAs(resultFilePath);
            e.CallbackData = resultFilePath;
        }
        UploadExcel(e.UploadedFile);
    }
    // logo upload complete
    public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {
        const string UploadDirectory = "~/Content/Images/";
        if (e.UploadedFile.IsValid)
        {
            // Save uploaded file to some location
            MemoryStream ms = new MemoryStream();
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
            string resultFileUrl = UploadDirectory + resultFileName;
            string resultFilePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/") + "gillette_logo_4c.jpg";
            e.UploadedFile.SaveAs(resultFilePath);
            e.CallbackData = resultFilePath;
        }
    }
    // Banner upload complete
    public static void BannerUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {
        const string UploadDirectory = "~/Content/Images/";
        if (e.UploadedFile.IsValid)
        {
            // Save uploaded file to some location
            MemoryStream ms = new MemoryStream();
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
            string resultFileUrl = UploadDirectory + resultFileName;
            string resultFilePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/") + "pgbbanner.jpg";
            e.UploadedFile.SaveAs(resultFilePath);
            e.CallbackData = resultFilePath;
        }
    }
    // Contact upload complete
    public static void ContactUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
    {
        const string UploadDirectory = "~/Content/Images/";
        if (e.UploadedFile.IsValid)
        {
            // Save uploaded file to some location
            MemoryStream ms = new MemoryStream();
            string resultExtension = Path.GetExtension(e.UploadedFile.FileName);
            string resultFileName = Path.ChangeExtension(Path.GetRandomFileName(), resultExtension);
            string resultFileUrl = UploadDirectory + resultFileName;
            string resultFilePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/") + "bothdatesposter2.jpg";
            e.UploadedFile.SaveAs(resultFilePath);
            e.CallbackData = resultFilePath;
        }
    }
}
public class MultiFileSelectionBinder : DevExpressEditorsBinder
{
    public MultiFileSelectionBinder()
    {
        UploadControlBinderSettings.ValidationSettings.Assign(UploadControlSettings.UploadValidationSettings);
        UploadControlBinderSettings.FileUploadCompleteHandler = UploadControlSettings.FormUploadComplete;
    }
}


    //public class TextBoxSettings : SettingsBase
    //{
    //    public TextBoxExtension(TextBoxSettings settings)
    //}

    // DRAGONDROP SECTION
    //public class DragAndDropSupportBinder : DevExpressEditorsBinder
    //{
    //    // DragAndDropSupportBinder for img upload
    //    public DragAndDropSupportBinder()
    //    {
    //        UploadControlBinderSettings.ValidationSettings.Assign(UploadControlHelper.UploadValidationSettings);
    //        UploadControlBinderSettings.FileUploadCompleteHandler = UploadControlHelper.ucDragAndDrop_FileUploadComplete;
    //    }
    //}

    //public partial class UploadControlController : Controller
    //{
    //    const string UploadDirectory = "~/Content/UploadedFiles/";
    //    public ActionResult DragAndDrop()
    //    {
    //        return View("DragAndDrop");
    //    }
    //    public ActionResult DragAndDropImageUpload([ModelBinder(typeof(DragAndDropSupportBinder))]IEnumerable<UploadedFile> ucDragAndDrop)
    //    {
    //        return null;
    //    }
    //}
}