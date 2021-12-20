﻿using IronXL;
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

        public void readExcelFile(string fileName, string username)
        {
            // Create WorkBook
            WorkBook wb = WorkBook.Load(fileName);

            //// Read 'Supplier Info' Worksheet
            //getSupplierInfo(wb, username);

            //// Read 'Raw Material Tracking Info' Worksheet
            //getRawMaterialTrackingInfo(wb, username);

            // Testing
            getInventoryOverview(wb, username);
        }

        // 1.
        public void getSupplierInfo(WorkBook wb, string username)
        {
            WorkSheet ws = wb.GetWorkSheet("Supplier Info");

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
        }

        // 2.
        public void getRawMaterialTrackingInfo(WorkBook wb, string username)
        {
            WorkSheet ws = wb.GetWorkSheet("Raw Material Tracking Info");

            checkBrandCategoryType(ws, username);

            // Get Only Rows without headers
            foreach (var row in ws.Rows.ToList().GetRange(4, ws.RowCount - 4))
            {
                var tmp_arr = row.ToArray();

                // Product name
                string product_name = (string)tmp_arr[3].Value;

                if (product_name.Equals("")) continue;

                // Product Category
                string product_category = (string)tmp_arr[1].Value;
                int product_category_id = dbStatusFunction.productCategoryID(product_category);

                // Product Brand
                string product_brand = (string)tmp_arr[2].Value;
                int product_brand_id = dbStatusFunction.productBrandID(product_brand);

                string product_description = product_name;

                string product_sku2 = (string)tmp_arr[4].Value;

                // Product Status
                string product_status = (string)tmp_arr[8].Value;
                int product_status_id = dbStatusFunction.productStatusID(product_status);

                string product_code = (string)tmp_arr[11].Value;
                string product_sku = product_code;

                string product_type = (string)tmp_arr[12].Value;
                int product_type_id = dbStatusFunction.productTypeID(product_type);

                int product_model_id = dbStatusFunction.productModelID("Normal");
                int product_variety_id = dbStatusFunction.productVarietyID("Material");

                // Buy Price => tmp_arr[14].Value
                double buy_price = tmp_arr[14].DoubleValue;
                Decimal product_buy_price = Decimal.Parse(buy_price.ToString());

                // Insert New Product (Material)
                dbStoredProcedure.productInsert(
                    product_code, product_name,
                    product_description, product_sku, product_sku2,
                    product_buy_price, 0,
                    product_brand_id, product_model_id, product_category_id,
                    product_type_id, product_variety_id, product_status_id, username);

                // Create Supplier Shipment
                DateTime? ss_r_dt = tmp_arr[5].DateTimeValue;

                string ss_tra_id = (string)tmp_arr[6].Value;
                string ss_ntl_tra_id = (string)tmp_arr[7].Value;

                // Supplier
                string ss_supplier = (string)tmp_arr[9].Value;
                int supplier_id = dbStatusFunction.supplierID(ss_supplier);

                string roll_size = (string)tmp_arr[13].Value;
                Decimal[] hwl_arr = generalFunc.parseRollSize(roll_size);
                Decimal height = hwl_arr[0];
                Decimal width = hwl_arr[1];
                Decimal length = hwl_arr[2];

                int product_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeProduct') AS INT)").FirstOrDefault();

                // Insert Supplier Shipment
                dbStoredProcedure.supplierShipmentInsert(
                    ss_r_dt, ss_tra_id, ss_ntl_tra_id,
                    height, width, length,
                    supplier_id, product_id, username);
            }
            db.SaveChanges();
        }

        // Get All Brands
        public void checkBrandCategoryType(WorkSheet ws, string username)
        {
            // Get List of Brand name from Entity Framework
            IEnumerable<string> brandList = db.TShopeeProductBrands.Select(it => it.name.ToLower());

            // Get List of Category name from Entity Framework
            IEnumerable<string> categoryList = db.TShopeeProductCategories.Select(it => it.name.ToLower());

            // Get List of Type name from Entity Framework
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
        }

        // 4.
        public void getInventoryOverview(WorkBook wb, string username)
        {
            WorkSheet ws = wb.GetWorkSheet("Inventory Overview");

            // Get List of model name from Entity Framework
            IEnumerable<string> modelList = db.TShopeeProductModels.Select(x => x.name.ToLower());

            // Get List of Variety Name from Entity Framework
            IEnumerable<string> varietyList = db.TShopeeProductVarieties.Select(x => x.name.ToLower());

            // Get Rows Without Headers
            string model_name = "", panel_name = "";
            foreach (var row in ws.Rows.ToList().GetRange(3, ws.RowCount - 3))
            {
                var tmp_arr = row.ToArray();

                model_name = (tmp_arr[1].Text == "") ? model_name : tmp_arr[1].Text;
                panel_name = (tmp_arr[2].Text == "") ? panel_name : tmp_arr[2].Text;

                if (!model_name.Equals("") && !modelList.Contains(model_name.ToLower()))
                    dbStoredProcedure.productModelInsert(model_name, username);

                if (!panel_name.Equals("") && !varietyList.Contains(panel_name.ToLower()))
                    dbStoredProcedure.productVarietyInsert(panel_name, username);
            }
            db.SaveChanges();
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

                readExcelFile(file_path, username);
                return Content($"File {file.FileName} Uploaded Successfully!");

                //return Content(readExcelFile(file_path, username));
            }

            return Content($"Error! File {file.FileName} was not uploaded successfully...");
        }
    }
}