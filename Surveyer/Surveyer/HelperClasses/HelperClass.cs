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
    }
}