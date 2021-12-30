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

            int last_order_id = dbStoredProcedure.getID("TShopeeOrder") + 1;
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

            return RedirectToAction("Index", "OrderItemParam", new { order_id = item.order_id });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialDelete(int order_id)
        {
            dbStoredProcedure.orderDelete(order_id);
            db.SaveChanges();

            // Delete List of Order Items
            var orderItemList = db.TShopeeOrderItems.Where(it => it.order_id == order_id).ToList();
            orderItemList.ForEach(it =>
            {
                int order_item_status_id = (int) it.order_item_status_id;

                // Delete Corresponding Order item Status
                dbStoredProcedure.orderItemStatusDelete(order_item_status_id);

                // Delete Order Item
                dbStoredProcedure.orderItemDelete(it.order_item_id);
            });

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }
    }
}