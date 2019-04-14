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
    public class SurveysController : Controller
    {
        private JsonIO jsonIO = new JsonIO();
        // GET: Surveys
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var surveyallowaccess = new List<SelectListViewModel>{ new SelectListViewModel { Id = 0, Text = "Public" },
                new SelectListViewModel { Id = 1, Text = "Private" } };

            var surveytype = new List<SelectListViewModel>{ new SelectListViewModel { Id = 0, Text = "FeedBack" },
                new SelectListViewModel { Id = 1, Text = "Quiz" } };


            ViewBag.SurveyAllowAccess = new SelectList(surveyallowaccess, "Id", "Text");
            ViewBag.SurveyType = new SelectList(surveytype, "Id", "Text");
            ViewBag.Users = new SelectList(jsonIO.Users.GetData(this), "Id", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Survey survey,FormCollection form)
        {
            if (ModelState.IsValid)
            {
                if (survey.AllowAccess == 1)
                {
                    var users = form["Users"];
                    var userarr = users.Split(',');
                    survey.UsersAllowedAccess = new List<string>();
                    foreach (var user in userarr)
                        survey.UsersAllowedAccess.Add(user);
                }
                Session["CurrentSurvey"] = survey;
                return RedirectToAction("CreateCompleate", "Surveys");
            }
            return View(survey);
        }

        public ActionResult CreateCompleate()
        {
            //ViewBag.Title = CurrentSurvey.Title;
            //ViewBag.Description = CurrentSurvey.Description;
            //ViewBag.Color = CurrentSurvey.SurveyColor;
            Survey CurrentSurvey = (Survey)Session["CurrentSurvey"];

            return View(CurrentSurvey);
        }

        [HttpPost]
        public ActionResult CreateCompleate(Survey survey)
        {
            if (ModelState.IsValid)
            {
                jsonIO.Surveys.AddItem(this, survey);
                return RedirectToAction("Index", "Home");
            }
            return View(survey);
        }

    }
}