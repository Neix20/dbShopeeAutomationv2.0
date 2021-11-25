using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class UserController : AdminController
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
            dbStoredProcedure.userInsert(item.username, item.password, item.email);
            db.SaveChanges();

            var model = db.TShopeeUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserGridViewPartialUpdate(TShopeeUser item)
        {
            dbStoredProcedure.userUpdate(item.user_id, item.username, item.password, item.email);
            db.SaveChanges();

            var model = db.TShopeeUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserGridViewPartialDelete(int user_id)
        {
            dbStoredProcedure.userDelete(user_id);
            db.SaveChanges();

            var model = db.TShopeeUsers;
            return PartialView("_UserGridViewPartial", model.ToList());
        }
    }
}