using DevExpress.Web.Mvc;
using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class StockWarehouseController : AdminController
    {
        // GET: StockWarehouse
        public ActionResult Index()
        {
            return View();
        }

        public static string removeFirstAndLast(string str)
        {
            return str.Substring(1, str.Length - 2);
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartial()
        {
            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialAddNew(FormCollection collection)
        {
            string name = removeFirstAndLast(collection["name"]);
            string email_address = removeFirstAndLast(collection["email_address"]);
            string phone_number = removeFirstAndLast(collection["phone_number"]);
            string address = removeFirstAndLast(collection["Address"]);
            string username = User.Identity.Name;

            dbStoredProcedure.stockWarehouseInsert(name, email_address, phone_number, address, username);
            db.SaveChanges();

            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialUpdate(FormCollection collection)
        {
            int stock_warehouse_id = int.Parse(collection["stock_warehouse_id"]);
            string name = removeFirstAndLast(collection["name"]);
            string email_address = removeFirstAndLast(collection["email_address"]);
            string phone_number = removeFirstAndLast(collection["phone_number"]);
            string address = removeFirstAndLast(collection["Address"]);
            string username = User.Identity.Name;

            dbStoredProcedure.stockWarehouseUpdate(stock_warehouse_id, name, email_address, phone_number, address, username);
            db.SaveChanges();

            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialDelete(int stock_warehouse_id)
        {
            dbStoredProcedure.stockWarehouseDelete(stock_warehouse_id);
            db.SaveChanges();

            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }
    }
}