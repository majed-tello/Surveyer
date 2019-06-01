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
                if (survey.SurveyType==(int)SurveyType.FeedBack)
                    return RedirectToAction("CreateCompleate", "Surveys");
                return RedirectToAction("CreateQuiz", "Surveys");
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
            if (CurrentSurvey.AllowAccess==(int)SurveyAllowAccess.SpecificUsers)
            {
                foreach (var user in CurrentSurvey.UsersAllowedAccess)
                    jsonIO.Notefications.AddItem(this, new Notefication { UserId = user, Content = "You have been invited to fill out "+ CurrentSurvey.Title+ " survey by " + HelperClass.GetUserName(this,CurrentSurvey.UserId), Link = "http://localhost:49825/Surveys/FillSurvey?SurveyId="+ CurrentSurvey.Id });
            }
            Session["CurrentSurvey"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateQuiz()
        {
            Survey CurrentSurvey = (Survey)Session["CurrentSurvey"];
            ViewBag.t = CurrentSurvey.Title;
            ViewBag.description = CurrentSurvey.Description;
            ViewBag.color = CurrentSurvey.SurveyColor.Name;
            ViewBag.time = CurrentSurvey.SurveyTime;
            return View();
        }
        [HttpPost]
        public ActionResult CreateQuiz(FormCollection form)
        {
            return View();
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
            Session["fillsurvey"] = survey;
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
            Survey survey = (Survey)Session["fillsurvey"];
            SurveyResult surveyresult = new SurveyResult();
            surveyresult.SurveyId = survey.Id;
            var user = ((User)Session["user"]);

            surveyresult.UserId = (user == null) ? "" : user.Id;
            foreach (var item in survey.SurveyItems)
            {
                surveyresult.surveyItemResults.Add(new SurveyItemResult { Id = item.Id, Type = item.Type, Value = form[item.Id] });
            }
            jsonIO.SurveyResults.AddItem(this,surveyresult);
            jsonIO.Notefications.AddItem(this, new Notefication { UserId = survey.UserId, Content = (surveyresult.UserId != "") ? "The "+HelperClass.GetSurveyTitle(this,survey.Id)+" survey was filled out by "+ HelperClass.GetUserName(this, surveyresult.UserId) : "The " + HelperClass.GetSurveyTitle(this, survey.Id) + " survey was filled out by an unknown person" ,Link= "http://localhost:49825/Surveys/Statics/"+ survey.Id });
            Session["fillsurvey"] = null;
            return RedirectToAction("Index","Home");
        }


        public ActionResult MySurveys()
        {
            var surveys = jsonIO.Surveys.GetData(this).Where(x => x.UserId == ((User)Session["user"]).Id).OrderByDescending(x=>x.PublishDate).ToList();
            return View(surveys);
        }

        public ActionResult Detailes(string Id)
        {
            var survey = jsonIO.Surveys.GetData(this).Where(x => x.Id == Id).FirstOrDefault();
            if (survey.AllowAccess==(int)SurveyAllowAccess.SpecificUsers)
            {
                List<string> Users = new List<string>();
                foreach (var userid in survey.UsersAllowedAccess)
                    Users.Add(HelperClass.GetUserName(this, userid));
                ViewBag.Users = Users;
            }
            return View(jsonIO.Surveys.GetData(this).Where(x => x.Id == Id).FirstOrDefault());
        }

        public ActionResult Statics(string Id)
        {
            var surveyresults = jsonIO.SurveyResults.GetData(this).Where(x => x.SurveyId == Id).OrderByDescending(x => x.PublishDate).ToList();
            foreach(var item in surveyresults)
                item.UserId=(item.UserId=="")? "unknown person" : HelperClass.GetUserName(this,item.UserId);
            foreach (var item in surveyresults)
                foreach (var resul in item.surveyItemResults)
                    resul.Id = HelperClass.GetItemResultText(this, item.SurveyId, resul.Id);
            ViewBag.Items = jsonIO.Surveys.GetData(this).Where(x=>x.Id==Id).FirstOrDefault();
            ViewBag.survId = Id;
            return View(surveyresults);
        }
        public ActionResult ShareLink(string Id)
        {
            ViewBag.ShareLink = "http://localhost:49825/Surveys/FillSurvey?SurveyId=" + Id;
            return View();
        }

        public ActionResult GetData(string SurveyId,string SurveyItemId)
        {
            var surveyitems = jsonIO.Surveys.GetData(this).Where(x => x.Id == SurveyId).FirstOrDefault();
            var item = surveyitems.SurveyItems.Where(x => x.Id == SurveyItemId).FirstOrDefault();
            List<StaticsViewModel> staticsViewModels = new List<StaticsViewModel>();
            if (item.Type == (int)SurveyItemType.SingleChoice || item.Type == (int)SurveyItemType.MultipleChoice) {
                foreach (var x in item.Answers)
                    staticsViewModels.Add(new StaticsViewModel { Item = x.Text, Count = 0 ,ItemName=item.Text});
                for (int i = 0; i <= 5- staticsViewModels.Count; i++)
                {
                    staticsViewModels.Add(new StaticsViewModel { Item = "", Count = 0, ItemName = item.Text });
                }
                        }
            else if (item.Type == (int)SurveyItemType.Rating)
            {
                for (int i = 1; i <=5; i++)
                {
                    staticsViewModels.Add(new StaticsViewModel { Item = i.ToString(), Count = 0, ItemName = item.Text });
                }
            }

            var surveyResult = jsonIO.SurveyResults.GetData(this).Where(x => x.SurveyId == SurveyId).ToList();
            foreach (var result in surveyResult)
            {
                if (result.surveyItemResults.Where(xx => xx.Id == SurveyItemId).Select(x => x.Type).FirstOrDefault() == (int)SurveyItemType.MultipleChoice)
                {
                    var multiresults = (string)result.surveyItemResults.Where(xx => xx.Id == SurveyItemId).Select(x => x.Value).FirstOrDefault();
                    if (multiresults == null) continue;
                    var arrresults = multiresults.Split(',');
                    foreach (var res in arrresults)
                        staticsViewModels.Where(x => x.Item == res).FirstOrDefault().Count++;
                }
                else { var res = result.surveyItemResults.Where(xx => xx.Id == SurveyItemId).Select(xx => xx.Value).FirstOrDefault();
                    if (res == null) continue;
                    staticsViewModels.Where(x => x.Item == res.ToString()).FirstOrDefault().Count++; }
            }

            return Json(staticsViewModels, JsonRequestBehavior.AllowGet);
        }


    }
}