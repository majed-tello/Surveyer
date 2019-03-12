using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveyer.Controllers
{
    public class UsersManagementController : Controller
    {
        // GET: UsersManagement
        public ActionResult Loggin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Loggin(string UserName,string Password)
        {
            return RedirectToAction("");
        }

        public ActionResult Loggoff()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string UserName, string Password)
        {
            return RedirectToAction("");
        }
    }
}