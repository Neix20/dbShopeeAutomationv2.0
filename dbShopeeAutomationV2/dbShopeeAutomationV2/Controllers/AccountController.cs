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
            return RedirectToAction("Index", "Production");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TShopeeUser model, string returnUrl = "")
        {
            var dataItem = db.TShopeeUsers.FirstOrDefault(it => it.username.Equals(model.username) && it.password.Equals(model.password));
            if (dataItem != null)
            {
                // Issue 1: Have to login twice to check admin access priviledge
                // Hypothesis: Need to await for FormsAuthentication, before continuing to roles.IsUserInRole
                // Also, if it works, don't touch it.
                FormsAuthentication.SetAuthCookie(model.username, false);
                if (!Roles.IsUserInRole("Admin"))
                {
                    ViewData["login_error"] = "Error 403: You do not have authority to access this webpage";
                    return View();
                }

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            ViewData["login_error"] = "Error 400: Invalid Username or Password";
            return View();
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