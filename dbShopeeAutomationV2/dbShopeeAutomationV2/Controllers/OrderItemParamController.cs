using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class OrderItemParamController : AdminController
    {
        // GET: OrderItemParam
        public ActionResult Index(int? order_id)
        {
            order_id = (order_id == null) ? -1 : order_id;
            ViewData["order_id"] = order_id;
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult OrderItemParamGridViewPartial(int? order_id)
        {
            order_id = (order_id == null) ? -1 : order_id;
            var model = db.TShopeeOrderItems.Where(it => it.order_id == order_id);
            return PartialView("_OrderItemParamGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemParamGridViewPartialAddNew(TShopeeOrderItem item)
        {
            string username = User.Identity.Name;

            string order_title = generalFunc.trimStr(Request.Form["order_title"]);
            item.order_id = dbStatusFunction.orderID(order_title);

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
            dbStoredProcedure.orderItemStatusInsert($"Order Title: {order.order_title}, Product Item: {product.SKU}", "Order Item Description", 0, username);
            db.SaveChanges();

            item.order_item_status_id = dbStoredProcedure.getID("TShopeeOrderItemStatus");

            dbStoredProcedure.orderItemInsert(
                item.quantity,
                item.sub_total, item.discount_fee,
                item.RMA_num, item.RMA_issued_by, item.RMA_issued_date,
                item.order_id, item.order_item_status_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItems.Where(it => it.order_id == item.order_id);
            return PartialView("_OrderItemParamGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemParamGridViewPartialUpdate(TShopeeOrderItem item)
        {
            string username = User.Identity.Name;

            item.quantity = (item.quantity == null) ? 0 : item.quantity;
            item.sub_total = (item.sub_total == null) ? 0 : item.sub_total;
            item.discount_fee = (item.discount_fee == null) ? 0 : item.discount_fee;
            item.RMA_num = (item.RMA_num == null) ? 0 : item.RMA_num;
            item.RMA_issued_by = (item.RMA_issued_by == null) ? "rma_issued_by" : item.RMA_issued_by;
            item.RMA_issued_date = (item.RMA_issued_date == null) ? DateTime.Now : item.RMA_issued_date;

            item.order_item_status_id = (int)db.TShopeeOrderItems.FirstOrDefault(it => it.order_item_id == item.order_item_id).order_item_status_id;

            var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == item.product_id);
            item.sub_total = product.sell_price * item.quantity - item.discount_fee;

            dbStoredProcedure.orderItemUpdate(
                item.order_item_id, item.quantity,
                item.sub_total, item.discount_fee,
                item.RMA_num, item.RMA_issued_by, item.RMA_issued_date,
                item.order_id, item.order_item_status_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItems.Where(it => it.order_id == item.order_id);
            return PartialView("_OrderItemParamGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemParamGridViewPartialDelete(int order_item_id)
        {
            var item = db.TShopeeOrderItems.FirstOrDefault(it => it.order_item_id == order_item_id);

            int order_item_status_id = (int)item.order_item_status_id;

            dbStoredProcedure.orderItemStatusDelete(order_item_status_id);
            db.SaveChanges();

            dbStoredProcedure.orderItemDelete(order_item_id);
            db.SaveChanges();

            var model = db.TShopeeOrderItems.Where(it => it.order_id == item.order_id);
            return PartialView("_OrderItemParamGridViewPartial", model.ToList());
        }

        [HttpPost]
        public ActionResult CompleteOrder()
        {
            string username = User.Identity.Name;

            string order_id_str = generalFunc.trimStr(Request.Form["order_id"]);
            int order_id = int.Parse(order_id_str);

            // Update Order Status to Complete
            var order = db.TShopeeOrders.FirstOrDefault(it => it.order_id == order_id);
            order.order_status_id = dbStatusFunction.orderStatusID("Complete");

            // Get List of Order Item
            List<TShopeeOrderItem> orderItemList = db.TShopeeOrderItems.Where(it => it.order_id == order_id).ToList();

            orderItemList.ForEach(it =>
            {
                order.total_price += it.sub_total;
            });


            db.SaveChanges();

            return RedirectToAction("Index", "Order");
        }
    }
}