using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class ShipmentStatusController : Controller
    {
        // GET: ShipmentStatus
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ShipmentStatusGridViewPartial()
        {
            var model = db.TShopeeShipmentStatus;
            return PartialView("_ShipmentStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentStatusGridViewPartialAddNew(TShopeeShipmentStatu item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "shipment_status" : item.name;

            dbStoredProcedure.shipmentStatusInsert(item.name, username);
            db.SaveChanges();

            var model = db.TShopeeShipmentStatus;
            return PartialView("_ShipmentStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentStatusGridViewPartialUpdate(TShopeeShipmentStatu item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "shipment_status" : item.name;

            dbStoredProcedure.shipmentStatusUpdate(item.shipment_status_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeeShipmentStatus;
            return PartialView("_ShipmentStatusGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentStatusGridViewPartialDelete(int shipment_status_id)
        {
            dbStoredProcedure.shipmentStatusDelete(shipment_status_id);
            db.SaveChanges();

            var model = db.TShopeeShipmentStatus;
            return PartialView("_ShipmentStatusGridViewPartial", model.ToList());
        }
    }
}