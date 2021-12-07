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
            name = (name == "") ? "stock_warehouse_name" : name;

            string email_address = generalFunc.trimStr(collection["email_address"]);
            email_address = (email_address == "") ? generalFunc.GenEmail() : email_address;

            string phone_number = generalFunc.trimStr(collection["phone_number"]);
            phone_number = (phone_number == "") ? generalFunc.GenPhoneNum() : phone_number;

            string address = generalFunc.trimStr(collection["Address"]);
            string username = User.Identity.Name;

            int count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            dbStoredProcedure.stockWarehouseInsert(name, email_address, phone_number, address_line_1, address_line_2, city, zip_code, state, country, username);
            db.SaveChanges();

            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialUpdate(FormCollection collection)
        {
            int stock_warehouse_id = int.Parse(collection["stock_warehouse_id"]);

            string name = generalFunc.trimStr(collection["name"]);
            name = (name == "") ? "stock_warehouse_name" : name;

            string email_address = generalFunc.trimStr(collection["email_address"]);
            email_address = (email_address == "") ? generalFunc.GenEmail() : email_address;

            string phone_number = generalFunc.trimStr(collection["phone_number"]);
            phone_number = (phone_number == "") ? generalFunc.GenPhoneNum() : phone_number;

            string address = generalFunc.trimStr(collection["Address"]);
            string username = User.Identity.Name;

            int count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            dbStoredProcedure.stockWarehouseUpdate(stock_warehouse_id, name, email_address, phone_number, address_line_1, address_line_2, city, zip_code, state, country, username);
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