using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult UserGridViewPartial()
        {
            var model = db.TShopeeUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserGridViewPartialAddNew(TShopeeUser item)
        {
            var model = db.TShopeeUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserGridViewPartialUpdate(TShopeeUser item)
        {
            var model = db.TShopeeUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserGridViewPartialDelete(int user_id)
        {
            var model = db.TShopeeUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }
    }
}