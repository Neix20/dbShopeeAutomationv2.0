using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class InvoiceStatusController : AdminController
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
            string username = User.Identity.Name;

            dbStoredProcedure.invoiceStatusInsert(item.name, item.description, username);
            db.SaveChanges();

            var model = db.TShopeeInvoiceStatus;
            return PartialView("_InvoiceStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceStatusGridViewPartialUpdate(TShopeeInvoiceStatu item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.invoiceStatusUpdate(item.invoice_status_id, item.name, item.description, username);
            db.SaveChanges();

            var model = db.TShopeeInvoiceStatus;
            return PartialView("_InvoiceStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceStatusGridViewPartialDelete(int invoice_status_id)
        {
            dbStoredProcedure.invoiceStatusDelete(invoice_status_id);
            db.SaveChanges();

            var model = db.TShopeeInvoiceStatus;
            return PartialView("_InvoiceStatusGridViewPartial", model.ToList());
        }
    }
}