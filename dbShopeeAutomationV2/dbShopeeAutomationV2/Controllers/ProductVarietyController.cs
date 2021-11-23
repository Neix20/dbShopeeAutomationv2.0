using DevExpress.Web.Mvc;
using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductVarietyController : Controller
    {
        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // GET: ProductVariety
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProductVarietyGridViewPartial()
        {
            var model = db.TShopeeProductVarieties;
            return PartialView("_ProductVarietyGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductVarietyGridViewPartialAddNew(TShopeeProductVariety item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;

            db.NSP_TShopeeDetail_Insert("Normal", "-", username, currentTime, username, currentTime);
            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

            db.NSP_TShopeeProductVariety_Insert(item.name, detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductVarieties;
            return PartialView("_ProductVarietyGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductVarietyGridViewPartialUpdate(TShopeeProductVariety item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;
            int detail_id = (int)db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == item.product_variety_id).detail_id;

            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            db.NSP_TShopeeDetail_Update(detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username, currentTime);
            db.SaveChanges();

            db.NSP_TShopeeProductVariety_Update(item.product_variety_id, item.name, detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductVarieties;
            return PartialView("_ProductVarietyGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductVarietyGridViewPartialDelete(int product_variety_id)
        {
            db.NSP_TShopeeProductVariety_Delete(product_variety_id);
            db.SaveChanges();

            var model = db.TShopeeProductVarieties;
            return PartialView("_ProductVarietyGridViewPartial", model.ToList());
        }
    }
}