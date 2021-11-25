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
            List<SelectListItem> product_sku_list = new List<SelectListItem>();
            db.TShopeeProducts.Select(x => x.SKU).Distinct().ToList().ForEach(product_sku =>
            {
                product_sku_list.Add(new SelectListItem() { Text = product_sku, Value = product_sku });
            });
            ViewData["product_sku_option"] = product_sku_list;

            List<SelectListItem> production_id_list = new List<SelectListItem>();
            db.TShopeeProductions.ToList().ForEach(model =>
            {
                production_id_list.Add(new SelectListItem() { Text = model.title, Value = $"{model.production_id}" });
            });
            ViewData["production_id_option"] = production_id_list;
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

            string product_sku = generalFunc.trimStr(Request.Form["Product SKU"]);
            int product_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.SKU.ToLower().Equals(product_sku.ToLower())).product_id;

            dbStoredProcedure.productionDetailInsert(item.UOM, item.manufactured_date, item.expiry_date, item.quantity, product_id, item.production_id, username);
            db.SaveChanges();

            var model = db.TShopeeProductionDetails;
            return PartialView("_ProductionDetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductionDetailGridViewPartialUpdate(TShopeeProductionDetail item)
        {
            string username = User.Identity.Name;

            string production_title = generalFunc.trimStr(Request.Form["Production Title"]);
            item.production_id = db.TShopeeProductions.FirstOrDefault(it => it.title.ToLower().Equals(production_title.ToLower())).production_id;

            string product_sku = generalFunc.trimStr(Request.Form["Product SKU"]);
            int product_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.SKU.ToLower().Equals(product_sku.ToLower())).product_id;

            dbStoredProcedure.productionDetailUpdate(item.production_detail_id, item.UOM, item.manufactured_date, item.expiry_date, item.quantity, product_id, item.production_id, username);
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