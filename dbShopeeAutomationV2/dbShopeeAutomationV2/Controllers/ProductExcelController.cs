using IronXL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dbShopeeAutomationV2.Models;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductExcelController : Controller
    {
        // GET: ProductExcel
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        public string readExcelFile(string fileName, string username)
        {
            string str = "";

            // Create WorkBook
            WorkBook wb = WorkBook.Load(fileName);

            // Read 'Supplier Info' Worksheet
            str += getSupplierInfo(wb, username);
            str += getRawMaterialTrackingInfo(wb, username);

            return $"<div class='border border-success p-3 text-center'>{str}</div>";
        }

        // 1.
        public string getSupplierInfo(WorkBook wb, string username)
        {
            string str = "";

            WorkSheet ws = wb.GetWorkSheet("Supplier Info");

            // Header
            string header = "Supplier";
            str += $"<div class='row align-items-center'><div class='col fw-bold h4'>{header}</div></div>";

            // Get List of Supplier Name from Entity Framework
            IEnumerable<string> supplierList = db.TShopeeSuppliers.Select(it => it.name);

            // Debug
            foreach (var row in ws.Rows.ToList().GetRange(3, ws.RowCount - 3))
            {
                var tmp_arr = row.ToArray();

                string supplier_name = (string)tmp_arr[1].Value;

                if (supplier_name == "" || supplierList.Contains(supplier_name)) continue;

                string supplier_nation = (string)tmp_arr[2].Value;
                string supplier_code = (string)tmp_arr[3].Value;

                dbStoredProcedure.supplierInsert(supplier_name, supplier_code, supplier_nation, username);
            }
            db.SaveChanges();

            return str;
        }

        // 2.
        public int productStatusID(string name)
        {
            return -1;
        }

        public string getRawMaterialTrackingInfo(WorkBook wb, string username)
        {
            string str = "";

            WorkSheet ws = wb.GetWorkSheet("Raw Material Tracking Info");

            str += checkBrandCategoryType(ws, username);

            // Header
            string header = "Product (Material)";
            str += $"<div class='row align-items-center'><div class='col fw-bold h4'>{header}</div></div>";

            // Get Only Rows without headers
            foreach (var row in ws.Rows.ToList().GetRange(4, ws.RowCount - 4))
            {
                var tmp_arr = row.ToArray();

                // Create Product Model
                string product_name = (string)tmp_arr[3].Value;
                string product_sku2 = (string)tmp_arr[4].Value;


                // Create Supplier Shipment
            }
            db.SaveChanges();

            return str;
        }

        // Get All Brands
        public string checkBrandCategoryType(WorkSheet ws, string username)
        {
            string str = "";

            // Header
            string header = "Check Brand, Category and Type";
            str += $"<div class='row align-items-center'><div class='col fw-bold h4'>{header}</div></div>";

            // Get List of Brand name from Entity Framework
            IEnumerable<string> brandList = db.TShopeeProductBrands.Select(it => it.name.ToLower());

            // Get List of Category name from Entity Framework
            IEnumerable<string> categoryList = db.TShopeeProductCategories.Select(it => it.name.ToLower());

            // Get List of Type
            IEnumerable<string> typeList = db.TShopeeProductTypes.Select(it => it.name.ToLower());

            // Get Rows without Headers
            foreach(var row in ws.Rows.ToList().GetRange(4, ws.RowCount - 4))
            {
                // Check to see if brand exist in SQL Database
                var tmp_arr = row.ToArray();

                string category_name = (string)tmp_arr[1].Value;
                string brand_name = (string) tmp_arr[2].Value;
                string type_name = (string)tmp_arr[12].Value;

                if (!category_name.Equals("") && !categoryList.Contains(category_name.ToLower()))
                    dbStoredProcedure.productCategoryInsert(category_name, username);

                if (!brand_name.Equals("") && !brandList.Contains(brand_name.ToLower()))
                    dbStoredProcedure.productBrandInsert(brand_name, username);

                if (!type_name.Equals("") && !typeList.Contains(type_name.ToLower()))
                    dbStoredProcedure.productTypeInsert(type_name, username);
            }
            db.SaveChanges();

            return str;
        }

        [HttpPost]
        public ActionResult uploadFile()
        {
            string username = User.Identity.Name;

            var file = Request.Files["fileUpload"];

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/Uploads")}\\{file.FileName}";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);
                file.SaveAs(file_path);

                //readExcelFile(file_path, username);
                //return Content($"File {file.FileName} Uploaded Successfully!");

                string str = readExcelFile(file_path, username);
                return Content(str);
            }

            return Content($"Error! File {file.FileName} was not uploaded successfully...");
        }
    }
}