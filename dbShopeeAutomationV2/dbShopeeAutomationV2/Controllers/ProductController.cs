using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductController : AdminController
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductGridViewPartial()
        {
            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialAddNew(TShopeeProduct item)
        {
            string username = User.Identity.Name;

            item.product_code = (item.product_code == null || item.product_code.Equals("")) ? generalFunc.Random10DigitCode() : item.product_code;
            item.product_code = item.product_code.Replace(" ", String.Empty);

            item.name = (item.name == null) ? "product_name" : item.name;
            item.description = (item.description == null) ? "product_description" : item.description;

            item.SKU = (item.SKU == null) ? item.name : item.SKU;
            item.SKU2 = (item.SKU2 == null) ? item.SKU : item.SKU2;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;

            // Set Default for Product Variety ID
            item.product_variety_id = dbStoredProcedure.getID("TShopeeProductVariety");

            dbStoredProcedure.productInsert(
                item.product_code, item.name,
                item.description, item.SKU, item.SKU2,
                item.buy_price, item.sell_price,
                item.product_brand_id, item.product_model_id, item.product_category_id,
                item.product_type_id, item.product_variety_id, item.product_status_id, username);

            var file = Request.Files["product_image"];

            int product_id = dbStoredProcedure.getID("TShopeeProduct");

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/ProductImages")}\\Product_{product_id}.png";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/ProductImages"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

                var b = (Bitmap)Bitmap.FromStream(file.InputStream);
                b.Save(file_path, ImageFormat.Png);
            }

            // Create New Stock Item
            int stock_warehouse_id = dbStatusFunction.stockWarehouseID("VendLah! (HQ)");

            dbStoredProcedure.stockItemInsert(item.name, item.description, 0, product_id, stock_warehouse_id, username);

            int stock_item_id = dbStoredProcedure.getID("TShopeeStockItem");

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/StockItemImages")}\\{stock_item_id}_{product_id}.png";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/StockItemImages"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

                var b = (Bitmap)Bitmap.FromStream(file.InputStream);
                b.Save(file_path, ImageFormat.Png);
            }

            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialUpdate(TShopeeProduct item)
        {
            string username = User.Identity.Name;

            item.product_code = (item.product_code == null || item.product_code.Equals("")) ? generalFunc.Random10DigitCode() : item.product_code;
            item.product_code = item.product_code.Replace(" ", String.Empty);

            item.name = (item.name == null) ? "product_name" : item.name;
            item.description = (item.description == null) ? "" : item.description;

            item.SKU = (item.SKU == null) ? item.name : item.SKU;
            item.SKU2 = (item.SKU2 == null) ? item.SKU : item.SKU2;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;

            // Set Default for Product Variety ID
            item.product_variety_id = dbStoredProcedure.getID("TShopeeProductVariety");

            dbStoredProcedure.productUpdate(
                item.product_id, item.product_code, item.name,
                item.description, item.SKU, item.SKU2,
                item.buy_price, item.sell_price,
                item.product_brand_id, item.product_model_id, item.product_category_id,
                item.product_type_id, item.product_variety_id, item.product_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialDelete(int product_id)
        {
            dbStoredProcedure.productDelete(product_id);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost]
        public ActionResult UpdateSKU(int product_id)
        {
            // Check to see if Product is Material or Not
            int pro_mat_id = dbStatusFunction.productModelID("material");

            var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id);

            if(product.product_model_id == pro_mat_id)
            {
                // List of Materials
                var materialList = db.TShopeeProducts.Where(it => it.product_model_id == pro_mat_id).ToList();

                //// Check to see if Supplier Shipment Exists or not
                //// Technically, it is supposed to exist. If it doesn't, it just means the admin forgot to insert it.
                var supplierShipment = db.TShopeeSupplierShipments.FirstOrDefault(it => it.product_id == product.product_id);
                if (supplierShipment != null)
                {
                    var product_category = db.TShopeeProductCategories.FirstOrDefault(it => it.product_category_id == product.product_category_id);
                    string product_category_code = product_category.code;

                    var supplier = db.TShopeeSuppliers.FirstOrDefault(it => it.supplier_id == supplierShipment.supplier_id);
                    string supplier_code = supplier.code;

                    int material_count = materialList.Count;

                    product.SKU = generalFunc.GenMaterialSKU(product_category_code, supplier_code, material_count);
                }
            } else
            {
                // Check to see if Product Component Exist
                // Technically, it is supposed to exist. If it doesn't, it just means the admin forgot to insert it.
                var productComponent = db.TShopeeProductComponents.FirstOrDefault(it => it.master_product_id == product.product_id);
                if (productComponent != null)
                {
                    var product_model = db.TShopeeProductModels.FirstOrDefault(it => it.product_model_id == product.product_model_id);
                    string product_model_code = product_model.code;

                    var product_category = db.TShopeeProductCategories.FirstOrDefault(it => it.product_category_id == product.product_category_id);
                    string product_category_code = product_category.code;

                    var product_material = db.TShopeeProducts.FirstOrDefault(it => it.product_id == productComponent.sub_product_id);
                    string product_material_sku = product_material.SKU;

                    product.SKU = generalFunc.GenProductSKU(product_model_code, product_category_code, product_material_sku);
                }
            }
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost]
        public ActionResult UpdateProductSKU()
        {
            // Check to see if Product is Material or Not
            int pro_mat_id = dbStatusFunction.productModelID("material");

            // List of Master Product ID
            List<int> pcList = db.TShopeeProductComponents.Select(it => (int) it.master_product_id).ToList();

            var productList = db.TShopeeProducts.Where(it => it.product_model_id != pro_mat_id && pcList.Contains(it.product_id)).ToList();

            productList.ForEach(item =>
            {
                var product_model = db.TShopeeProductModels.FirstOrDefault(it => it.product_model_id == item.product_model_id);
                string product_model_code = product_model.code;

                var product_category = db.TShopeeProductCategories.FirstOrDefault(it => it.product_category_id == item.product_category_id);
                string product_category_code = product_category.code;

                // Check to see if Product Component Exist
                // Technically, it is supposed to exist. If it doesn't, it just means the admin forgot to insert it.
                var productComponent = db.TShopeeProductComponents.FirstOrDefault(it => it.master_product_id == item.product_id);
                var product_material = db.TShopeeProducts.FirstOrDefault(it => it.product_id == productComponent.sub_product_id);
                string product_material_sku = product_material.SKU;

                item.SKU = generalFunc.GenProductSKU(product_model_code, product_category_code, product_material_sku);
            });
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        // [Depreceated]
        [HttpPost]
        public ActionResult UpdateMaterialSKU()
        {
            // Check to see if Product is Material or Not
            int pro_mat_id = dbStatusFunction.productModelID("material");

            var model = db.TShopeeProducts;

            int material_ind = 0;

            model.ToList().ForEach(item =>
            {
                if (item.product_model_id == pro_mat_id)
                {
                    // Check to see if Supplier Shipment Exists or not
                    // Technically, it is supposed to exist. If it doesn't, it just means the admin forgot to insert it.
                    var supplierShipment = db.TShopeeSupplierShipments.FirstOrDefault(it => it.product_id == item.product_id);
                    if (supplierShipment != null)
                    {
                        string product_category_code = db.TShopeeProductCategories.FirstOrDefault(it => it.product_category_id == item.product_category_id).code;
                        string supplier_code = db.TShopeeSuppliers.FirstOrDefault(it => it.supplier_id == supplierShipment.supplier_id).code;
                        int material_count = ++material_ind;

                        item.SKU = generalFunc.GenMaterialSKU(product_category_code, supplier_code, material_count);
                    }
                }
            });
            db.SaveChanges();

            return PartialView("_ProductGridViewPartial", model.ToList());
        }


        public ActionResult ProductComponentMasterDetailDetailPartial(int product_id)
        {
            ViewData["product_id"] = product_id;
            var model = db.TShopeeProductComponents.Where(it=> it.master_product_id == product_id);

            return PartialView("_ProductGridViewPartialDetails", model.ToList());
        }

        [ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialSpecial(int product_id)
        {
            ViewData["product_id"] = product_id;
            var model = db.TShopeeProductComponents.Where(it=>it.master_product_id==product_id);
            return PartialView("_ProductGridViewPartialDetails", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialAddNew(TShopeeProductComponent item, int product_id)
        {
            ViewData["product_id"] = product_id;
            string username = User.Identity.Name;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;

            dbStoredProcedure.productComponentInsert(product_id, item.sub_product_id, item.quantity, item.type_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductComponents.Where(it => it.master_product_id == product_id);
            return PartialView("_ProductGridViewPartialDetails", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialUpdate(TShopeeProductComponent item, int product_id)
        {
            ViewData["product_id"] = product_id;
            string username = User.Identity.Name;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;

            dbStoredProcedure.productComponentUpdate(item.product_component_id, product_id, item.sub_product_id, item.quantity, item.type_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductComponents.Where(it => it.master_product_id == product_id);
            return PartialView("_ProductGridViewPartialDetails", model.ToList());
        }
    }
}