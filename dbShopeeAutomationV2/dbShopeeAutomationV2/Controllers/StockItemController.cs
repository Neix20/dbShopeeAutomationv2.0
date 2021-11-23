using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class StockItemController : Controller
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
        public ActionResult StockItemGridViewPartialAddNew(TShopeeStockItem item)
        {
            var model = db.TShopeeStockItems;
            return PartialView("_StockItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockItemGridViewPartialUpdate(FormCollection collection)
        {
            int stock_item_id = int.Parse(collection["stock_item_id"]);
            string stock_item_name = removeFirstAndLast(collection["name"]);
            string stock_item_description = removeFirstAndLast(collection["description"]);
            int stock_item_quantity = int.Parse(removeFirstAndLast(collection["stock_quantity"]));

            string stock_item_sku = removeFirstAndLast(collection["Product SKU"]);
            var product_model = db.TShopeeProducts.FirstOrDefault(it => it.SKU.Equals(stock_item_sku));
            int product_id = product_model.product_id;

            string warehouse_title = removeFirstAndLast(collection["Stock Warehouse Location"]);
            var warehouse_model = db.TShopeeStockWarehouses.FirstOrDefault(it => it.name.ToLower().Equals(warehouse_title.ToLower()));
            int warehouse_id = warehouse_model.stock_warehouse_id;

            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;
            int detail_id = (int)db.TShopeeStockWarehouses.FirstOrDefault(it => it.stock_warehouse_id == warehouse_id).detail_id;

            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            db.NSP_TShopeeDetail_Update(detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username, currentTime);
            db.SaveChanges();

            db.NSP_TShopeeStockItem_Update(stock_item_id, stock_item_name, stock_item_description, stock_item_quantity, product_id, warehouse_id, detail_id);
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