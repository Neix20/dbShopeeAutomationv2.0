using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductComponentController : AdminController
    {
        // GET: ProductComponent
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartial()
        {
            var model = db.TShopeeProductComponents;
            return PartialView("_ProductComponentGridViewPartial", model.ToList());
        }

     
        [HttpPost, ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialAddNew(TShopeeProductComponent item)
        {
            string username = User.Identity.Name;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;

            dbStoredProcedure.productComponentInsert(item.master_product_id, item.sub_product_id, item.quantity, item.type_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductComponents;
            return PartialView("_ProductComponentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialUpdate(TShopeeProductComponent item)
        {
            string username = User.Identity.Name;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;

            dbStoredProcedure.productComponentUpdate(item.product_component_id, item.master_product_id, item.sub_product_id, item.quantity, item.type_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductComponents;
            return PartialView("_ProductComponentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductComponentGridViewPartialDelete(int product_component_id)
        {
            dbStoredProcedure.productComponentDelete(product_component_id);
            db.SaveChanges();

            var model = db.TShopeeProductComponents;
            return PartialView("_ProductComponentGridViewPartial", model.ToList());
        }
    }
}