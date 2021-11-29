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

            dbStoredProcedure.orderInsert(item.order_title, item.order_placed_date, item.total_price, item.order_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialUpdate(TShopeeOrder item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.orderUpdate(item.order_id, item.order_title, item.order_placed_date, item.total_price, item.order_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialDelete(int order_id)
        {
            dbStoredProcedure.orderDelete(order_id);
            db.SaveChanges();

            // Delete All Order Item
            db.TShopeeOrderItems.Where(it => it.order_id == order_id).ToList().ForEach(order_item => {
                int order_item_status_id = (int) db.TShopeeOrderItems.FirstOrDefault(tmp => tmp.order_item_id == order_item.order_item_id).order_item_status_id;
                dbStoredProcedure.orderItemStatusDelete(order_item_status_id);
                db.SaveChanges();

                dbStoredProcedure.orderItemDelete(order_item.order_item_id);
                db.SaveChanges();
            });

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }
    }
}