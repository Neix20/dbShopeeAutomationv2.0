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

            string product_sku = generalFunc.trimStr(Request.Form["Product Code"]);
            string order_item_status = generalFunc.trimStr(Request.Form["Order Item Status"]);

            // Create New Order Item Status
            dbStoredProcedure.orderItemStatusInsert(order_item_status, "", 0, username);
            db.SaveChanges(); 
            item.order_item_status_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeOrderItemStatus') AS INT)").FirstOrDefault(); ;

            item.product_id = db.TShopeeProducts.FirstOrDefault(it => it.SKU.ToLower().Equals(product_sku.ToLower())).product_id;

            dbStoredProcedure.orderItemInsert(item.quantity, item.sub_total, item.discount_fee, item.RMA_num, item.RMA_issued_by, item.RMA_issued_date, item.order_id, item.order_item_status_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeOrderItems;
            return PartialView("~/Views/OrderItem/_OrderItemGridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult OrderItemGridViewPartialUpdate(TShopeeOrderItem item)
        {
            string username = User.Identity.Name;

            string product_sku = generalFunc.trimStr(Request.Form["Product Code"]);
            string order_item_status = generalFunc.trimStr(Request.Form["Order Item Status"]);

            item.order_item_status_id = db.TShopeeOrderItemStatus.FirstOrDefault(it => it.name.ToLower().Equals(order_item_status.ToLower())).order_item_status_id;
            item.product_id = db.TShopeeProducts.FirstOrDefault(it => it.SKU.ToLower().Equals(product_sku.ToLower())).product_id;

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