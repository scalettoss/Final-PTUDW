using MyClass.DAO;
using MyStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        [RoleUser]
        public ActionResult Index()
        {
            return View();
        }






    }
}