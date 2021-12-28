using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductionController : AdminController
    {
        // GET: ProductionV2
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductionGridViewPartial()
        {
            int last_production_id = dbStoredProcedure.getID("TShopeeProduction");
            ViewData["new_production_title"] = generalFunc.GenProductionCode(last_production_id);
            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewPartialAddNew(TShopeeProduction item)
        {
            string username = User.Identity.Name;

            int last_production_id = dbStoredProcedure.getID("TShopeeProduction");
            item.title = (item.title == null) ? generalFunc.GenProductionCode(last_production_id) : item.title;
            item.description = (item.description == null) ? "production_description" : item.description;
            item.production_status_id = dbStatusFunction.productionStatusID("Incomplete");
            item.staff_name = (item.staff_name == null) ? username : item.staff_name;
            item.created_date = (item.created_date == null) ? DateTime.Now : item.created_date;
            item.total_usage = (item.total_usage == null) ? 0 : item.total_usage;

            dbStoredProcedure.productionInsert(item.title, item.description, item.staff_name, item.created_date, item.total_usage, item.production_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewPartialUpdate(TShopeeProduction item)
        {
            string username = User.Identity.Name;

            item.description = (item.description == null) ? "production_description" : item.description;
            item.staff_name = (item.staff_name == null) ? username : item.staff_name;
            item.created_date = (item.created_date == null) ? DateTime.Now : item.created_date;
            item.total_usage = (item.total_usage == null) ? 0 : item.total_usage;

            dbStoredProcedure.productionUpdate(item.production_id, item.title, item.description, item.staff_name, item.created_date, item.total_usage, item.production_status_id, username);
            db.SaveChanges();

            return RedirectToAction("Index", "ProductionDetailParam", new { production_id = item.production_id });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewPartialDelete(int production_id)
        {
            // Delete List of Production Detail
            var production_detail_list = db.TShopeeProductionDetails.Where(it => it.production_id == production_id).ToList();
            production_detail_list.ForEach(tmp_model =>
            {
                int production_detail_id = tmp_model.production_detail_id;
                dbStoredProcedure.productionDetailDelete(production_detail_id);
                db.SaveChanges();
            });

            dbStoredProcedure.productionDelete(production_id);
            db.SaveChanges();

            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridViewPartial", model.ToList());
        }
    }
}