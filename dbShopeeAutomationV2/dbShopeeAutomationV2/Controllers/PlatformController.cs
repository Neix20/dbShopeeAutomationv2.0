using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using IronXL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class PlatformController : AdminController
    {
        // GET: Platform
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        public void readExcelFile(string filename)
        {
            WorkBook wb = WorkBook.Load(filename);
            WorkSheet ws = wb.GetWorkSheet("Shopee Report");

            // Header Row
            ws.Rows[0].ToList().ForEach(col => {

            });

            // Record Rows
            ws.Rows.ToList().GetRange(1, ws.RowCount - 1).ForEach(row => {
                row.ToList().ForEach(col => {

                });
            });
        }

        [ValidateInput(false)]
        public ActionResult PlatformGridViewPartial()
        {
            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialAddNew(TShopeePlatform item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;

            db.NSP_TShopeeDetail_Insert($"Platform {item.name}", "-", username, currentTime, username, currentTime);
            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

            db.NSP_TShopeePlatform_Insert(item.name, detail_id);
            db.SaveChanges();

            int platform_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeePlatform') AS INT)").FirstOrDefault();

            // File Upload / Read Excel
            var file = Request.Files["fileUpload"];

            if(file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/Uploads")}\\{file.FileName}";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);
                file.SaveAs(file_path);


            }

            return RedirectToAction("Index", "Platform");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialUpdate(TShopeePlatform item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;
            int detail_id = (int) db.TShopeePlatforms.FirstOrDefault(it => it.platform_id == item.platform_id).detail_id;

            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            db.NSP_TShopeeDetail_Update(detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username, currentTime);
            db.SaveChanges();

            db.NSP_TShopeePlatform_Update(item.platform_id, item.name, detail_id);
            db.SaveChanges();

            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialDelete(int platform_id)
        {
            db.NSP_TShopeePlatform_Delete(platform_id);
            db.SaveChanges();

            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }
    }
}