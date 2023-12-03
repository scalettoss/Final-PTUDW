using MyStore.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.Controllers
{
    public class CustomerProfileController : Controller
    {
        // GET: CustomerProfile
        public ActionResult Index()
        {
            var user = SessionConfig.GetUser();
            if (user != null)
            {
                string userName = user != null ? user.Username : string.Empty;
                ViewBag.UserName = userName;
                return View();
            }
            else return RedirectToAction("Login", "Auth");
        }
        
    }
}