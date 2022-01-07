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
    public class ProductExcelController : AdminController
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

            // Read 'Supplier Info' Worksheet
            getSupplierInfo(wb, username);

            // Read 'Raw Material Tracking Info' Worksheet
            getRawMaterialTrackingInfo(wb, username);

            // Read 'Inventory Overview' Worksheet 
            getInventoryOverview(wb, username);

            // Read 'Record' Worksheet
            getRecord(wb, username);
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

                string supplier_nation = (string) tmp_arr[2].Value;
                string supplier_code = (string) tmp_arr[3].Value;

                dbStoredProcedure.supplierInsert(
                    supplier_name, supplier_code, supplier_nation, 
                    "", "", "", 
                    username);
            }
            db.SaveChanges();
        }

        // 2.
        public void getRawMaterialTrackingInfo(WorkBook wb, string username)
        {
            WorkSheet ws = wb.GetWorkSheet("Raw Material Tracking Info");

            checkBrandCategoryTypeVariety(ws, username);

            // Get List of Product SKU from Entity Framework
            IEnumerable<string> productSKUList = db.TShopeeProducts.Select(x => x.SKU);

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

                string product_sku = (string)tmp_arr[4].Value;
                product_sku = generalFunc.removeWhitespace(product_sku);

                if (productSKUList.Contains(product_sku)) continue;

                // Product Status
                string product_status = (string)tmp_arr[8].Value;
                int product_status_id = dbStatusFunction.productStatusID(product_status);

                string product_code = (string) tmp_arr[11].Value;
                product_code = generalFunc.removeWhitespace(product_code);

                string product_sku2 = product_code;

                string product_type = (string)tmp_arr[12].Value;
                int product_type_id = dbStatusFunction.productTypeID(product_type);

                int product_model_id = dbStatusFunction.productModelID("Material");

                string product_variety = product_category;
                int product_variety_id = dbStatusFunction.productVarietyID(product_variety);

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

                string roll_size = (string)tmp_arr[13].Value;
                Decimal[] hwl_arr = generalFunc.parseRollSize(roll_size);
                Decimal height = hwl_arr[0];
                Decimal width = hwl_arr[1];
                Decimal length = hwl_arr[2];

                // Insert New Stock Item
                int stock_warehouse_id = dbStatusFunction.stockWarehouseID("VendLah! (HQ)");
                int product_id = dbStoredProcedure.getID("TShopeeProduct");

                dbStoredProcedure.stockItemInsert(product_name, product_description, width * length, product_id, stock_warehouse_id, username);

                // Create Supplier Shipment
                DateTime? ss_r_dt = tmp_arr[5].DateTimeValue;

                string ss_tra_id = (string)tmp_arr[6].Value;
                string ss_ntl_tra_id = (string)tmp_arr[7].Value;

                // Supplier
                string ss_supplier = (string)tmp_arr[9].Value;
                int supplier_id = dbStatusFunction.supplierID(ss_supplier);

                // Insert Supplier Shipment
                dbStoredProcedure.supplierShipmentInsert(
                    ss_r_dt, ss_tra_id, ss_ntl_tra_id,
                    height, width, length,
                    supplier_id, product_id, username);
            }
            db.SaveChanges();
        }

        // Get All Brands
        public void checkBrandCategoryTypeVariety(WorkSheet ws, string username)
        {
            // Get List of Brand name from Entity Framework
            IEnumerable<string> brandList = db.TShopeeProductBrands.Select(it => it.name.ToLower());

            // Get List of Category name from Entity Framework
            IEnumerable<string> categoryList = db.TShopeeProductCategories.Select(it => it.name.ToLower());

            // Get List of Type name from Entity Framework
            IEnumerable<string> typeList = db.TShopeeProductTypes.Select(it => it.name.ToLower());

            // Get List of Variety name From Entity Framework
            IEnumerable<string> varietyList = db.TShopeeProductVarieties.Select(it => it.name.ToLower());

            // Get Rows without Headers
            foreach(var row in ws.Rows.ToList().GetRange(4, ws.RowCount - 4))
            {
                // Check to see if brand exist in SQL Database
                var tmp_arr = row.ToArray();

                string category_name = (string)tmp_arr[1].Value;
                string variety_name = category_name;
                string brand_name = (string) tmp_arr[2].Value;
                string type_name = (string)tmp_arr[12].Value;

                if (!category_name.Equals("") && !categoryList.Contains(category_name.ToLower()))
                    dbStoredProcedure.productCategoryInsert(category_name, $"{category_name.Substring(0, 1)}", username);

                if (!brand_name.Equals("") && !brandList.Contains(brand_name.ToLower()))
                    dbStoredProcedure.productBrandInsert(brand_name, brand_name, username);

                if (!type_name.Equals("") && !typeList.Contains(type_name.ToLower()))
                    dbStoredProcedure.productTypeInsert(type_name, type_name, username);

                if(!variety_name.Equals("") && !varietyList.Contains(variety_name.ToLower()))
                    dbStoredProcedure.productVarietyInsert(variety_name, $"{variety_name.Substring(0, 1)}", username);
            }
            db.SaveChanges();
        }

        // 3.
        public void getRecord(WorkBook wb, string username)
        {
            WorkSheet ws = wb.GetWorkSheet("Record");

            // Insert New Production Detail
            int last_production_id = dbStoredProcedure.getID("TShopeeProduction");
            decimal total_usage;

            // tmp_arr[1] => Production Date Created
            // tmp_arr[2] => Production Staff Name
            // tmp_arr[6] => Total Usage
            // tmp_arr[15] => Production Description

            // tmp_arr[4] => Material

            // tmp_arr[8] => Product SKU
            // tmp_arr[10] => Product L x W
            // tmp_arr[11] => Product Total Quantity
            // tmp_arr[12] => Product NG
            // tmp_arr[13] => Product OK

            // Get Rows Without Headers
            foreach (var row in ws.Rows.ToList().GetRange(4, ws.RowCount - 4))
            {
                var tmp_arr = row.ToArray();

                // Only Update Values when reached a new Production
                if (tmp_arr[1].Text != "")
                {
                    string title = generalFunc.GenProductionCode(last_production_id);

                    last_production_id += 1;

                    DateTime? created_date = tmp_arr[1].DateTimeValue;

                    string staff_name = tmp_arr[2].Text;

                    string total_usage_str = (tmp_arr[6].Text == "") ? "0" : tmp_arr[6].Text;
                    total_usage = Decimal.Parse(total_usage_str);

                    string description = tmp_arr[15].Text;

                    int production_status_id = dbStatusFunction.productionStatusID("Complete");

                    dbStoredProcedure.productionInsert(title, description, staff_name, created_date, total_usage, production_status_id, username);

                    // Product Material
                    string material_code = tmp_arr[4].Text;
                    material_code = generalFunc.removeWhitespace(material_code);

                    int material_id = dbStatusFunction.productIdByCode(material_code);

                    dbStoredProcedure.productionDetailInsert("", DateTime.Now, DateTime.Now, 0, 0, 0, total_usage, 0, 0, last_production_id, material_id, username);
                }

                if (tmp_arr[8].Text == "") continue;

                string product_sku = tmp_arr[8].Text;
                int product_id = dbStatusFunction.productIdBySKU(product_sku);

                string size = (tmp_arr[10].Text == "") ? "0 x 0" : tmp_arr[10].Text;
                string[] size_arr = size.Split(new[] { " x " }, StringSplitOptions.None);
                decimal length = Decimal.Parse(size_arr[0]);
                decimal width = Decimal.Parse(size_arr[1]);

                string total_quantity_str = (tmp_arr[11].Text == "") ? "0" : tmp_arr[11].Text;
                decimal total_quantity = decimal.Parse(total_quantity_str);

                string not_ok_str = (tmp_arr[12].Text == "") ? "0" : tmp_arr[12].Text;
                int not_ok = int.Parse(not_ok_str);

                string ok_str = (tmp_arr[13].Text == "") ? "0" : tmp_arr[13].Text;
                int ok = int.Parse(ok_str);

                dbStoredProcedure.productionDetailInsert(
                    "", DateTime.Now, DateTime.Now,
                    0, width, length,
                    total_quantity, not_ok, ok,
                    last_production_id, product_id, username);

                // Update Stock Item
                var stockItem = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == product_id);
                stockItem.stock_quantity += ok;
            }
            db.SaveChanges();
        }

        public void checkProduction(WorkSheet ws, string username)
        {
            int last_production_id = dbStoredProcedure.getID("TShopeeProduction");

            // Get Rows Without Headers
            foreach (var row in ws.Rows.ToList().GetRange(3, ws.RowCount - 3))
            {
                var tmp_arr = row.ToArray();

                if (tmp_arr[8].Text == "") continue;

                if (tmp_arr[1].Text != "")
                {
                    string title = generalFunc.GenProductionCode(last_production_id);
                    last_production_id += 1;

                    DateTime? created_date = tmp_arr[1].DateTimeValue;

                    string staff_name = tmp_arr[2].Text;

                    string total_usage_str = (tmp_arr[6].Text == "") ? "0" : tmp_arr[6].Text;
                    decimal total_usage = Decimal.Parse(total_usage_str);

                    string description = tmp_arr[15].Text;

                    int production_status_id = dbStatusFunction.productionStatusID("Complete");

                    dbStoredProcedure.productionInsert(title, description, staff_name, created_date, total_usage, production_status_id, username);
                }
            }
            db.SaveChanges();
        }

        // 4.
        public void getInventoryOverview(WorkBook wb, string username)
        {
            WorkSheet ws = wb.GetWorkSheet("Inventory Overview");

            // Check if Model and Variety Exist
            checkModelVariety(ws, username);

            // Get List of Product SKU from Entity Framework
            IEnumerable<string> productSKUList = db.TShopeeProducts.Select(x => x.SKU);

            // Insert New Product (Product)
            string model_name = "", panel_name = "", product_sku = "";
            foreach (var row in ws.Rows.ToList().GetRange(3, ws.RowCount - 3))
            {
                var tmp_arr = row.ToArray();

                model_name = (tmp_arr[1].Text == "") ? model_name : tmp_arr[1].Text;
                panel_name = (tmp_arr[2].Text == "") ? panel_name : tmp_arr[2].Text;
                product_sku = tmp_arr[3].Text;
                product_sku = generalFunc.removeWhitespace(product_sku);

                if (product_sku == "" || productSKUList.Contains(product_sku)) continue;

                string[] info_arr = product_sku.Split('-');

                string model_code = info_arr[0];
                string variety_code = info_arr[1];
                string material_sku = info_arr[2];

                int product_brand_id = dbStatusFunction.productBrandID("NTL Asia");
                int product_status_id = dbStatusFunction.productStatusID("Empty");

                int product_variety_id = dbStatusFunction.productVarietyCodeID(variety_code);
                int product_category_id = product_variety_id;

                int product_model_id = dbStatusFunction.productModelCodeID(model_code);

                int sc_type_id = (int)dbStatusFunction.productTypeID("Solid Color");
                int product_type_id = sc_type_id;

                string product_code = product_sku;
                string product_sku2 = product_sku;

                string product_name = $"{model_name} : {panel_name}";
                string product_description = product_name;

                dbStoredProcedure.productInsert(
                    product_code, product_name,
                    product_description, product_sku, product_sku2,
                    0, 0,
                    product_brand_id, product_model_id, product_category_id,
                    product_type_id, product_variety_id, product_status_id, username
                );

                // Insert New Stock Item
                int stock_warehouse_id = dbStatusFunction.stockWarehouseID("VendLah! (HQ)");
                int product_id = dbStoredProcedure.getID("TShopeeProduct");

                dbStoredProcedure.stockItemInsert(product_name, product_description, 0, product_id, stock_warehouse_id, username);

                // Get Material
                material_sku = generalFunc.removeWhitespace(material_sku);
                var material = db.TShopeeProducts.FirstOrDefault(it => it.SKU == material_sku);

                // Insert Product Component
                int master_product_id = product_id;
                int sub_product_id = material.product_id;

                dbStoredProcedure.productComponentInsert(master_product_id, sub_product_id, 1,1, username);
            }
            db.SaveChanges();
        }

        public void checkModelVariety(WorkSheet ws, string username)
        {
            // Get List of model name from Entity Framework
            IEnumerable<string> modelList = db.TShopeeProductModels.Select(x => x.name.ToLower());

            // Get List of Variety Name from Entity Framework
            IEnumerable<string> varietyList = db.TShopeeProductVarieties.Select(x => x.name.ToLower());

            // Get List of Category Name From Entity Framework
            IEnumerable<string> categoryList = db.TShopeeProductCategories.Select(x => x.name.ToLower());

            // Get Rows Without Headers
            string model_name = "", model_code = "",
                panel_name = "", panel_code = "",
                product_sku = "";
            foreach (var row in ws.Rows.ToList().GetRange(3, ws.RowCount - 3))
            {
                var tmp_arr = row.ToArray();

                model_name = (tmp_arr[1].Text == "") ? model_name : tmp_arr[1].Text;
                panel_name = (tmp_arr[2].Text == "") ? panel_name : tmp_arr[2].Text;
                product_sku = tmp_arr[3].Text;

                if (product_sku == "") continue;

                string[] info_arr = product_sku.Split('-');
                model_code = info_arr[0];
                panel_code = info_arr[1];
                
                if (!modelList.Contains(model_name.ToLower()))
                    dbStoredProcedure.productModelInsert(model_name, model_code, username);

                if (!varietyList.Contains(panel_name.ToLower()))
                    dbStoredProcedure.productVarietyInsert(panel_name, panel_code, username);

                if (!categoryList.Contains(panel_name.ToLower()))
                    dbStoredProcedure.productCategoryInsert(panel_name, panel_code, username);
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
            }

            return Content($"Error! File {file.FileName} was not uploaded successfully...");
        }
    }
}