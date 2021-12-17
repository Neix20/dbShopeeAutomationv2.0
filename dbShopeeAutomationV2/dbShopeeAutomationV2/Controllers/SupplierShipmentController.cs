using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class SupplierShipmentController : Controller
    {
        // GET: SupplierShipment
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult SupplierShipmentGridViewPartial()
        {
            var model = db.TShopeeSupplierShipments;
            return PartialView("_SupplierShipmentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SupplierShipmentGridViewPartialAddNew(TShopeeSupplierShipment item)
        {
            string username = User.Identity.Name;

            item.received_date = (item.received_date == null) ? DateTime.Now : item.received_date;
            item.supplier_tracking_id = (item.supplier_tracking_id == null) ? generalFunc.Random10DigitCode() : item.supplier_tracking_id;
            item.NTL_tracking_id = (item.NTL_tracking_id == null) ? generalFunc.Random10DigitCode() : item.NTL_tracking_id;

            item.height = (item.height == null) ? 0 : item.height;
            item.width = (item.width == null) ? 0 : item.width;
            item.length = (item.length == null) ? 0 : item.length;

            dbStoredProcedure.supplierShipmentInsert(
                item.received_date, item.supplier_tracking_id, item.NTL_tracking_id, 
                item.height, item.width, item.length, 
                item.supplier_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeSupplierShipments;
            return PartialView("_SupplierShipmentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SupplierShipmentGridViewPartialUpdate(TShopeeSupplierShipment item)
        {
            string username = User.Identity.Name;

            item.received_date = (item.received_date == null) ? DateTime.Now : item.received_date;
            item.supplier_tracking_id = (item.supplier_tracking_id == null) ? generalFunc.Random10DigitCode() : item.supplier_tracking_id;
            item.NTL_tracking_id = (item.NTL_tracking_id == null) ? generalFunc.Random10DigitCode() : item.NTL_tracking_id;

            item.height = (item.height == null) ? 0 : item.height;
            item.width = (item.width == null) ? 0 : item.width;
            item.length = (item.length == null) ? 0 : item.length;

            dbStoredProcedure.supplierShipmentUpdate(
                item.supplier_shipment_id, item.received_date, item.supplier_tracking_id, item.NTL_tracking_id,
                item.height, item.width, item.length,
                item.supplier_id, item.product_id, username);
            db.SaveChanges();

            var model = db.TShopeeSupplierShipments;
            return PartialView("_SupplierShipmentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SupplierShipmentGridViewPartialDelete(int supplier_shipment_id)
        {
            dbStoredProcedure.supplierShipmentDelete(supplier_shipment_id);
            db.SaveChanges();

            var model = db.TShopeeSupplierShipments;
            return PartialView("_SupplierShipmentGridViewPartial", model.ToList());
        }
    }
}