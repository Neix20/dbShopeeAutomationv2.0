using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class OrderStatusController : AdminController
    {
        // GET: OrderStatus
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartial()
        {
            var model = db.TShopeeOrderStatus;
            return PartialView("_OrderStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartialAddNew(TShopeeOrderStatu item)
        {
            var model = db.TShopeeOrderStatus;
            return PartialView("_OrderStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartialUpdate(TShopeeOrderStatu item)
        {
            var model = db.TShopeeOrderStatus;
            return PartialView("_OrderStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartialDelete(int order_status_id)
        {
            var model = db.TShopeeOrderStatus;
            return PartialView("_OrderStatusGridViewPartial", model.ToList());
        }
    }
}