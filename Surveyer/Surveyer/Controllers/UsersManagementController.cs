using Surveyer.HelperClasses;
using Surveyer.Models;
using Surveyer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveyer.Controllers
{
    public class UsersManagementController : Controller
    {
        private JsonIO jsonIO = new JsonIO();

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
        public ActionResult Register(User user,HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (jsonIO.Users.GetData(this).Where(x=>x.UserName==user.UserName).Count()!=0)
                    return View(user);

                if (Image != null)
                {
                    string path = Server.MapPath("~/Imeges/" + Image.FileName);
                    Image.SaveAs(path);
                    user.ImageURL = Image.FileName;
                }
                user.Password = user.Password.GetHashCode().ToString();
                jsonIO.Users.AddItem(this, user);
                return RedirectToAction("Index","Home");
            }
            return View(user);
        }
        [CustomAuthorize]
        public ActionResult MyNotefication()
        {
            ViewBag.name = ((User)Session["user"]).UserName;
            if (((User)Session["user"]).ImageURL == null)
                ViewBag.image = Url.Content("~/Imeges/defultuserx.png");
            else
                ViewBag.image = Url.Content($"~/Imeges/{((User)Session["user"]).ImageURL}");
            return View(jsonIO.Notefications.GetData(this).Where(x => x.UserId == ((User)Session["user"]).Id).OrderByDescending(x => x.Date).ToList());
        }
    }
}