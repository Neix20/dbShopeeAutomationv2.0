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

            item.quantity = (item.quantity == null) ? 0 : item.quantity;
            item.sub_total = (item.sub_total == null) ? 0 : item.sub_total;
            item.discount_fee = (item.discount_fee == null) ? 0 : item.discount_fee;
            item.RMA_num = (item.RMA_num == null) ? 0 : item.RMA_num;
            item.RMA_issued_by = (item.RMA_issued_by == null) ? "rma_issued_by" : item.RMA_issued_by;
            item.RMA_issued_date = (item.RMA_issued_date == null) ? DateTime.Now : item.RMA_issued_date;

            // Update Sub Total
            var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == item.product_id);
            item.sub_total = product.sell_price * item.quantity - item.discount_fee;

            var order = db.TShopeeOrders.FirstOrDefault(it => it.order_id == item.order_id);

            // Create New Order Item Status
            dbStoredProcedure.orderItemStatusInsert($"Order Title: {order.order_title}, Product Item: {product.SKU}", "", 0, username);
            db.SaveChanges();

            item.order_item_status_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeOrderItemStatus') AS INT)").FirstOrDefault(); ;

            dbStoredProcedure.orderItemInsert(item.quantity, item.sub_total, item.discount_fee, item.RMA_num, item.RMA_issued_by, item.RMA_issued_date, item.order_id, item.order_item_status_id, item.product_id, username);
            db.SaveChanges();

            // Update order Total
            var orderItems = db.TShopeeOrderItems.Where(it => it.order_id == item.order_id).ToList();
            order.total_price = (decimal)orderItems.Select(x => x.sub_total).Sum();

            dbStoredProcedure.orderUpdate(order.order_id, order.order_title, order.order_placed_date, order.total_price, order.order_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItems;
            return PartialView("_OrderItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialUpdate(TShopeeOrderItem item)
        {
            string username = User.Identity.Name;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;
            item.sub_total = (item.sub_total == null) ? 0 : item.sub_total;
            item.discount_fee = (item.discount_fee == null) ? 0 : item.discount_fee;
            item.RMA_num = (item.RMA_num == null) ? 0 : item.RMA_num;
            item.RMA_issued_date = (item.RMA_issued_date == null) ? DateTime.Now : item.RMA_issued_date;
            item.order_item_status_id = (int)db.TShopeeOrderItems.FirstOrDefault(it => it.order_item_id == item.order_item_id).order_item_status_id;

            // Update Sub Total
            var order = db.TShopeeOrders.FirstOrDefault(it => it.order_id == item.order_id);
            order.total_price -= item.sub_total;

            var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == item.product_id);
            item.sub_total = product.sell_price * item.quantity - item.discount_fee;

            dbStoredProcedure.orderItemUpdate(item.order_item_id, item.quantity, item.sub_total, item.discount_fee, item.RMA_num, item.RMA_issued_by, item.RMA_issued_date, item.order_id, item.order_item_status_id, item.product_id, username);
            db.SaveChanges();

            // Update order Total
            order.total_price += item.sub_total;
            dbStoredProcedure.orderUpdate(order.order_id, order.order_title, order.order_placed_date, order.total_price, order.order_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItems;
            return PartialView("_OrderItemGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialDelete(int order_item_id)
        {
            string username = User.Identity.Name;
            var item = db.TShopeeOrderItems.FirstOrDefault(it => it.order_item_id == order_item_id);

            var order = db.TShopeeOrders.FirstOrDefault(it => it.order_id == item.order_id);

            int order_item_status_id = (int)db.TShopeeOrderItems.FirstOrDefault(it => it.order_item_id == order_item_id).order_item_status_id;

            dbStoredProcedure.orderItemStatusDelete(order_item_status_id);
            db.SaveChanges();

            dbStoredProcedure.orderItemDelete(order_item_id);
            db.SaveChanges();

            // Update order Total
            var orderItems = db.TShopeeOrderItems.Where(it => it.order_id == item.order_id).ToList();
            order.total_price = (decimal)orderItems.Select(x => x.sub_total).Sum();

            dbStoredProcedure.orderUpdate(order.order_id, order.order_title, order.order_placed_date, order.total_price, order.order_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItems;
            return PartialView("_OrderItemGridViewPartial", model.ToList());
        }

    }
}