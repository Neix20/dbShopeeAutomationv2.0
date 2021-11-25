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
            string username = User.Identity.Name;

            dbStoredProcedure.paymentMethodInsert(item.name, username);
            db.SaveChanges();

            var model = db.TShopeePaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartialUpdate(TShopeePaymentMethod item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.paymentMethodUpdate(item.payment_method_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeePaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PaymentMethodGridViewPartialDelete(int payment_method_id)
        {
            dbStoredProcedure.paymentMethodDelete(payment_method_id);
            db.SaveChanges();

            var model = db.TShopeePaymentMethods;
            return PartialView("_PaymentMethodGridViewPartial", model.ToList());
        }
    }
}