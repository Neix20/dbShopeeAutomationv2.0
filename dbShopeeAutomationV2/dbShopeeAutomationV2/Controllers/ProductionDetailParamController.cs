using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductionDetailParamController : AdminController
    {
        // GET: ProductionDetailParam
        public ActionResult Index(int? production_id)
        {
            production_id = (production_id == null) ? -1 : production_id;
            ViewData["production_id"] = production_id;

            int production_status_id = (int)db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id).production_status_id;
            int c_pro_sta_id = dbStatusFunction.productionStatusID("complete");

            ViewData["production_status"] = (production_status_id == c_pro_sta_id) ? "true" : "false";

            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductionDetailParamGridViewGridViewPartial(int? production_id)
        {
            production_id = (production_id == null) ? -1 : production_id;
            var model = db.TShopeeProductionDetails.Where(it => it.production_id == production_id);
            return PartialView("_ProductionDetailParamGridViewGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailParamGridViewGridViewPartialAddNew(TShopeeProductionDetail item)
        {
            string username = User.Identity.Name;

            string production_title = Request.Form["production_title"];
            item.production_id = dbStatusFunction.productionID(production_title);

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

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id);
            return PartialView("_ProductionDetailParamGridViewGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailParamGridViewGridViewPartialUpdate(TShopeeProductionDetail item)
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

            var oriProductionDetail = db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == item.production_detail_id);
            item.production_id = oriProductionDetail.production_id;

            // Update Stock Item 
            var production = db.TShopeeProductions.FirstOrDefault(it => it.production_id == oriProductionDetail.production_id);
            int c_pro_sta_id = dbStatusFunction.productionStatusID("complete");

            // Only Update Stock Item Count when Production is marked as complete
            if (production.production_status_id == c_pro_sta_id)
            {
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

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id);
            return  PartialView("_ProductionDetailParamGridViewGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailParamGridViewGridViewPartialDelete(int production_detail_id)
        {
            var item = db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == production_detail_id);
            dbStoredProcedure.productionDetailDelete(production_detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id);
            return PartialView("_ProductionDetailParamGridViewGridViewPartial", model.ToList());
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
                } else
                {
                    stock_item.stock_quantity += tmp_model.can_be_used;
                }
            });

            db.SaveChanges();

            return RedirectToAction("Index", "Production");
        }
    }
}