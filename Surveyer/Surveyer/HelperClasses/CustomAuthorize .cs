using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Surveyer.HelperClasses
{
    public class CustomAuthorize : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (((Surveyer.Models.User)httpContext.Session["user"]) != null)
                return true;
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (((Surveyer.Models.User)filterContext.HttpContext.Session["user"]) == null)
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                                       { "action", "Login" },
                                       { "controller", "UsersManagement" }
                                   });

          
        }



    }
}