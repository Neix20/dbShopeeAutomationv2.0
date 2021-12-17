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

            item.UOM = (item.UOM == null) ? "UOM" : item.UOM;
            item.manufactured_date = (item.manufactured_date == null) ? DateTime.Now : item.manufactured_date;
            item.expiry_date = (item.expiry_date == null) ? DateTime.Now : item.expiry_date;

            item.height = (item.height == null) ? 0 : item.height;
            item.width = (item.width == null) ? 0 : item.width;
            item.length = (item.length == null) ? 0 : item.length;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;
            item.cannot_be_used = (item.cannot_be_used == null) ? 0 : item.cannot_be_used;
            item.can_be_used = (item.can_be_used == null) ? 0 : item.can_be_used;

            dbStoredProcedure.productionDetailInsert(
                item.UOM, item.manufactured_date, item.expiry_date,
                item.height, item.width, item.length,
                item.quantity, item.cannot_be_used, item.can_be_used, 
                item.production_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails;
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartialUpdate(TShopeeProductionDetail item)
        {
            string username = User.Identity.Name;

            item.UOM = (item.UOM == null) ? "UOM" : item.UOM;
            item.manufactured_date = (item.manufactured_date == null) ? DateTime.Now : item.manufactured_date;
            item.expiry_date = (item.expiry_date == null) ? DateTime.Now : item.expiry_date;

            item.height = (item.height == null) ? 0 : item.height;
            item.width = (item.width == null) ? 0 : item.width;
            item.length = (item.length == null) ? 0 : item.length;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;
            item.cannot_be_used = (item.cannot_be_used == null) ? 0 : item.cannot_be_used;
            item.can_be_used = (item.can_be_used == null) ? 0 : item.can_be_used;

            dbStoredProcedure.productionDetailUpdate(
                item.production_detail_id, item.UOM, 
                item.manufactured_date, item.expiry_date,
                item.height, item.width, item.length,
                item.quantity, item.cannot_be_used, item.can_be_used,
                item.production_id, item.product_id, username);
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