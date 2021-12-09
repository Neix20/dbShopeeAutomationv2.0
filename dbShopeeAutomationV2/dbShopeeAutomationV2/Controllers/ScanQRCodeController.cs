using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ScanQRCodeController : AdminController
    {
        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // GET: ScanQRCode
        public ActionResult Index()
        {
            return View();
        }

        public int invoiceStatusID(string name)
        {
            var invoiceStatus = db.TShopeeInvoiceStatus.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (invoiceStatus == null) ? -1 : invoiceStatus.invoice_status_id;
        }

        [HttpPost]
        public ActionResult SubmitCode(string invoice_title)
        {
            var model = db.TShopeeInvoices.FirstOrDefault(it => it.invoice_title.ToLower().Equals(invoice_title.ToLower()));

            if(model == null) return Content("Error! Invalid Invoice Code!");

            int c_inv_sta_id = invoiceStatusID("Complete");

            if (c_inv_sta_id == -1) return Content("Error! Invoice Status Code for 'Complete' does not exist!");
            
            if(model.invoice_status_id != c_inv_sta_id)
            {
                model.invoice_status_id = c_inv_sta_id;
                model.invoice_completed_date = DateTime.Now;

                String username = User.Identity.Name;

                dbStoredProcedure.invoiceUpdate(model.invoice_id, model.invoice_title, model.invoice_created_date, model.invoice_completed_date, model.invoice_details, model.shipping_fee, model.invoice_status_id, model.payment_method_id, model.order_id, model.customer_id, username);
                db.SaveChanges();

                return Content($"Success! Invoice Code {model.invoice_title} was successfully updated!");
            }

            return Content($"Error! You have already scanned Invoice Code {model.invoice_title}!");
        }
    }
}