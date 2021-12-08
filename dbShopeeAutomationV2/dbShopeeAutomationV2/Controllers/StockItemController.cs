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

            item.name = (item.name == null) ? "Stock Item Name" : item.name;
            item.description = (item.description == null) ? "Stock Item Description" : item.description;
            item.stock_quantity = (item.stock_quantity == null) ? 0 : item.stock_quantity;

            dbStoredProcedure.stockItemInsert(item.name, item.description, (int)item.stock_quantity, (int)item.product_id, (int)item.stock_warehouse_id, username);
            db.SaveChanges();

            var model = db.TShopeeStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialUpdate(TShopeeStockItem item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "Stock Item Name" : item.name;
            item.description = (item.description == null) ? "Stock Item Description" : item.description;
            item.stock_quantity = (item.stock_quantity == null) ? 0 : item.stock_quantity;

            dbStoredProcedure.stockItemUpdate(item.stock_item_id, item.name, item.description, (int)item.stock_quantity, (int)item.product_id, (int)item.stock_warehouse_id, username);
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