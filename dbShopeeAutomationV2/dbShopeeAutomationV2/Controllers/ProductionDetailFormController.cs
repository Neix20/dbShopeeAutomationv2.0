using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductionDetailFormController : AdminController
    {

        // GET: ProductionDetailForm
        public ActionResult Index(int? production_id)
        {
            ViewData["production_id"] = (production_id == null) ? 1 : production_id;

            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ProductionDetailFormGridViewPartial(int production_id)
        {
            var model = db.TShopeeProductionDetails.Where(it => it.production_id == production_id);
            return PartialView("_ProductionDetailFormGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailFormGridViewPartialAddNew(TShopeeProductionDetail item)
        {
            string username = User.Identity.Name;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;
            item.manufactured_date = (item.manufactured_date == null) ? DateTime.Now : item.manufactured_date;
            item.expiry_date = (item.expiry_date == null) ? DateTime.Now : item.expiry_date;

            dbStoredProcedure.productionDetailInsert(item.UOM, item.manufactured_date, item.expiry_date, item.quantity, (int) item.product_id, item.production_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id);
            return PartialView("_ProductionDetailFormGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailFormGridViewPartialUpdate(TShopeeProductionDetail item)
        {
            string username = User.Identity.Name;

            item.production_detail_id = db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == item.production_detail_id).production_detail_id;
            item.quantity = (item.quantity == null) ? 0 : item.quantity;
            item.manufactured_date = (item.manufactured_date == null) ? DateTime.Now : item.manufactured_date;
            item.expiry_date = (item.expiry_date == null) ? DateTime.Now : item.expiry_date;

            dbStoredProcedure.productionDetailUpdate(item.production_detail_id, item.UOM, item.manufactured_date, item.expiry_date, item.quantity, (int) item.product_id, item.production_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id);
            return PartialView("_ProductionDetailFormGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailFormGridViewPartialDelete(int production_detail_id)
        {
            var item = db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == production_detail_id);

            dbStoredProcedure.productionDetailDelete(production_detail_id);
            db.SaveChanges();
            
            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id);
            return PartialView("_ProductionDetailFormGridViewPartial", model.ToList());
        }

        [HttpPost]
        public ActionResult completeProduction(int production_id)
        {
            string username = User.Identity.Name;

            var proModel = db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id);
            proModel.status = "Complete";
            dbStoredProcedure.productionUpdate(proModel.production_id, proModel.title, proModel.description, proModel.status, username);
            db.SaveChanges();

            var production_detail_list = db.TShopeeProductionDetails.Where(it => it.production_id == production_id);

            production_detail_list.ToList().ForEach(production_detail =>
            {
                int quantity = (int)production_detail.quantity;
                int product_id = (int)production_detail.product_id;

                var siModel = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == product_id);
                siModel.stock_quantity = siModel.stock_quantity + quantity;

                dbStoredProcedure.stockItemUpdate(siModel.stock_item_id, siModel.name, siModel.description, (int)siModel.stock_quantity, (int)siModel.product_id, (int)siModel.stock_warehouse_id, username);
            });

            return RedirectToAction("Index", "Production");
        }
    }
}