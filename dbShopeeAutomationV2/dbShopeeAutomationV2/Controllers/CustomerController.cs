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
            var model = db.TShopeeCustomers;
            return PartialView("~/Views/Customer/_CustomerGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialUpdate(TShopeeCustomer item)
        {
            var model = db.TShopeeCustomers;
            return PartialView("~/Views/Customer/_CustomerGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CustomerGridViewPartialDelete(int customer_id)
        {
            var model = db.TShopeeCustomers;
            return PartialView("~/Views/Customer/_CustomerGridViewPartial.cshtml", model.ToList());
        }

    }
}