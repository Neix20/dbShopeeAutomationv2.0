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
            item.name = (item.name == null) ? "product_name" : item.name;
            item.description = (item.description == null) ? "product_description" : item.description;

            item.SKU = (item.SKU == null) ? generalFunc.Random10DigitCode() : item.SKU;
            item.SKU2 = (item.SKU2 == null) ? $"{item.SKU}_2" : item.SKU2;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;

            dbStoredProcedure.productInsert(
                item.product_code, item.name,
                item.description, item.SKU, item.SKU2,
                item.buy_price, item.sell_price,
                item.product_brand_id, item.product_model_id, item.product_category_id,
                item.product_type_id, item.product_variety_id, item.product_status_id, username);

            var file = Request.Files["product_image"];

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/ProductImages")}\\{item.SKU}.png";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/ProductImages"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

                var b = (Bitmap)Bitmap.FromStream(file.InputStream);
                b.Save(file_path, ImageFormat.Png);
            }

            // Create New Stock Item
            int stock_warehouse_id = dbStatusFunction.stockWarehouseID("VendLah! (HQ)");
            int product_id = dbStoredProcedure.getID("TShopeeProduct");

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
            item.name = (item.name == null) ? "product_name" : item.name;
            item.description = (item.description == null) ? "product_description" : item.description;

            item.SKU = (item.SKU == null) ? generalFunc.Random10DigitCode() : item.SKU;
            item.SKU2 = (item.SKU2 == null) ? $"{item.SKU}_2" : item.SKU2;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;

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
    }
}