using DevExpress.Web.Mvc;
using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductBrandController : AdminController
    {
        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // GET: ProductBrand
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProductBrandGridViewPartial()
        {
            var model = db.TShopeeProductBrands;
            return PartialView("_ProductBrandGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductBrandGridViewPartialAddNew(TShopeeProductBrand item)
        {
            dbStoredProcedure.productBrandInsert(item.name, User.Identity.Name);
            db.SaveChanges();

            var model = db.TShopeeProductBrands;
            return PartialView("_ProductBrandGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductBrandGridViewPartialUpdate(TShopeeProductBrand item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.productBrandUpdate(item.product_brand_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeeProductBrands;
            return PartialView("_ProductBrandGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductBrandGridViewPartialDelete(int product_brand_id)
        {
            dbStoredProcedure.productBrandDelete(product_brand_id);
            db.SaveChanges();

            var model = db.TShopeeProductBrands;
            return PartialView("_ProductBrandGridViewPartial", model.ToList());
        }
    }
}