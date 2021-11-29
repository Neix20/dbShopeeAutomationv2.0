using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductController : Controller
    {
        public static string randomProductCode(int length = 10)
        {
            Random rand = new Random();
            string str = "";
            for (int i = 0; i < length; i++) str += $"{rand.Next(0, 10) + 1}";
            return str;
        }

        // GET: Product
        public ActionResult Index()
        {
            ViewData["product_code"] = randomProductCode();
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductGridViewPartial()
        {
            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialAddNew(TShopeeProduct item)
        {
            string username = User.Identity.Name;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;
            item.product_code = (item.product_code == null || item.product_code.Equals("")) ? randomProductCode() : item.product_code;

            dbStoredProcedure.productInsert(item.product_code, item.name, item.description, item.SKU, item.SKU2, item.buy_price, item.sell_price, item.product_brand_id, item.product_type_id, item.product_variety_id, username);
            db.SaveChanges();

            int product_id = db.TShopeeProducts.FirstOrDefault(it => it.SKU.Equals(item.SKU)).product_id;

            string warehouse_title = generalFunc.trimStr(Request.Form["Stock Warehouse Location"]);
            int warehouse_id = db.TShopeeStockWarehouses.FirstOrDefault(it => it.name.ToLower().Equals(warehouse_title.ToLower())).stock_warehouse_id;

            dbStoredProcedure.stockItemInsert(item.name, item.description, 0, product_id, warehouse_id, username);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialUpdate(TShopeeProduct item)
        {
            string username = User.Identity.Name;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;
            item.product_code = (item.product_code == null || item.product_code.Equals("")) ? randomProductCode() : item.product_code;

            dbStoredProcedure.productUpdate(item.product_id, item.product_code, item.name, item.description, item.SKU, item.SKU2, item.buy_price, item.sell_price, item.product_brand_id, item.product_type_id, item.product_variety_id, username);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialDelete(int product_id)
        {
            dbStoredProcedure.productDelete(product_id);
            db.SaveChanges();

            int stock_item_id = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == product_id).stock_item_id;
            dbStoredProcedure.stockItemDelete(stock_item_id);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }
    }
}