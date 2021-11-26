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

            dbStoredProcedure.orderInsert(item.order_placed_date, item.total_price, item.order_status, username);
            db.SaveChanges();

            item.order_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeOrder') AS INT)").FirstOrDefault();

            dbStoredProcedure.orderUpdate(item.order_id, item.order_placed_date, item.total_price, item.order_status, username);
            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialUpdate(TShopeeOrder item)
        {
            string username = User.Identity.Name;

            dbStoredProcedure.orderUpdate(item.order_id, item.order_placed_date, item.total_price, item.order_status, username);
            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderGridViewPartialDelete(int order_id)
        {
            dbStoredProcedure.orderDelete(order_id);
            db.SaveChanges();

            var model = db.TShopeeOrders;
            return PartialView("_OrderGridViewPartial", model.ToList());
        }
    }
}