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
            int name_count = name.Split(' ').Length - 1;
            name = (name_count != 1) ? "first_name last_name" : name;

            string[] name_arr = name.Split(new[] { " " }, StringSplitOptions.None);

            string first_name = name_arr[0];
            string last_name = name_arr[1];

            item.dob = (item.dob == null) ? DateTime.Now : item.dob;
            DateTime dob = (DateTime)item.dob;

            string email_address = (item.email_address == null) ? generalFunc.GenEmail() : item.email_address;

            string phone_number = (item.phone_number == null) ? generalFunc.GenPhoneNum() : item.phone_number;

            int platform_id = (int)item.platform_id;

            string address = generalFunc.trimStr(Request.Form["Address"]);

            int address_count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (address_count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            string username = User.Identity.Name;

            dbStoredProcedure.customerInsert(first_name, last_name, dob, email_address, phone_number, address_line_1, address_line_2, city, zip_code, state, country, platform_id, username);
            db.SaveChanges();

            var model = db.TShopeeCustomers;
            return PartialView("~/Views/Customer/_CustomerGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialUpdate(TShopeeCustomer item)
        {
            int customer_id = item.customer_id;

            string name = generalFunc.trimStr(Request.Form["Name"]);
            int name_count = name.Split(' ').Length - 1;
            name = (name_count != 1) ? "first_name last_name" : name;

            string[] name_arr = name.Split(new[] { " " }, StringSplitOptions.None);

            string first_name = name_arr[0];
            string last_name = name_arr[1];

            item.dob = (item.dob == null) ? DateTime.Now : item.dob;
            DateTime dob = (DateTime)item.dob;

            string email_address = (item.email_address == null) ? generalFunc.GenEmail() : item.email_address;

            string phone_number = (item.phone_number == null) ? generalFunc.GenPhoneNum() : item.phone_number;

            int platform_id = (int)item.platform_id;

            string address = generalFunc.trimStr(Request.Form["Address"]);

            int address_count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (address_count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            string username = User.Identity.Name;

            dbStoredProcedure.customerUpdate(customer_id, first_name, last_name, dob, email_address, phone_number, address_line_1, address_line_2, city, zip_code, state, country, platform_id, username);
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