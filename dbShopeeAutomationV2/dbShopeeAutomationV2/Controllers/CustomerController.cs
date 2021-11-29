using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class CustomerController : AdminController
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }


        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult CustomerGridViewPartial()
        {
            var model = db.TShopeeCustomers;
            return PartialView("~/Views/Customer/_CustomerGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialAddNew(TShopeeCustomer item)
        {
            string name = generalFunc.trimStr(Request.Form["Name"]);
            DateTime dob = (DateTime)item.dob;
            string email_address = item.email_address;
            string phone_number = item.phone_number;
            string address = generalFunc.trimStr(Request.Form["Address"]);
            int platform_id = (int)item.platform_id;

            string username = User.Identity.Name;

            dbStoredProcedure.customerInsert(name, dob, email_address, phone_number, address, platform_id, username);
            db.SaveChanges();

            var model = db.TShopeeCustomers;
            return PartialView("~/Views/Customer/_CustomerGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialUpdate(TShopeeCustomer item)
        {
            int customer_id = item.customer_id;
            string name = generalFunc.trimStr(Request.Form["Name"]);
            DateTime dob = (DateTime)item.dob;
            string email_address = item.email_address;
            string phone_number = item.phone_number;
            string address = generalFunc.trimStr(Request.Form["Address"]);
            int platform_id = (int) item.platform_id;

            string username = User.Identity.Name;

            dbStoredProcedure.customerUpdate(customer_id, name, dob, email_address, phone_number, address, platform_id, username);
            db.SaveChanges();

            var model = db.TShopeeCustomers;
            return PartialView("~/Views/Customer/_CustomerGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialDelete(int customer_id)
        {
            dbStoredProcedure.customerDelete(customer_id);
            db.SaveChanges();

            var model = db.TShopeeCustomers;
            return PartialView("~/Views/Customer/_CustomerGridViewPartial.cshtml", model.ToList());
        }

    }
}