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
            ViewData["product_brand_option"] = db.TShopeeProductBrands.Select(x => x.name).ToList();
            ViewData["product_type_option"] = db.TShopeeProductTypes.Select(x => x.name).ToList();
            ViewData["product_variety_option"] = db.TShopeeProductVarieties.Select(x => x.name).ToList();

            List<SelectListItem> tmp_list = new List<SelectListItem>();
            db.TShopeeStockWarehouses.ToList().ForEach(model =>
            {
                tmp_list.Add(new SelectListItem() { Text = model.name, Value = $"{model.stock_warehouse_id}" });
            });
            ViewData["stock_warehouse_option"] = tmp_list;
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
            DateTime currentTime = DateTime.Now;

            db.NSP_TShopeeDetail_Insert("Normal", "-", username, currentTime, username, currentTime);
            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

            db.NSP_TShopeeProduct_Insert(item.product_code, item.name, item.description, item.SKU, item.SKU2, item.buy_price, item.sell_price, item.product_brand, item.product_type, item.product_variety, detail_id);
            db.SaveChanges();

            // Check if Product Brand Exist
            var product_brand_list = db.TShopeeProductBrands.Select(x => x.name.ToLower()).ToList();
            string product_brand = item.product_brand;
            if (!product_brand_list.Contains(product_brand.ToLower()))
            {
                db.NSP_TShopeeDetail_Insert("Normal", "-", username, currentTime, username, currentTime);
                detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

                db.NSP_TShopeeProductBrand_Insert(product_brand, detail_id);
                db.SaveChanges();
            }

            // Check if Product Type Exist
            var product_type_list = db.TShopeeProductTypes.Select(x => x.name.ToLower()).ToList();
            string product_type = item.product_type;
            if (!product_type_list.Contains(product_type.ToLower()))
            {
                db.NSP_TShopeeDetail_Insert("Normal", "-", username, currentTime, username, currentTime);
                detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

                db.NSP_TShopeeProductType_Insert(product_type, detail_id);
                db.SaveChanges();
            }

            // Check if Product Variety Exist
            var product_variety_list = db.TShopeeProductVarieties.Select(x => x.name.ToLower()).ToList();
            string product_variety = item.product_variety;
            if (!product_variety_list.Contains(product_variety.ToLower()))
            {
                db.NSP_TShopeeDetail_Insert("Normal", "-", username, currentTime, username, currentTime);
                detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

                db.NSP_TShopeeProductVariety_Insert(product_variety, detail_id);
                db.SaveChanges();
            }

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialUpdate(TShopeeProduct item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;
            int detail_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.product_id == item.product_id).detail_id;

            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            db.NSP_TShopeeDetail_Update(detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username, currentTime);
            db.SaveChanges();

            db.NSP_TShopeeProduct_Update(item.product_id, item.product_code, item.name, item.description, item.SKU, item.SKU2, item.buy_price, item.sell_price, item.product_brand, item.product_type, item.product_variety, detail_id);
            db.SaveChanges();

            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductGridViewPartialDelete(int product_id)
        {
            var model = db.TShopeeProducts;
            return PartialView("_ProductGridViewPartial", model.ToList());
        }
    }
}