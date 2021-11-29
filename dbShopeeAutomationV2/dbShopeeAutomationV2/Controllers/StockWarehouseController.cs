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
            string name = generalFunc.trimStr(collection["name"]);
            string email_address = generalFunc.trimStr(collection["email_address"]);
            string phone_number = generalFunc.trimStr(collection["phone_number"]);
            string address = generalFunc.trimStr(collection["Address"]);
            string username = User.Identity.Name;

            int count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

            dbStoredProcedure.stockWarehouseInsert(name, email_address, phone_number, address, username);
            db.SaveChanges();

            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialUpdate(FormCollection collection)
        {
            int stock_warehouse_id = int.Parse(collection["stock_warehouse_id"]);
            string name = generalFunc.trimStr(collection["name"]);
            string email_address = generalFunc.trimStr(collection["email_address"]);
            string phone_number = generalFunc.trimStr(collection["phone_number"]);
            string address = generalFunc.trimStr(collection["Address"]);
            string username = User.Identity.Name;

            int count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

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