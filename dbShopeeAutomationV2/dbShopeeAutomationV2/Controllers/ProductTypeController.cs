using DevExpress.Web.Mvc;
using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductTypeController : AdminController
    {
        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // GET: ProductType
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ProductTypeGridViewPartial()
        {
            var model = db.TShopeeProductTypes;
            return PartialView("_ProductTypeGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductTypeGridViewPartialAddNew(TShopeeProductType item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;

            db.NSP_TShopeeDetail_Insert("Normal", "-", username, currentTime, username, currentTime);
            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

            db.NSP_TShopeeProductType_Insert(item.name, detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductTypes;
            return PartialView("_ProductTypeGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ProductTypeGridViewPartialUpdate(TShopeeProductType item)
        {
            string username = User.Identity.Name;
            DateTime currentTime = DateTime.Now;
            int detail_id = (int)db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == item.product_type_id).detail_id;

            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            db.NSP_TShopeeDetail_Update(detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username, currentTime);
            db.SaveChanges();

            db.NSP_TShopeeProductType_Update(item.product_type_id, item.name, detail_id);
            db.SaveChanges();

            var model = db.TShopeeProductTypes;
            return PartialView("_ProductTypeGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ProductTypeGridViewPartialDelete(int product_type_id)
        {
            db.NSP_TShopeeProductType_Delete(product_type_id);
            db.SaveChanges();

            var model = db.TShopeeProductTypes;
            return PartialView("_ProductTypeGridViewPartial", model.ToList());
        }
    }
}