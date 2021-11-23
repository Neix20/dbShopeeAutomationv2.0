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
        public ActionResult StockWarehouseGridViewPartialAddNew(TShopeeStockWarehouse item)
        {
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

            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            // Error Might be cause here
            // Hypotheses: Remove First and Last for Stock Ware house ID
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;
            int detail_id = (int) db.TShopeeStockWarehouses.FirstOrDefault(it => it.stock_warehouse_id == stock_warehouse_id).detail_id;

            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            db.NSP_TShopeeDetail_Update(detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username, currentTime);
            db.SaveChanges();

            db.NSP_TShopeeStockWarehouse_Update(stock_warehouse_id, name, email_address, phone_number, address_line_1, address_line_2, city, state, zip_code, country, detail_id);
            db.SaveChanges();

            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialDelete(int stock_warehouse_id)
        {
            db.NSP_TShopeeStockWarehouse_Delete(stock_warehouse_id);
            db.SaveChanges();

            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }
    }
}