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
            string username = User.Identity.Name;
            item.name = (item.name == null) ? "order_status" : item.name;

            dbStoredProcedure.orderStatusInsert(item.name, username);
            db.SaveChanges();

            var model = db.TShopeeOrderStatus;
            return PartialView("_OrderStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartialUpdate(TShopeeOrderStatu item)
        {
            string username = User.Identity.Name;
            item.name = (item.name == null) ? "order_status" : item.name;

            dbStoredProcedure.orderStatusUpdate(item.order_status_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeeOrderStatus;
            return PartialView("_OrderStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderStatusGridViewPartialDelete(int order_status_id)
        {
            dbStoredProcedure.orderStatusDelete(order_status_id);
            db.SaveChanges();

            var model = db.TShopeeOrderStatus;
            return PartialView("_OrderStatusGridViewPartial", model.ToList());
        }
    }
}