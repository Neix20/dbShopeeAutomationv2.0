using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class DetailController : AdminController
    {
        // GET: Detail
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2.Models.dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2.Models.dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult DetailGridViewPartial()
        {
            var model = db.TShopeeDetails;
            return PartialView("_DetailGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialAddNew(dbShopeeAutomationV2.Models.TShopeeDetail item)
        {
            var model = db.TShopeeDetails;
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
            return PartialView("_DetailGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialUpdate(dbShopeeAutomationV2.Models.TShopeeDetail item)
        {
            var model = db.TShopeeDetails;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.detail_id == item.detail_id);
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
            return PartialView("_DetailGridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DetailGridViewPartialDelete(System.Int32 detail_id)
        {
            var model = db.TShopeeDetails;
            if (detail_id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.detail_id == detail_id);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_DetailGridViewPartial", model.ToList());
        }
    }
}