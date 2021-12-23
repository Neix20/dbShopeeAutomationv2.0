using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductionStatusController : AdminController
    {
        // GET: ProductionStatus
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductionStatusGridViewPartial()
        {
            var model = db.TShopeeProductionStatus;
            return PartialView("_ProductionStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionStatusGridViewPartialAddNew(TShopeeProductionStatu item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "production_status" : item.name;

            dbStoredProcedure.productionStatusInsert(item.name, username);
            db.SaveChanges();

            var model = db.TShopeeProductionStatus;
            return PartialView("_ProductionStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionStatusGridViewPartialUpdate(TShopeeProductionStatu item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "production_status" : item.name;

            dbStoredProcedure.productionStatusUpdate(item.production_status_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeeProductionStatus;
            return PartialView("_ProductionStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionStatusGridViewPartialDelete(int production_status_id)
        {
            dbStoredProcedure.productionStatusDelete(production_status_id);
            db.SaveChanges();

            var model = db.TShopeeProductionStatus;
            return PartialView("_ProductionStatusGridViewPartial", model.ToList());
        }
    }
}