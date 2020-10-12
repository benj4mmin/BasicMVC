using DevExpress.Web;
using DevExpress.Web.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;

namespace BasicMVC.Controllers
{
    public const string UploadDirectory = "~/Content/UploadedFiles/";
    public const string DocumentsDirectory = "~/Content/UploadedFiles/";
    public const string TempDirectory = "~/Content/UploadedFiles/";
    public const string ThumbnailFormat = "Thumbnail{0}{1}";


    public static void ucDragAndDrop_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
    {
        if (e.UploadedFile.IsValid)
        {
            string fileName = Path.ChangeExtension(Path.GetRandomFileName(), ".jpg");
            string resultFilePath = UploadDirectory + fileName;
            using (Image original = Image.FromStream(e.UploadedFile.FileContent))
            using (Image thumbnail = new ImageThumbnailCreator(original).CreateImageThumbnail(new Size(350, 350)))
                ImageUtils.SaveToJpeg(thumbnail, HttpContext.Current.Request.MapPath(resultFilePath));
            UploadingUtils.RemoveFileWithDelay(fileName, HttpContext.Current.Request.MapPath(resultFilePath), 5);
            IUrlResolutionService urlResolver = sender as IUrlResolutionService;
            if (urlResolver != null)
                e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
        }
    }

    public static void ucMultiSelection_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
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
        UploadControlSettings.UploadExcel(e.UploadedFile);
    }

    static string GetCallbackData(UploadedFile uploadedFile, string fileUrl)
    {
        string name = uploadedFile.FileName;
        long sizeInKilobytes = uploadedFile.ContentLength / 1024;
        string sizeText = sizeInKilobytes.ToString() + " KB";

        return name + "|" + fileUrl + "|" + sizeText;
    }
    public static DevExpress.Web.UploadControlValidationSettings UploadHelperValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
    {
        AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".xlsx", ".xls" },
        MaxFileSize = 4000000
    };

}
}