using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class SupplierShipmentController : AdminController
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
            int supplier_shipment_id = db.TShopeeSupplierShipments.ToList().Count + 1;

            item.received_date = (item.received_date == null) ? DateTime.Now : item.received_date;
            item.supplier_tracking_id = (item.supplier_tracking_id == null) ? generalFunc.Random10DigitCode() : item.supplier_tracking_id;
            item.NTL_tracking_id = (item.NTL_tracking_id == null) ? generalFunc.GenSupplierTrackingCode(supplier_shipment_id) : item.NTL_tracking_id;

            item.height = (item.height == null) ? 0 : item.height;
            item.width = (item.width == null) ? 0 : item.width;
            item.length = (item.length == null) ? 0 : item.length;

            // Update Stock Item Quantity
            var stockItem = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == item.product_id);
            stockItem.stock_quantity += item.width * item.length;

            dbStoredProcedure.supplierShipmentInsert(
                item.received_date, item.supplier_tracking_id, item.NTL_tracking_id, 
                item.height, item.width, item.length, 
                item.supplier_id, item.product_id, username);
            db.SaveChanges();

            var file = Request.Files["tracking_image"];

            supplier_shipment_id = dbStoredProcedure.getID("TShopeeSupplierShipment");

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/SupplierShipmentTrackingImages")}\\SupplierShipment_{supplier_shipment_id}.png";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/StockWarehouseImages"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);

                var b = (Bitmap)Bitmap.FromStream(file.InputStream);
                b.Save(file_path, ImageFormat.Png);
            }

            var model = db.TShopeeSupplierShipments;
            return PartialView("_SupplierShipmentGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SupplierShipmentGridViewPartialUpdate(TShopeeSupplierShipment item)
        {
            string username = User.Identity.Name;
            int supplier_shipment_id = db.TShopeeSupplierShipments.ToList().Count;

            item.received_date = (item.received_date == null) ? DateTime.Now : item.received_date;
            item.supplier_tracking_id = (item.supplier_tracking_id == null) ? generalFunc.Random10DigitCode() : item.supplier_tracking_id;
            item.NTL_tracking_id = (item.NTL_tracking_id == null) ? generalFunc.GenSupplierTrackingCode(supplier_shipment_id) : item.NTL_tracking_id;

            item.height = (item.height == null) ? 0 : item.height;
            item.width = (item.width == null) ? 0 : item.width;
            item.length = (item.length == null) ? 0 : item.length;

            // Update Stock Item Quantity
            var oriSupplierShipment = db.TShopeeSupplierShipments.FirstOrDefault(it => it.supplier_shipment_id == item.supplier_shipment_id);
            var stockItem = db.TShopeeStockItems.FirstOrDefault(it => it.product_id == item.product_id);

            stockItem.stock_quantity -= oriSupplierShipment.width * oriSupplierShipment.length;
            stockItem.stock_quantity += item.width * item.length;

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