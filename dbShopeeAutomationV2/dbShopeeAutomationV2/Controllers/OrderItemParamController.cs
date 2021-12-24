using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class OrderItemParamController : Controller
    {
        // GET: OrderItemParam
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult OrderItemParamGridViewPartial()
        {
            var model = db.TShopeeOrderItems;
            return PartialView("_OrderItemParamGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemParamGridViewPartialAddNew(TShopeeOrderItem item)
        {
            var model = db.TShopeeOrderItems;
            return PartialView("_OrderItemParamGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemParamGridViewPartialUpdate(TShopeeOrderItem item)
        {
            var model = db.TShopeeOrderItems;
            return PartialView("_OrderItemParamGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemParamGridViewPartialDelete(int order_item_id)
        {
            var model = db.TShopeeOrderItems;
            return PartialView("_OrderItemParamGridViewPartial", model.ToList());
        }
    }
}