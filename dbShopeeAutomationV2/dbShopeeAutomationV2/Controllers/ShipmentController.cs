using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ShipmentController : Controller
    {
        // GET: Shipment
        public ActionResult Index()
        {
            return View();
        }

        public static string randomCode(int length = 10)
        {
            Random rand = new Random();
            string str = "";
            for (int i = 0; i < length; i++) str += $"{rand.Next(0, 10) + 1}";
            return str;
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ShipmentGridViewPartial()
        {
            var model = db.TShopeeShipments;
            return PartialView("_ShipmentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentGridViewPartialAddNew(TShopeeShipment item)
        {
            string username = User.Identity.Name;

            item.tracking_id = (item.tracking_id == null) ? randomCode() : item.tracking_id;
            item.created_date = (item.created_date == null) ? DateTime.Now : item.created_date;
            item.expected_date = (item.expected_date == null) ? DateTime.Now : item.expected_date;
            item.due_date = (item.due_date == null) ? DateTime.Now : item.due_date;

            dbStoredProcedure.shipmentInsert(item.start_location, item.destination, item.tracking_id, item.created_date, item.expected_date, item.due_date, item.invoice_id, item.carrier_id, item.shipment_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeShipments;
            return PartialView("_ShipmentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentGridViewPartialUpdate(TShopeeShipment item)
        {
            string username = User.Identity.Name;

            item.tracking_id = (item.tracking_id == null) ? randomCode() : item.tracking_id;
            item.created_date = (item.created_date == null) ? DateTime.Now : item.created_date;
            item.expected_date = (item.expected_date == null) ? DateTime.Now : item.expected_date;
            item.due_date = (item.due_date == null) ? DateTime.Now : item.due_date;

            dbStoredProcedure.shipmentUpdate(item.shipment_id, item.start_location, item.destination, item.tracking_id, item.created_date, item.expected_date, item.due_date, item.invoice_id, item.carrier_id, item.shipment_status_id, username);
            db.SaveChanges();

            var model = db.TShopeeShipments;
            return PartialView("_ShipmentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentGridViewPartialDelete(int shipment_id)
        {
            dbStoredProcedure.shipmentDelete(shipment_id);
            db.SaveChanges();

            var model = db.TShopeeShipments;
            return PartialView("_ShipmentGridViewPartial", model.ToList());
        }
    }
}