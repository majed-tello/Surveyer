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
        public ActionResult Loggin()
        {
           // ViewBag.raaed = new List<int> { 1, 2, 3, 4, 5 };
            return View();
        }
        [HttpPost]
        public ActionResult Loggin(LogginViewModel logginviewmodel)
        {
            if (ModelState.IsValid)
            {
                var user = jsonIO.Users.GetData(this).Where(x => x.UserName == logginviewmodel.UserName && x.Password == logginviewmodel.Password).FirstOrDefault();
                if (user!=null)
                {
                    Session["user"] = user;
                    return RedirectToAction("Index","Home");
                }
            }
            return View(logginviewmodel);
        }

        public ActionResult Loggoff()
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
                jsonIO.Users.AddItem(this, user);
                return RedirectToAction("Index","Home");
            }
            return View(user);
        }
    }
}