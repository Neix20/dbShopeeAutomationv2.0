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
        public static string removeFirstAndLast(string str)
        {
            return str.Substring(1, str.Length - 2);
        }

        // GET: ProductionDetailForm
        public ActionResult Index(int? production_id)
        {
            production_id = (production_id == null) ? 1 : production_id;
            ViewData["production_id"] = production_id;

            List<SelectListItem> tmp_list = new List<SelectListItem>();
            db.TShopeeProducts.Select(x => x.SKU).Distinct().ToList().ForEach(product_sku =>
            {
                tmp_list.Add(new SelectListItem() { Text = product_sku, Value = product_sku });
            });
            ViewData["product_sku_option"] = tmp_list;

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

            string product_sku = removeFirstAndLast(Request.Form["Product SKU"]);
            int product_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.SKU.ToLower().Equals(product_sku.ToLower())).product_id;

            dbStoredProcedure.productionDetailInsert(item.UOM, item.manufactured_date, item.expiry_date, item.quantity, product_id, item.production_id, username);

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id);
            return PartialView("_ProductionDetailFormGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailFormGridViewPartialUpdate(TShopeeProductionDetail item)
        {
            string username = User.Identity.Name;

            string product_sku = Request.Form["Product SKU"];
            int product_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.SKU.ToLower().Equals(product_sku.ToLower())).product_id;

            dbStoredProcedure.productionDetailUpdate(item.production_detail_id, item.UOM, item.manufactured_date, item.expiry_date, item.quantity, product_id, item.production_id, username);

            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id);
            return PartialView("_ProductionDetailFormGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailFormGridViewPartialDelete(int production_detail_id)
        {
            var item = db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == production_detail_id);
            var model = db.TShopeeProductionDetails.Where(it => it.production_id == item.production_id); ;
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