using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult InvoiceGridViewPartial()
        {
            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceGridViewPartialAddNew(TShopeeInvoice item)
        {
            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceGridViewPartialUpdate(TShopeeInvoice item)
        {
            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceGridViewPartialDelete(int invoice_id)
        {
            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceGridViewPartial", model.ToList());
        }
    }
}