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
        // GET: Product
        public ActionResult Index()
        {
            ViewData["product_code"] = generalFunc.Random10DigitCode();
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductGridViewPartial()
        {
            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        public int stockWarehouseID(string title)
        {
            var stock_warehouse = db.TShopeeStockWarehouses.FirstOrDefault(it => it.name.ToLower().Equals(title.ToLower()));
            stock_warehouse = (stock_warehouse == null) ? db.TShopeeStockWarehouses.ToList().ElementAt(0) : stock_warehouse;
            return stock_warehouse.stock_warehouse_id;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialAddNew(TShopeeProduct item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "product_name" : item.name;
            item.description = (item.description == null) ? "product_description" : item.description;

            string product_brand = db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == item.product_brand_id).name;
            string product_variety = db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == item.product_variety_id).name;
            item.SKU = (item.SKU == null) ? generalFunc.GenSKU(product_brand, product_variety) : item.SKU;
            item.SKU2 = (item.SKU2 == null) ? $"{item.SKU}2" : item.SKU2;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;
            item.product_code = (item.product_code == null || item.product_code.Equals("")) ? generalFunc.Random10DigitCode() : item.product_code;

            dbStoredProcedure.productInsert(item.product_code, item.name, item.description, item.SKU, item.SKU2, item.buy_price, item.sell_price, item.product_brand_id, item.product_type_id, item.product_variety_id, username);
            db.SaveChanges();

            int product_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeProduct') AS INT)").FirstOrDefault();

            string warehouse_title = generalFunc.trimStr(Request.Form["Stock Warehouse Location"]);
            int warehouse_id = stockWarehouseID(warehouse_title);

            dbStoredProcedure.stockItemInsert(item.name, item.description, 0, product_id, warehouse_id, username);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialUpdate(TShopeeProduct item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "product_name" : item.name;
            item.description = (item.description == null) ? "product_description" : item.description;

            string product_brand = db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == item.product_brand_id).name;
            string product_variety = db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == item.product_variety_id).name;
            item.SKU = (item.SKU == null) ? generalFunc.GenSKU(product_brand, product_variety) : item.SKU;
            item.SKU2 = (item.SKU2 == null) ? $"{item.SKU}2" : item.SKU2;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;
            item.product_code = (item.product_code == null || item.product_code.Equals("")) ? generalFunc.Random10DigitCode() : item.product_code;

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