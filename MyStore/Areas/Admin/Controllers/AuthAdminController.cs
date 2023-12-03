using MyClass.DAO;
using MyStore.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.Areas.Admin.Controllers
{
    public class AuthAdminController : Controller
    {
        private readonly Services.AuthenticationService _authenticationService = new Services.AuthenticationService();
        UsersDAO usersDAO = new UsersDAO();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var user = usersDAO.getUser(username, password);
            if (user != null && user.Status == 1)
            {
                if (user.Role == "admin")
                {
                    SessionConfig.SetUser(user);
                    Session["user"] = SessionConfig.GetUser();
                    return RedirectToAction("Index", "Dashboard");
                }
                ViewBag.Login = "Tài khoản không được quyền đăng nhập!";
                return View();
            }
            ViewBag.Login = "Đăng nhập không thành công!";
            return View();
        }
        public ActionResult Logout()
        {
            SessionConfig.SetUser(null);
            return RedirectToAction("Login");
        }
    }
}