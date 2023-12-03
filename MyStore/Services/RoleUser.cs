using MyStore.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.Services
{
    public class RoleUser : AuthorizeAttribute
    {
        // GET: RoleUser
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = SessionConfig.GetUser();
            if (user == null || user.Role != "admin")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new
                    {
                        controller = "AuthAdmin",
                        action = "Login",
                        area = "Admin",

                    }));
                return;
            }
            return;
        }
    }
}