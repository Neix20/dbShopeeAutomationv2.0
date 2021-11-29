using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class OrderItemController : AdminController
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
            string username = User.Identity.Name;

            // Create New Order Item Status
            dbStoredProcedure.orderItemStatusInsert("", 0, username);
            db.SaveChanges();
            item.order_item_status_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeOrderItemStatus') AS INT)").FirstOrDefault(); ;

            dbStoredProcedure.orderItemInsert(item.quantity, item.sub_total, item.discount_fee, item.RMA_num, item.RMA_issued_by, item.RMA_issued_date, item.order_id, item.order_item_status_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItems;
            return PartialView("~/Views/OrderItem/_OrderItemGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialUpdate(TShopeeOrderItem item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.orderItemUpdate(item.order_item_id, item.quantity, item.sub_total, item.discount_fee, item.RMA_num, item.RMA_issued_by, item.RMA_issued_date, item.order_id, item.order_item_status_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItems;
            return PartialView("~/Views/OrderItem/_OrderItemGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialDelete(int order_item_id)
        {
            int order_item_status_id = (int)db.TShopeeOrderItems.FirstOrDefault(it => it.order_item_id == order_item_id).order_item_status_id;
            dbStoredProcedure.orderItemStatusDelete(order_item_status_id);
            db.SaveChanges();

            dbStoredProcedure.orderItemDelete(order_item_id);
            db.SaveChanges();

            var model = db.TShopeeOrderItems;
            return PartialView("~/Views/OrderItem/_OrderItemGridViewPartial.cshtml", model.ToList());
        }

    }
}