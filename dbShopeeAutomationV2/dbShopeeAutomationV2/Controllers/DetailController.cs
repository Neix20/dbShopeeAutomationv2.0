using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class DetailController : AdminController
    {
        // GET: Detail
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult DetailGridViewPartial()
        {
            var model = db.TShopeeDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialAddNew(TShopeeDetail item)
        {
            dbStoredProcedure.detailInsert(item);
            db.SaveChanges();

            var model = db.TShopeeDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialUpdate(TShopeeDetail item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.detailUpdate(item, username);
            db.SaveChanges();

            var model = db.TShopeeDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialDelete(int detail_id)
        {
            dbStoredProcedure.detailDelete(detail_id);
            db.SaveChanges();

            var model = db.TShopeeDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }
    }
}