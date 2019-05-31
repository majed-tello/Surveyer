using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveyer.HelperClasses
{
    public class HelperClass
    {
       public static string GetUserName(Controller controller,string Id)
        {
            JsonIO jsonIO = new JsonIO();
            return jsonIO.Users.GetData(controller).Where(x => x.Id == Id).Select(x => x.UserName).FirstOrDefault();
        }
        public static string GetSurveyTitle(Controller controller, string Id)
        {
            JsonIO jsonIO = new JsonIO();
            return jsonIO.Surveys.GetData(controller).Where(x => x.Id == Id).Select(x => x.Title).FirstOrDefault();
        }
        public static string GetItemResultText(Controller controller, string SurveyId,string ItemResultId)
        {
            JsonIO jsonIO = new JsonIO();
            var a= jsonIO.Surveys.GetData(controller).Where(x => x.Id == SurveyId).FirstOrDefault();
            return a.SurveyItems.Where(x => x.Id == ItemResultId).Select(x=>x.Text).FirstOrDefault();
        }
    }
}