using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class PaymentMethodController : AdminController
    {
        // GET: PaymentMethod
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartial()
        {
            var model = db.TShopeePaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartialAddNew(TShopeePaymentMethod item)
        {
            var model = db.TShopeePaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartialUpdate(TShopeePaymentMethod item)
        {
            var model = db.TShopeePaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartialDelete(int payment_method_id)
        {
            var model = db.TShopeePaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }
    }
}