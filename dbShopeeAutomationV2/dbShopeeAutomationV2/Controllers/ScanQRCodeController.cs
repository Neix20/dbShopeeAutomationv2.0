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

        [HttpPost]
        public ActionResult SubmitCode(string invoice_title)
        {
            var model = db.TShopeeInvoices.FirstOrDefault(it => it.invoice_title.ToLower().Equals(invoice_title.ToLower()));

            // Check If Invoice Code Exist in Database
            if(model == null) {
                return Content("Error! Invalid Invoice Code!");
            }

            int c_inv_sta_id = dbStatusFunction.invoiceStatusID("Complete");

            if (c_inv_sta_id == -1) {
                return Content("Error! Invoice Status Code for 'Complete' does not exist!");
            }
            
            if(model.invoice_status_id != c_inv_sta_id)
            {
                string username = User.Identity.Name;

                model.invoice_status_id = c_inv_sta_id;
                model.invoice_completed_date = DateTime.Now;

                db.SaveChanges();

                return Content($"Success! Invoice Code {model.invoice_title} was successfully updated!");
            }

            return Content($"Error! You have already scanned Invoice Code {model.invoice_title}!");
        }
    }
}