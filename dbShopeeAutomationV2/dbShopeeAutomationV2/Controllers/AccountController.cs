using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace dbShopeeAutomationV2.Controllers
{
    public class AccountController : Controller
    {
        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Index", "DailyTask");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TShopeeUser model)
        {
            string returnUrl = Request.QueryString["returnUrl"];

            var dataItem = db.TShopeeUsers.FirstOrDefault(it => it.username.Equals(model.username) && it.password.Equals(model.password));
            if(dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(model.username, false);

                string[] role_arr = Roles.GetRolesForUser(model.username);

                if (!role_arr.Contains("Admin".ToLower()))
                {
                    return Content($"Error 403: You do not have authority to access this webpage");
                }

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Content(returnUrl);
                }
                else
                {
                    return Content("/DailyTask/Index");
                }
            }

            return Content($"Error 400: Invalid Username or Password");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public bool CheckUsernameExist(TShopeeUser model)
        {
            return db.TShopeeUsers.Select(x => x.username).Contains(model.username);
        }

        [HttpPost]
        public ActionResult ForgotPassword(TShopeeUser model)
        {
            var dataItem = db.TShopeeUsers.FirstOrDefault(it => it.username.Equals(model.username));
            dataItem.password = model.password;

            dbStoredProcedure.userUpdate(dataItem.user_id, dataItem.username, dataItem.password, dataItem.email);
            db.SaveChanges();

            return Content("/DailyTask/Index");
        }

        [Authorize]
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}