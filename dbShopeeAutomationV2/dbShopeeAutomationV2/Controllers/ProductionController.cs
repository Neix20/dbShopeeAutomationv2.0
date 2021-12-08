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
        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // GET: Production
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int? production_id)
        {
            TShopeeProduction tmp_model;
            if (production_id == null)
            {
                int count = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeProduction') AS INT)").FirstOrDefault() + 1;
                tmp_model = new TShopeeProduction(count);
            }
            else
            {
                tmp_model = db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id);
            }
            return View(tmp_model);
        }

        [HttpPost]
        public ActionResult Create(TShopeeProduction model)
        {
            string username = User.Identity.Name;

            model.title = (model.title == null) ? "production_title" : model.title;
            model.description = (model.description == null) ? "production_description" : model.description;
            model.production_status_id = db.TShopeeProductionStatus.FirstOrDefault(it => it.name.ToLower().Equals("Incomplete".ToLower())).production_status_id;

            var tmpModel = db.TShopeeProductions.FirstOrDefault(it => it.production_id == model.production_id);
            if (tmpModel != null)
            {
                dbStoredProcedure.productionUpdate(model.production_id, model.title, model.description, model.production_status_id, username);
            }
            else
            {
                dbStoredProcedure.productionInsert(model.title, model.description, model.production_status_id, username);
            }
            db.SaveChanges();

            return RedirectToAction("Index", "ProductionDetailForm", new { production_id = model.production_id });
        }

        [ValidateInput(false)]
        public ActionResult ProductionGridViewPartial()
        {
            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewPartialAddNew(TShopeeProduction item)
        {
            string username = User.Identity.Name;

            item.title = (item.title == null) ? "production_title" : item.title;
            item.description = (item.description == null) ? "production_description" : item.description;

            dbStoredProcedure.productionInsert(item.title, item.description, item.production_status_id, username);
            db.SaveChanges();

            return RedirectToAction("Create", "Production", new { production_id = item.production_id });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewPartialUpdate(TShopeeProduction item)
        {
            string username = User.Identity.Name;

            item.title = (item.title == null) ? "production_title" : item.title;
            item.description = (item.description == null) ? "production_description" : item.description;

            dbStoredProcedure.productionUpdate(item.production_id, item.title, item.description, item.production_status_id, username);
            db.SaveChanges();

            return RedirectToAction("Create", "Production", new { production_id = item.production_id });
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