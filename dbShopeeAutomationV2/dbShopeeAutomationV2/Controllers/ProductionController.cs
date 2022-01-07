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
            int last_production_id = dbStoredProcedure.getID("TShopeeProduction") + 1;
            ViewData["new_production_title"] = generalFunc.GenProductionCode(last_production_id);
            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewPartialAddNew(TShopeeProduction item)
        {
            string username = User.Identity.Name;

            int last_production_id = db.TShopeeProductions.ToList().Count + 1;
            item.title = (item.title == null) ? generalFunc.GenProductionCode(last_production_id) : item.title;
            item.description = (item.description == null) ? "" : item.description;
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

            int last_production_id = db.TShopeeProductions.ToList().Count;
            item.title = (item.title == null) ? generalFunc.GenProductionCode(last_production_id) : item.title;
            item.description = (item.description == null) ? "" : item.description;
            item.staff_name = (item.staff_name == null) ? username : item.staff_name;
            item.created_date = (item.created_date == null) ? DateTime.Now : item.created_date;
            item.total_usage = (item.total_usage == null) ? 0 : item.total_usage;

            dbStoredProcedure.productionUpdate(item.production_id, item.title, item.description, item.staff_name, item.created_date, item.total_usage, item.production_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewPartialDelete(int production_id)
        {
            // Get Production Status
            var production = db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id);
            int pro_sta_id = (int) production.production_status_id;

            // Delete List of Production Detail
            var production_detail_list = db.TShopeeProductionDetails.Where(it => it.production_id == production_id).ToList();
            production_detail_list.ForEach(tmp_model =>
            {
                dbStoredProcedure.productionDetailDelete(tmp_model.production_detail_id);
            });

            dbStoredProcedure.productionDelete(production_id);
            db.SaveChanges();

            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridViewPartial", model.ToList());
        }

        [HttpPost]
        public ActionResult CompleteProduction()
        {
            string username = User.Identity.Name;

            string production_id_str = generalFunc.trimStr(Request.Form["production_id"]);
            int production_id = int.Parse(production_id_str);

            // Update Production
            var production = db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id);

            production.production_status_id = dbStatusFunction.productionStatusID("Complete");

            int material_model_id = dbStatusFunction.productModelID("Material");

            // Update Stock Item
            var production_detail_list = db.TShopeeProductionDetails.Where(it => it.production_id == production_id).ToList();
            production_detail_list.ForEach(tmp_model =>
            {
                var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == tmp_model.product_id);
                var stock_item = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == tmp_model.product_id);
                if (product.product_model_id == material_model_id)
                {
                    stock_item.stock_quantity -= tmp_model.quantity;
                    production.total_usage = tmp_model.quantity;
                }
                else
                {
                    stock_item.stock_quantity += tmp_model.can_be_used;
                }
            });
            db.SaveChanges();

            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridViewPartial", model.ToList());
        }
    }
}