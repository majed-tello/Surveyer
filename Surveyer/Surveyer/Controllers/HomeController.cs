using Surveyer.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveyer.Controllers
{
    public class HomeController : Controller
    {
        private JsonIO jsonIO = new JsonIO();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}