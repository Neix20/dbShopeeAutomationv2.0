using DevExpress.Web.Mvc;
using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

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
        public ActionResult StockWarehouseGridViewPartialAddNew(TShopeeStockWarehouse item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "stock_warehouse_name" : item.name;
            item.email_address = (item.email_address == null) ? generalFunc.GenEmail() : item.email_address;
            item.phone_number = (item.phone_number == null) ? generalFunc.GenPhoneNum() : item.phone_number;

            if (item.address_line_1 == null)
            {
                string address = Request.Form["Address"];
                string[] address_arr = generalFunc.FormatAddress(address);
                item.address_line_1 = address_arr[0];
                item.address_line_2 = address_arr[1];
                item.city = address_arr[2];
                item.zip_code = int.Parse(address_arr[3]);
                item.state = address_arr[4];
                item.country = address_arr[5];
            }
            item.address_line_1 = item.address_line_1.Replace(",", String.Empty);

            item.address_line_2 = (item.address_line_2 == null) ? "address_line_2" : item.address_line_2;
            item.address_line_2 = item.address_line_2.Replace(",", String.Empty);

            item.city = (item.city == null) ? "city" : item.city;
            item.zip_code = (item.zip_code == null) ? 0 : item.zip_code;
            item.state = (item.state == null) ? "state" : item.state;
            item.country = (item.country == null) ? "country" : item.country;

            dbStoredProcedure.stockWarehouseInsert(
                item.name, item.email_address, item.phone_number,
                item.address_line_1, item.address_line_2, item.city,
                item.zip_code, item.state, item.country, username);
            db.SaveChanges();

            var file = Request.Files["warehouse_image"];

            int stock_warehouse_id = dbStoredProcedure.getID("TShopeeStockWarehouse");

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/StockWarehouseImages")}\\{stock_warehouse_id}_{item.name}.png";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/StockWarehouseImages"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

                var b = (Bitmap)Bitmap.FromStream(file.InputStream);
                b.Save(file_path, ImageFormat.Png);
            }

            var model = db.TShopeeStockWarehouses;
            return PartialView("_StockWarehouseGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StockWarehouseGridViewPartialUpdate(TShopeeStockWarehouse item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "stock_warehouse_name" : item.name;
            item.email_address = (item.email_address == null) ? generalFunc.GenEmail() : item.email_address;
            item.phone_number = (item.phone_number == null) ? generalFunc.GenPhoneNum() : item.phone_number;

            if (item.address_line_1 == null)
            {
                string address = Request.Form["Address"];
                string[] address_arr = generalFunc.FormatAddress(address);
                item.address_line_1 = address_arr[0];
                item.address_line_2 = address_arr[1];
                item.city = address_arr[2];
                item.zip_code = int.Parse(address_arr[3]);
                item.state = address_arr[4];
                item.country = address_arr[5];
            }

            item.address_line_2 = (item.address_line_2 == null) ? "address_line_2" : item.address_line_2;
            item.city = (item.city == null) ? "city" : item.city;
            item.zip_code = (item.zip_code == null) ? 0 : item.zip_code;
            item.state = (item.state == null) ? "state" : item.state;
            item.country = (item.country == null) ? "country" : item.country;

            dbStoredProcedure.stockWarehouseUpdate(
                item.stock_warehouse_id,
                item.name, item.email_address, item.phone_number,
                item.address_line_1, item.address_line_2, item.city,
                item.zip_code, item.state, item.country, username
            );
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