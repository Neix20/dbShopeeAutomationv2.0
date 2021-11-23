using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class OrderItemController : Controller
    {
        // GET: OrderItem
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult OrderItemGridViewPartial()
        {
            var model = db.TShopeeOrderItems;
            return PartialView("~/Views/OrderItem/_OrderItemGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialAddNew(TShopeeOrderItem item)
        {
            var model = db.TShopeeOrderItems;
            return PartialView("~/Views/OrderItem/_OrderItemGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialUpdate(TShopeeOrderItem item)
        {
            var model = db.TShopeeOrderItems;
            return PartialView("~/Views/OrderItem/_OrderItemGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialDelete(int order_item_id)
        {
            var model = db.TShopeeOrderItems;
            return PartialView("~/Views/OrderItem/_OrderItemGridViewPartial.cshtml", model.ToList());
        }

    }
}