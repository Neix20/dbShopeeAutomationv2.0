using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class UserRoleController : Controller
    {
        // GET: UserRole
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        [ValidateInput(false)]
        public ActionResult UserRoleGridViewPartial()
        {
            var model = db.TShopeeUserRoles;
            return PartialView("_UserRoleGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserRoleGridViewPartialAddNew(TShopeeUserRole item)
        {
            var model = db.TShopeeUserRoles;
            return PartialView("_UserRoleGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserRoleGridViewPartialUpdate(TShopeeUserRole item)
        {
            var model = db.TShopeeUserRoles;
            return PartialView("_UserRoleGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserRoleGridViewPartialDelete(int user_role_id)
        {
            var model = db.TShopeeUserRoles;
            return PartialView("_UserRoleGridViewPartial", model.ToList());
        }
    }
}