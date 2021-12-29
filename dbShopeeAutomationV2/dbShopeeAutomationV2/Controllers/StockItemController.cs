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
    public class StockItemController : AdminController
    {
        // GET: StockItem
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult StockItemGridViewPartial()
        {
            var model = db.TShopeeStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialAddNew(TShopeeStockItem item)
        {
            string username = User.Identity.Name;

            var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == item.product_id);
            string product_name = product.name;

            item.name = (item.name == null) ? product_name : item.name;
            item.description = (item.description == null) ? "Stock Item Description" : item.description;
            item.stock_quantity = (item.stock_quantity == null) ? 0 : item.stock_quantity;

            dbStoredProcedure.stockItemInsert(item.name, item.description, item.stock_quantity, item.product_id, item.stock_warehouse_id, username);
            db.SaveChanges();

            var file = Request.Files["stock_item_image"];

            int stock_item_id = dbStoredProcedure.getID("TShopeeStockItem");

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/StockItemImages")}\\{stock_item_id}_{item.product_id}.png";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/StockItemImages"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

                var b = (Bitmap)Bitmap.FromStream(file.InputStream);
                b.Save(file_path, ImageFormat.Png);
            }

            var model = db.TShopeeStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialUpdate(TShopeeStockItem item)
        {
            string username = User.Identity.Name;

            var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == item.product_id);
            string product_name = product.name;

            item.name = (item.name == null) ? product_name : item.name;
            item.description = (item.description == null) ? "Stock Item Description" : item.description;
            item.stock_quantity = (item.stock_quantity == null) ? 0 : item.stock_quantity;

            dbStoredProcedure.stockItemUpdate(item.stock_item_id, item.name, item.description, item.stock_quantity, item.product_id, item.stock_warehouse_id, username);
            db.SaveChanges();

            var model = db.TShopeeStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialDelete(int stock_item_id)
        {
            db.NSP_TShopeeStockItem_Delete(stock_item_id);
            db.SaveChanges();

            var model = db.TShopeeStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }
    }
}