using DevExpress.Web.Mvc;
using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductV2Controller : Controller
    {
        // GET: ProductV2
        public ActionResult Index()
        {
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

            item.product_code = (item.product_code == null || item.product_code.Equals("")) ? generalFunc.Random10DigitCode() : item.product_code;
            item.name = (item.name == null) ? "product_name" : item.name;
            item.description = (item.description == null) ? "product_description" : item.description;

            string product_brand = db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == item.product_brand_id).name;
            string product_type = db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == item.product_type_id).name;
            string product_variety = db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == item.product_variety_id).name;
            item.SKU = (item.SKU == null) ? generalFunc.GenSKU(product_brand, product_type, product_variety) : item.SKU;
            item.SKU2 = (item.SKU2 == null) ? $"{item.SKU}2" : item.SKU2;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;

            dbStoredProcedure.productInsert(
                item.product_code, item.name,
                item.description, item.SKU, item.SKU2,
                item.buy_price, item.sell_price,
                item.product_brand_id, item.product_model_id, item.product_category_id,
                item.product_type_id, item.product_variety_id, item.product_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialUpdate(TShopeeProduct item)
        {
            string username = User.Identity.Name;

            item.product_code = (item.product_code == null || item.product_code.Equals("")) ? generalFunc.Random10DigitCode() : item.product_code;
            item.name = (item.name == null) ? "product_name" : item.name;
            item.description = (item.description == null) ? "product_description" : item.description;

            string product_brand = db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == item.product_brand_id).name;
            string product_type = db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == item.product_type_id).name;
            string product_variety = db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == item.product_variety_id).name;
            item.SKU = (item.SKU == null) ? generalFunc.GenSKU(product_brand, product_type, product_variety) : item.SKU;
            item.SKU2 = (item.SKU2 == null) ? $"{item.SKU}2" : item.SKU2;

            item.buy_price = (item.buy_price == null) ? 0 : item.buy_price;
            item.sell_price = (item.sell_price == null) ? 0 : item.sell_price;

            dbStoredProcedure.productUpdate(
                item.product_id, item.product_code, item.name, 
                item.description, item.SKU, item.SKU2, 
                item.buy_price, item.sell_price, 
                item.product_brand_id, item.product_model_id, item.product_category_id, 
                item.product_type_id, item.product_variety_id, item.product_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialDelete(int product_id)
        {
            dbStoredProcedure.productDelete(product_id);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }
    }
}