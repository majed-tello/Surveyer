using Surveyer.HelperClasses;
using Surveyer.Models;
using Surveyer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveyer.Controllers
{
    public class UsersManagementController : Controller
    {
        private JsonIO jsonIO = new JsonIO();

        public ActionResult Index()
        {
            return View(jsonIO.Users.GetData(this));
        }
        // GET: UsersManagement
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginviewmodel)
        {
            if (ModelState.IsValid)
            {
                var user = jsonIO.Users.GetData(this).Where(x => x.UserName == loginviewmodel.UserName && x.Password == loginviewmodel.Password.GetHashCode().ToString()).FirstOrDefault();
                if (user!=null)
                {
                    Session["user"] = user;
                    return RedirectToAction("Index","Home");
                }
            }
            return View(loginviewmodel);
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = user.Password.GetHashCode().ToString();
                jsonIO.Users.AddItem(this, user);
                return RedirectToAction("Index","Home");
            }
            return View(user);
        }
    }
}