using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class SupplierController : AdminController
    {
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult SupplierGridViewPartial()
        {
            var model = db.TShopeeSuppliers;
            return PartialView("_SupplierGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SupplierGridViewPartialAddNew(TShopeeSupplier item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "supplier_name" : item.name;
            item.code = (item.code == null) ? "ABC" : item.code;
            item.nation = (item.nation == null) ? "International" : item.nation;

            dbStoredProcedure.supplierInsert(item.name, item.code, item.nation, username);
            db.SaveChanges();

            var model = db.TShopeeSuppliers;
            return PartialView("_SupplierGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SupplierGridViewPartialUpdate(TShopeeSupplier item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "supplier_name" : item.name;
            item.code = (item.code == null) ? "ABC" : item.code;
            item.nation = (item.nation == null) ? "International" : item.nation;

            dbStoredProcedure.supplierUpdate(item.supplier_id, item.name, item.code, item.nation, username);
            db.SaveChanges();

            var model = db.TShopeeSuppliers;
            return PartialView("_SupplierGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SupplierGridViewPartialDelete(int supplier_id)
        {
            dbStoredProcedure.supplierDelete(supplier_id);
            db.SaveChanges();

            var model = db.TShopeeSuppliers;
            return PartialView("_SupplierGridViewPartial", model.ToList());
        }
    }
}