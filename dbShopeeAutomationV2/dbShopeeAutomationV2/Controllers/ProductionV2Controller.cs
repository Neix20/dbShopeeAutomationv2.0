using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductionV2Controller : Controller
    {
        // GET: ProductionV2
        public ActionResult Index()
        {
            return View();
        }

        public int productionStatusID(string name)
        {
            var productionStatus = db.TShopeeProductionStatus.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (productionStatus == null) ? 0 : productionStatus.production_status_id;
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductionGridView()
        {
            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridView", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewAddNew(TShopeeProduction item)
        {
            string username = User.Identity.Name;

            item.title = (item.title == null) ? "production_title" : item.title;
            item.description = (item.description == null) ? "production_description" : item.description;
            item.production_status_id = productionStatusID("Incomplete");

            dbStoredProcedure.productionInsert(item.title, item.description, item.total_usage, item.production_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridView", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewUpdate(TShopeeProduction item)
        {
            string username = User.Identity.Name;

            item.title = (item.title == null) ? "production_title" : item.title;
            item.description = (item.description == null) ? "production_description" : item.description;

            // If Production Status is Marked As Complete
            int c_pro_sta_id = productionStatusID("Complete");

            if(item.production_status_id == c_pro_sta_id)
            {
                int production_id = item.production_id;
                var production_detail_list = db.TShopeeProductionDetails.Where(it => it.production_id == production_id);

                production_detail_list.ToList().ForEach(production_detail =>
                {
                    int quantity = (int)production_detail.quantity;
                    int product_id = (int)production_detail.product_id;

                    var siModel = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == product_id);
                    siModel.stock_quantity = siModel.stock_quantity + quantity;

                    dbStoredProcedure.stockItemUpdate(siModel.stock_item_id, siModel.name, siModel.description, (int)siModel.stock_quantity, (int)siModel.product_id, (int)siModel.stock_warehouse_id, username);
                });
                db.SaveChanges();
            }

            dbStoredProcedure.productionUpdate(item.production_id, item.title, item.description, item.total_usage, item.production_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductions;
            return PartialView("_ProductionGridView", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionGridViewDelete(int production_id)
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
            return PartialView("_ProductionGridView", model.ToList());
        }
    }
}