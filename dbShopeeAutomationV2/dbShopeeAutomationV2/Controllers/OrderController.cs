using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class OrderController : AdminController
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult OrderGridViewPartial()
        {
            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialAddNew(TShopeeOrder item)
        {
            string username = User.Identity.Name;

            int last_order_id = db.TShopeeOrders.ToList().Count + 1;
            item.order_title = (item.order_title == null) ? generalFunc.GenOrderCode(last_order_id) : item.order_title;

            item.order_placed_date = (item.order_placed_date == null) ? DateTime.Now : item.order_placed_date;
            item.total_price = 0;
            item.order_status_id = dbStatusFunction.orderStatusID("Incomplete");

            dbStoredProcedure.orderInsert(item.order_title, item.order_placed_date, item.total_price, item.order_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialUpdate(TShopeeOrder item)
        {
            string username = User.Identity.Name;

            int last_order_id = dbStoredProcedure.getID("TShopeeOrder");
            item.order_title = (item.order_title == null) ? generalFunc.GenOrderCode(last_order_id) : item.order_title;

            item.order_placed_date = (item.order_placed_date == null) ? DateTime.Now : item.order_placed_date;

            dbStoredProcedure.orderUpdate(item.order_id, item.order_title, item.order_placed_date, item.total_price, item.order_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialDelete(int order_id)
        {
            // Get Order Status
            var order = db.TShopeeOrders.FirstOrDefault(it => it.order_id == order_id);
            int ord_sta_id = (int) order.order_status_id;
            int c_ord_sta_id = dbStatusFunction.orderStatusID("complete");

            // Delete List of Order Items
            var orderItemList = db.TShopeeOrderItems.Where(it => it.order_id == order_id).ToList();
            orderItemList.ForEach(tmp_model =>
            {
                if(ord_sta_id == c_ord_sta_id)
                {
                    int product_id = (int) tmp_model.product_id;
                    var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id);
                    var stockItem = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == product_id);
                    stockItem.stock_quantity -= tmp_model.quantity;
                }

                // Delete Corresponding Order item Status
                int order_item_status_id = (int)tmp_model.order_item_status_id;
                dbStoredProcedure.orderItemStatusDelete(order_item_status_id);

                // Delete Order Item
                dbStoredProcedure.orderItemDelete(tmp_model.order_item_id);
            });

            dbStoredProcedure.orderDelete(order_id);
            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
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
                var stockItem = db.TShopeeStockItems.FirstOrDefault(si => si.product_id == it.product_id);
                stockItem.stock_quantity -= it.quantity;

                order.total_price += it.sub_total;
            });

            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }
    }
}