using DevExpress.Web.Mvc;
using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductBrandController : AdminController
    {
        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // GET: ProductBrand
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProductBrandGridViewPartial()
        {
            var model = db.TShopeeProductBrands;
            return PartialView("_ProductBrandGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductBrandGridViewPartialAddNew(TShopeeProductBrand item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;

            db.NSP_TShopeeDetail_Insert("Normal", "-", username, currentTime, username, currentTime);
            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

            db.NSP_TShopeeProductBrand_Insert(item.name, detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductBrands;
            return PartialView("_ProductBrandGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductBrandGridViewPartialUpdate(TShopeeProductBrand item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;
            int detail_id = (int) db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == item.product_brand_id).detail_id;

            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            db.NSP_TShopeeDetail_Update(detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username, currentTime);
            db.SaveChanges();

            db.NSP_TShopeeProductBrand_Update(item.product_brand_id, item.name, detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductBrands;
            return PartialView("_ProductBrandGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductBrandGridViewPartialDelete(int product_brand_id)
        {
            db.NSP_TShopeeProductBrand_Delete(product_brand_id);
            db.SaveChanges();

            var model = db.TShopeeProductBrands;
            return PartialView("_ProductBrandGridViewPartial", model.ToList());
        }
    }
}