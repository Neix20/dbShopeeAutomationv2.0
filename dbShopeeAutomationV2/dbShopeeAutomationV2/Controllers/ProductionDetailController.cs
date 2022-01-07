using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductionDetailController : AdminController
    {
        // GET: ProductionDetail
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartial(int production_id)
        {
            ViewData["production_id"] = production_id;

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == production_id);
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartialAddNew(TShopeeProductionDetail item, int production_id)
        {
            ViewData["production_id"] = production_id;

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

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == production_id);
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartialUpdate(TShopeeProductionDetail item, int production_id)
        {
            ViewData["production_id"] = production_id;

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

            // Update Stock Item 
            var production = db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id);
            int pro_sta_id = (int)production.production_status_id;
            int c_pro_sta_id = dbStatusFunction.productionStatusID("complete");
            // Only Update Stock Item Count when Production is marked as complete
            if (pro_sta_id == c_pro_sta_id)
            {
                var oriProductionDetail = db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == item.production_detail_id);
                var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == oriProductionDetail.product_id);
                var stock_item = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == oriProductionDetail.product_id);

                int material_model_id = dbStatusFunction.productModelID("Material");

                if (product.product_model_id == material_model_id)
                {
                    stock_item.stock_quantity += oriProductionDetail.quantity;
                    stock_item.stock_quantity -= item.quantity;
                    production.total_usage = item.quantity;
                }
                else
                {
                    stock_item.stock_quantity -= oriProductionDetail.can_be_used;
                    stock_item.stock_quantity += item.can_be_used;
                }
            }

            dbStoredProcedure.productionDetailUpdate(
                item.production_detail_id, item.UOM, 
                item.manufactured_date, item.expiry_date,
                item.height, item.width, item.length,
                item.quantity, item.cannot_be_used, item.can_be_used,
                item.production_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == production_id);
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartialDelete(int production_detail_id, int production_id)
        {
            ViewData["production_id"] = production_id;

            dbStoredProcedure.productionDetailDelete(production_detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == production_id);
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }
    }
}