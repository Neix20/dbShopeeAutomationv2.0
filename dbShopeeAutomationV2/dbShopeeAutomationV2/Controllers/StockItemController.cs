using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class StockItemController : AdminController
    {
        public static string removeFirstAndLast(string str)
        {
            return str.Substring(1, str.Length - 2);
        }

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
        public ActionResult StockItemGridViewPartialAddNew(FormCollection collection)
        {
            string name = removeFirstAndLast(collection["name"]);
            string description = removeFirstAndLast(collection["description"]);
            int quantity = int.Parse(removeFirstAndLast(collection["stock_quantity"]));
            string product_sku = removeFirstAndLast(collection["Product SKU"]);
            string warehouse_title = removeFirstAndLast(collection["Stock Warehouse Location"]);
            string username = User.Identity.Name;

            dbStoredProcedure.stockItemInsert(name, description, quantity, product_sku, warehouse_title, username);
            db.SaveChanges();

            var model = db.TShopeeStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialUpdate(FormCollection collection)
        {
            int stock_item_id = int.Parse(collection["stock_item_id"]);
            string name = removeFirstAndLast(collection["name"]);
            string description = removeFirstAndLast(collection["description"]);
            int quantity = int.Parse(removeFirstAndLast(collection["stock_quantity"]));
            string product_sku = removeFirstAndLast(collection["Product SKU"]);
            string warehouse_title = removeFirstAndLast(collection["Stock Warehouse Location"]);
            string username = User.Identity.Name;

            dbStoredProcedure.stockItemUpdate(stock_item_id, name, description, quantity, product_sku, warehouse_title, username);
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

        [HttpPost]
        public void StockItemAdd(FormCollection collection)
        {
            // To Fix
            string name = collection["stock_item_name"];
            string description = collection["stock_item_description"];
            int product_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeProduct') AS INT)").FirstOrDefault();


            int warehouse_id = int.Parse(collection["stock_item_warehouse_id"]);

            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;

            db.NSP_TShopeeDetail_Insert("Normal", "-", username, currentTime, username, currentTime);
            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

            //db.NSP_TShopeeStockItem_Insert(stock_item_name, stock_item_description, 0, product_id, warehouse_id, detail_id);
            //db.SaveChanges();
        }
    }
}