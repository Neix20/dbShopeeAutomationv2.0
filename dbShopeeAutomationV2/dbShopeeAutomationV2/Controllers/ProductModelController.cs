using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductModelController : AdminController
    {
        // GET: ProductModel
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductModelGridViewPartial()
        {
            var model = db.TShopeeProductModels;
            return PartialView("_ProductModelGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductModelGridViewPartialAddNew(TShopeeProductModel item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "product_model" : item.name;
            item.code = (item.code == null) ? "model_code" : item.code;

            dbStoredProcedure.productModelInsert(item.name, item.code, username);
            db.SaveChanges();

            var model = db.TShopeeProductModels;
            return PartialView("_ProductModelGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductModelGridViewPartialUpdate(TShopeeProductModel item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "product_model" : item.name;
            item.code = (item.code == null) ? "model_code" : item.code;

            dbStoredProcedure.productModelUpdate(item.product_model_id, item.name, item.code, username);
            db.SaveChanges();

            var model = db.TShopeeProductModels;
            return PartialView("_ProductModelGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductModelGridViewPartialDelete(int product_model_id)
        {
            dbStoredProcedure.productModelDelete(product_model_id);
            db.SaveChanges();

            var model = db.TShopeeProductModels;
            return PartialView("_ProductModelGridViewPartial", model.ToList());
        }
    }
}