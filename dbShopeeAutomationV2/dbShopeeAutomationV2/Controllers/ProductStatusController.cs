using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductStatusController : AdminController
    {
        // GET: ProductStatus
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductStatusGridViewPartial()
        {
            var model = db.TShopeeProductStatus;
            return PartialView("_ProductStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductStatusGridViewPartialAddNew(TShopeeProductStatu item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "product_category" : item.name;

            dbStoredProcedure.productStatusInsert(item.name, username);
            db.SaveChanges();

            var model = db.TShopeeProductStatus;
            return PartialView("_ProductStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductStatusGridViewPartialUpdate(TShopeeProductStatu item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "product_category" : item.name;

            dbStoredProcedure.productStatusUpdate(item.product_status_id, item.name, username);
            db.SaveChanges();
            var model = db.TShopeeProductStatus;
            return PartialView("_ProductStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductStatusGridViewPartialDelete(int product_status_id)
        {
            dbStoredProcedure.productStatusDelete(product_status_id);
            db.SaveChanges();

            var model = db.TShopeeProductStatus;
            return PartialView("_ProductStatusGridViewPartial", model.ToList());
        }
    }
}