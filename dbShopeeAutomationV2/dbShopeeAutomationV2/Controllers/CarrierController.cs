using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class CarrierController : Controller
    {
        // GET: Carrier
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult CarrierGridViewPartial()
        {
            var model = db.TShopeeCarriers;
            return PartialView("_CarrierGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CarrierGridViewPartialAddNew(TShopeeCarrier item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.carrierInsert(item.name, username);
            db.SaveChanges();

            var model = db.TShopeeCarriers;
            return PartialView("_CarrierGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CarrierGridViewPartialUpdate(TShopeeCarrier item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.carrierUpdate(item.carrier_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeeCarriers;
            return PartialView("_CarrierGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CarrierGridViewPartialDelete(int carrier_id)
        {
            dbStoredProcedure.carrierDelete(carrier_id);
            db.SaveChanges();

            var model = db.TShopeeCarriers;
            return PartialView("_CarrierGridViewPartial", model.ToList());
        }
    }
}