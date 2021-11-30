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

        public static string randomCode(int length = 10)
        {
            Random rand = new Random();
            string str = "";
            for (int i = 0; i < length; i++) str += $"{rand.Next(0, 10) + 1}";
            return str;
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

            item.invoice_date = (item.invoice_date == null) ? DateTime.Now : item.invoice_date;
            item.shipping_fee = (item.shipping_fee == null) ? 0 : item.shipping_fee;
            item.invoice_title = (item.invoice_title == null) ? randomCode() : item.invoice_title;

            dbStoredProcedure.invoiceInsert(item.invoice_title, item.invoice_date, item.invoice_details, item.shipping_fee, item.invoice_status_id, item.payment_method_id, item.order_id, item.customer_id, username);
            db.SaveChanges();

            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceGridViewPartialUpdate(TShopeeInvoice item)
        {
            string username = User.Identity.Name;

            item.invoice_date = (item.invoice_date == null) ? DateTime.Now : item.invoice_date;
            item.shipping_fee = (item.shipping_fee == null) ? 0 : item.shipping_fee;
            item.invoice_title = (item.invoice_title == null) ? randomCode() : item.invoice_title;

            dbStoredProcedure.invoiceUpdate(item.invoice_id, item.invoice_title, item.invoice_date, item.invoice_details, item.shipping_fee, item.invoice_status_id, item.payment_method_id, item.order_id, item.customer_id, username);
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