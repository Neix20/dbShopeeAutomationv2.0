using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class OrderItemStatusController : AdminController
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
            string username = User.Identity.Name;

            dbStoredProcedure.orderItemStatusInsert(item.name, item.description, 0, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItemStatus;
            return PartialView("_OrderItemStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemStatusGridViewPartialUpdate(TShopeeOrderItemStatu item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.orderItemStatusUpdate(item.order_item_status_id, item.name, item.description, item.return_merchandise_authorization_nr, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItemStatus;
            return PartialView("_OrderItemStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemStatusGridViewPartialDelete(int order_item_status_id)
        {
            dbStoredProcedure.orderItemStatusDelete(order_item_status_id);
            db.SaveChanges();

            var model = db.TShopeeOrderItemStatus;
            return PartialView("_OrderItemStatusGridViewPartial", model.ToList());
        }
    }
}