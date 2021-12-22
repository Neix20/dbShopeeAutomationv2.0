using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductCategoryController : AdminController
    {
        // GET: ProductCategory
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductCategoryGridViewPartial()
        {
            var model = db.TShopeeProductCategories;
            return PartialView("_ProductCategoryGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductCategoryGridViewPartialAddNew(TShopeeProductCategory item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "product_category" : item.name;
            item.code = (item.code == null) ? "category_code" : item.code;

            dbStoredProcedure.productCategoryInsert(item.name, item.code, username);
            db.SaveChanges();

            var model = db.TShopeeProductCategories;
            return PartialView("_ProductCategoryGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductCategoryGridViewPartialUpdate(TShopeeProductCategory item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "product_category" : item.name;
            item.code = (item.code == null) ? "category_code" : item.code;

            dbStoredProcedure.productCategoryUpdate(item.product_category_id, item.name, item.code, username);
            db.SaveChanges();

            var model = db.TShopeeProductCategories;
            return PartialView("_ProductCategoryGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductCategoryGridViewPartialDelete(int product_category_id)
        {
            dbStoredProcedure.productCategoryDelete(product_category_id);
            db.SaveChanges();

            var model = db.TShopeeProductCategories;
            return PartialView("_ProductCategoryGridViewPartial", model.ToList());
        }
    }
}