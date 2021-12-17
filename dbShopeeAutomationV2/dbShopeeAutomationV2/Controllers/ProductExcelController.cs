using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductExcelController : Controller
    {
        // GET: ProductExcel
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult uploadFile()
        {
            var file = Request.Files["fileUpload"];

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/Uploads")}\\{file.FileName}";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);
                file.SaveAs(file_path);

                //readExcelFile(file_path, platform_id, username);

                return Content($"File {file.FileName} Uploaded Successfully!");
            }

            return Content($"Error! File {file.FileName} was not uploaded successfully...");
        }
    }
}