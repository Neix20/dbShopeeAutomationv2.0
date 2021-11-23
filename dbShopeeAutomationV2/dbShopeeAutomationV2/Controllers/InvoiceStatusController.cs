using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class InvoiceStatusController : Controller
    {
        // GET: InvoiceStatus
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult InvoiceStatusGridViewPartial()
        {
            var model = db.TShopeeInvoiceStatus;
            return PartialView("_InvoiceStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceStatusGridViewPartialAddNew(TShopeeInvoiceStatu item)
        {
            var model = db.TShopeeInvoiceStatus;
            return PartialView("_InvoiceStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceStatusGridViewPartialUpdate(TShopeeInvoiceStatu item)
        {
            var model = db.TShopeeInvoiceStatus;
            return PartialView("_InvoiceStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceStatusGridViewPartialDelete(int invoice_status_id)
        {
            var model = db.TShopeeInvoiceStatus;
            return PartialView("_InvoiceStatusGridViewPartial", model.ToList());
        }
    }
}