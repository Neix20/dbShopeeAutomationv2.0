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

            int name_count = name.Split(' ').Length - 1;
            name = (name_count != 1) ? "first_name last_name" : name;

            int address_count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (address_count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

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

            int count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

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