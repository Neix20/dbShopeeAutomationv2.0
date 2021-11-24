using DevExpress.Web.Mvc;
using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductVarietyController : AdminController
    {
        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // GET: ProductVariety
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProductVarietyGridViewPartial()
        {
            var model = db.TShopeeProductVarieties;
            return PartialView("_ProductVarietyGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductVarietyGridViewPartialAddNew(TShopeeProductVariety item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.productVarietyInsert(item.name, username);
            db.SaveChanges();

            var model = db.TShopeeProductVarieties;
            return PartialView("_ProductVarietyGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductVarietyGridViewPartialUpdate(TShopeeProductVariety item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.productVarietyUpdate(item.product_variety_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeeProductVarieties;
            return PartialView("_ProductVarietyGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductVarietyGridViewPartialDelete(int product_variety_id)
        {
            dbStoredProcedure.productVarietyDelete(product_variety_id);
            db.SaveChanges();

            var model = db.TShopeeProductVarieties;
            return PartialView("_ProductVarietyGridViewPartial", model.ToList());
        }
    }
}