using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class InvoiceController : AdminController
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
            string username = User.Identity.Name;

            int last_invoice_id = dbStoredProcedure.getID("TShopeeInvoice") + 1;
            item.invoice_title = (item.invoice_title == null) ? generalFunc.GenInvoiceCode(last_invoice_id) : item.invoice_title;

            item.invoice_details = (item.invoice_details == null) ? "Invoice Details" : item.invoice_details;
            item.invoice_created_date = (item.invoice_created_date == null) ? DateTime.Now : item.invoice_created_date;
            item.invoice_status_id = dbStatusFunction.invoiceStatusID("Incomplete");
            item.shipping_fee = (item.shipping_fee == null) ? 0 : item.shipping_fee;

            dbStoredProcedure.invoiceInsert(item.invoice_title, item.invoice_created_date, item.invoice_completed_date, item.invoice_details, item.shipping_fee, item.invoice_status_id, item.payment_method_id, item.order_id, item.customer_id, username);
            db.SaveChanges();

            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceGridViewPartialUpdate(TShopeeInvoice item)
        {
            string username = User.Identity.Name;

            int last_invoice_id = dbStoredProcedure.getID("TShopeeInvoice");
            item.invoice_title = (item.invoice_title == null) ? generalFunc.GenInvoiceCode(last_invoice_id) : item.invoice_title;

            item.invoice_details = (item.invoice_details == null) ? "Invoice Details" : item.invoice_details;
            item.invoice_created_date = (item.invoice_created_date == null) ? DateTime.Now : item.invoice_created_date;
            item.shipping_fee = (item.shipping_fee == null) ? 0 : item.shipping_fee;
            item.invoice_title = (item.invoice_title == null) ? generalFunc.Random10DigitCode() : item.invoice_title;

            dbStoredProcedure.invoiceUpdate(item.invoice_id, item.invoice_title, item.invoice_created_date, item.invoice_completed_date, item.invoice_details, item.shipping_fee, item.invoice_status_id, item.payment_method_id, item.order_id, item.customer_id, username);
            db.SaveChanges();

            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceGridViewPartialDelete(int invoice_id)
        {
            dbStoredProcedure.invoiceDelete(invoice_id);
            db.SaveChanges();

            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceGridViewPartial", model.ToList());
        }
    }
}