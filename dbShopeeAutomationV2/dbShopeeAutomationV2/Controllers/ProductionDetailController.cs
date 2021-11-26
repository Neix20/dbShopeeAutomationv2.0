using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductionDetailController : Controller
    {
        // GET: ProductionDetail
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartial()
        {
            var model = db.TShopeeProductionDetails;
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartialAddNew(TShopeeProductionDetail item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.productionDetailInsert(item.UOM, item.manufactured_date, item.expiry_date, item.quantity, (int)item.product_id, item.production_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails;
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartialUpdate(TShopeeProductionDetail item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.productionDetailUpdate(item.production_detail_id, item.UOM, item.manufactured_date, item.expiry_date, item.quantity, (int) item.product_id, item.production_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails;
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartialDelete(int production_detail_id)
        {
            dbStoredProcedure.productionDetailDelete(production_detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails;
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }
    }
}