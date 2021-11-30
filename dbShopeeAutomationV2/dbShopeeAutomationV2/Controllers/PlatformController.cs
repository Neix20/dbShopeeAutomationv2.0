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

        //public void readExcelFile(string filename, int platform_id, string username)
        //{
        //    WorkBook wb = WorkBook.Load(filename);
        //    WorkSheet ws = wb.GetWorkSheet("Shopee Report");

        //    // Record Rows
        //    // Order Id => col[0]
        //    // Order Status => col[1]
        //    // Payment Method => col[2]
        //    // Buyer Name => col[5]
        //    // Buyer Mobile Phone => col[6]
        //    // Product Name => col[7]
        //    // Product Quantity => col[8]
        //    // Product Unit Price => col[9]
        //    // Order Sub Total => col[10]
        //    // Shipping Fee => col[11]
        //    // Extra Charges => col[12]
        //    // Order Income => col[13]
        //    // Carrier Name => col[14]
        //    // Tracking Id => col[15]
        //    // Shipping Address => col[16]
        //    // Product Variety => col[18]
        //    // Purchase Date Time => col[19]

        //    ws.Rows.ToList().GetRange(1, ws.RowCount - 1).ForEach(row => {
        //        var tmp_arr = row.ToArray();

        //        // Create Customer / Buyer
        //        string customer_name = (string) tmp_arr[5].Value;
        //        string customer_phone_numer = (string)tmp_arr[6].Value;
        //        dbStoredProcedure.customerInsert(customer_name, DateTime.Now, "", customer_phone_numer, "", platform_id, username);

        //        // Create Carrier
        //        string carrier_name = (string)tmp_arr[14].Value;
        //        dbStoredProcedure.carrierInsert(carrier_name, username);

        //        // Create Product Variety
        //        string product_variety = (string)tmp_arr[18].Value;
        //        dbStoredProcedure.productVarietyInsert(product_variety, username);
        //        int product_variety_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeProductVariety') AS INT)").FirstOrDefault();

        //        // Create Product
        //        int product_brand_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeProductBrand') AS INT)").FirstOrDefault();
        //        int product_type_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeProductType') AS INT)").FirstOrDefault();
        //        string product_name = (string)tmp_arr[7].Value;

        //        // Create Order

        //        // Create Order Item

        //        // Create Invoice

        //        // Create Shipment
        //    });
        //}

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

            // Prevent Duplicate
            var platformNameList = db.TShopeePlatforms.Select(it => it.name.ToLower()).ToList();
            string platform = item.name;

            if (!platformNameList.Contains(platform.ToLower()))
            {
                dbStoredProcedure.platformInsert(platform, username);
                db.SaveChanges();
            }

            int platform_id = db.TShopeePlatforms.FirstOrDefault(it => it.name.ToLower().Equals(platform.ToLower())).platform_id;

            // File Upload / Read Excel
            var file = Request.Files["fileUpload"];

            if(file != null && file.ContentLength > 0)
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

            dbStoredProcedure.platformUpdate(item.platform_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialDelete(int platform_id)
        {
            dbStoredProcedure.platformDelete(platform_id);
            db.SaveChanges();

            // Delete All Customer
            var customer_list = db.TShopeeCustomers.Where(it => it.platform_id == platform_id).ToList();
            customer_list.ForEach(cusModel => {
                int customer_id = cusModel.customer_id;
                dbStoredProcedure.customerDelete(customer_id);
                db.SaveChanges();
            });

            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }
    }
}