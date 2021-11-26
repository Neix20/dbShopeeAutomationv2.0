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

        dbShopeeAutomationV2.Models.dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2.Models.dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult ShipmentGridViewPartial()
        {
            var model = db.TShopeeShipments;
            return PartialView("_ShipmentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentGridViewPartialAddNew(dbShopeeAutomationV2.Models.TShopeeShipment item)
        {
            var model = db.TShopeeShipments;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ShipmentGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentGridViewPartialUpdate(dbShopeeAutomationV2.Models.TShopeeShipment item)
        {
            var model = db.TShopeeShipments;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.shipment_id == item.shipment_id);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_ShipmentGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult ShipmentGridViewPartialDelete(System.Int32 shipment_id)
        {
            var model = db.TShopeeShipments;
            if (shipment_id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.shipment_id == shipment_id);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_ShipmentGridViewPartial", model.ToList());
        }
    }
}