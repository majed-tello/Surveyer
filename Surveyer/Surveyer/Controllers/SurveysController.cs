using Surveyer.HelperClasses;
using Surveyer.Models;
using Surveyer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var surveyallowaccess = new List<SelectListViewModel>{ new SelectListViewModel { Id = 0, Text = "Aanonymous" },
                new SelectListViewModel { Id = 1, Text = "Specific Users" } };

            var surveytype = new List<SelectListViewModel>{ new SelectListViewModel { Id = 0, Text = "FeedBack" },
                new SelectListViewModel { Id = 1, Text = "Quiz" } };


            ViewBag.AllowAccess = new SelectList(surveyallowaccess, "Id", "Text");
            ViewBag.SurveyType = new SelectList(surveytype, "Id", "Text");
            ViewBag.Users = new SelectList(jsonIO.Users.GetData(this), "Id", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Survey survey,FormCollection form)
        {
            if (ModelState.IsValid)
            {
                if (survey.AllowAccess == (int)SurveyAllowAccess.SpecificUsers)
                {
                    
                    var users = form["Users"];
                    var userarr = users.Split(',');
                    survey.UsersAllowedAccess = new List<string>();
                    foreach (var user in userarr)
                        survey.UsersAllowedAccess.Add(user);
                }
                survey.UserId = ((User)Session["user"]).Id;
                Session["CurrentSurvey"] = survey;
                return RedirectToAction("CreateCompleate", "Surveys");
            }
            return View(survey);
        }

        public ActionResult CreateCompleate()
        {
            Survey CurrentSurvey = (Survey)Session["CurrentSurvey"];
            ViewBag.t = CurrentSurvey.Title;
            ViewBag.description = CurrentSurvey.Description;
            ViewBag.color = CurrentSurvey.SurveyColor.Name;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCompleate(FormCollection form)
        {
            Survey CurrentSurvey = (Survey)Session["CurrentSurvey"];
            for (int i = 0; i < 100; i++)
            {
                var type = form[i.ToString()];
                if (type == null)
                    continue;
                else
                {
                    if (type == "Short Answer" || type == "Pargrph" || type == "Rating" || type == "File Upload" || type == "Date" || type == "Time")
                    {
                        var text = form["t" + i].ToString();
                        var r = (form["r" + i]);
                        bool required =(r==null)? false:true;
                        CurrentSurvey.SurveyItems.Add(GetItem(type, text, required, new List<Choice> { }));
                    }
                    else
                    {
                        List<Choice> answers = new List<Choice>();
                        for (int j = 0; j < 100; j++)
                        {
                            var answer = form["b" + i + "-a" + j];
                            if (answer != null)
                                answers.Add(new Choice {Text= answer,IsCorect=false });
                        }
                        var text = form["t" + i].ToString();
                        var r = (form["r" + i]);
                        bool required = (r == null) ? false : true;
                        CurrentSurvey.SurveyItems.Add(GetItem(type, text, required, answers));
                    }
                }
            }
            jsonIO.Surveys.AddItem(this, CurrentSurvey);
            Session["CurrentSurvey"] = null;
            return RedirectToAction("Index", "Home");
        }

        private SurveyItem GetItem(string type, string text, bool required, List<Choice> choices)
        {
            switch(type)
            {
                case "Short Answer":return new SurveyItem((int)SurveyItemType.ShortAnswer,text, required, choices);
                case "Pargrph": return new SurveyItem((int)SurveyItemType.Pargrph, text, required, choices);
                case "Single Choice": return new SurveyItem((int)SurveyItemType.SingleChoice, text, required, choices);
                case "Multiple Choice": return new SurveyItem((int)SurveyItemType.MultipleChoice, text, required, choices);
                case "Rating": return new SurveyItem((int)SurveyItemType.Rating, text, required, choices);
                case "File Upload": return new SurveyItem((int)SurveyItemType.FileUpload, text, required, choices);
                case "Date": return new SurveyItem((int)SurveyItemType.Date, text, required, choices);
                case "Time": return new SurveyItem((int)SurveyItemType.Time, text, required, choices);
            }
            return new SurveyItem((int)SurveyItemType.ShortAnswer, text, required, choices);
        }


        public ActionResult FillSurvey(string SurveyId)
        {
            Survey survey = jsonIO.Surveys.GetData(this).Where(x => x.Id == SurveyId).FirstOrDefault();
            User user = (User)Session["user"];
            if (survey == null)
                return new HttpNotFoundResult("Survey Not Found .......");
            if (survey.AllowAccess==(int)SurveyAllowAccess.SpecificUsers)
            {
                if (!survey.UsersAllowedAccess.Contains(user.Id))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "You are not allowed to fill this survey .......");
            }
            return View(survey);
        }
        [HttpPost]
        public ActionResult FillSurvey(FormCollection form)
        {
            return View();
        }


    }
}