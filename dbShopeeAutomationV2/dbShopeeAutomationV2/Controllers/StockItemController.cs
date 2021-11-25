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

            int product_id = db.TShopeeProducts.FirstOrDefault(it => it.SKU.Equals(product_sku)).product_id;
            int warehouse_id = db.TShopeeStockWarehouses.FirstOrDefault(it => it.name.Equals(warehouse_title)).stock_warehouse_id;

            dbStoredProcedure.stockItemInsert(name, description, quantity, product_id, warehouse_id, username);
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

            int product_id = db.TShopeeProducts.FirstOrDefault(it => it.SKU.Equals(product_sku)).product_id;
            int warehouse_id = db.TShopeeStockWarehouses.FirstOrDefault(it => it.name.Equals(warehouse_title)).stock_warehouse_id;

            dbStoredProcedure.stockItemUpdate(stock_item_id, name, description, quantity, product_id, warehouse_id, username);
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