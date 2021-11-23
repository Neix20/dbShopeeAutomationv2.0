using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class OrderItemStatusController : Controller
    {
        // GET: OrderItemStatus
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult OrderItemStatusGridViewPartial()
        {
            var model = db.TShopeeOrderItemStatus;
            return PartialView("_OrderItemStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemStatusGridViewPartialAddNew(TShopeeOrderItemStatu item)
        {
            var model = db.TShopeeOrderItemStatus;
            return PartialView("_OrderItemStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemStatusGridViewPartialUpdate(TShopeeOrderItemStatu item)
        {
            var model = db.TShopeeOrderItemStatus;
            return PartialView("_OrderItemStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemStatusGridViewPartialDelete(int order_item_status_id)
        {
            var model = db.TShopeeOrderItemStatus;
            return PartialView("_OrderItemStatusGridViewPartial", model.ToList());
        }
    }
}