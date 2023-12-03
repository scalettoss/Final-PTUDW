using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Helpers;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using MyStore.App_Start;
using MyStore.Libarary;
using MyStore.Services;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace MyStore.Controllers
{
    public class AuthController : Controller
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
            if (user == null)
            {
                TempData["login"] = "Đăng nhập không thành công! Sai tài khoản hoặc mật khẩu";
                return View();
            }
            else if(user.Status != 1)
            {
                TempData["login"] = "Tài khoản chưa được kích hoạt";
                return View();
            }
            else
            {
                SessionConfig.SetUser(user);
                Session["user"] = SessionConfig.GetUser();
                return RedirectToAction("Index", "Site");
            }    
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users newUser)
        {
            if (ModelState.IsValid)
            {
                var img = Request.Files["img"];//lay thong tin file
                if (img != null && img.ContentLength > 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string name = newUser.Username;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = name + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        newUser.Img = imgName;
                        //upload hinh
                        string PathDir = "/Public/img/user/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                bool res = _authenticationService.RegisterUser(newUser.Username, hashedPassword, newUser.Email , newUser.Img, "user");
                if(!res)
                {
                    TempData["mes"] = "Đăng kí thất bại! Vui lòng thử lại";
                    return RedirectToAction("Register", "Auth");
                }
                return RedirectToAction("Login", "Auth");
            }
            // Hiển thị thông báo lỗi nếu thông tin người dùng không hợp lệ
            ViewBag.Res = "Đăng kí thất bại! Vui lòng thử lại";
            return View();
        }
        public ActionResult Logout()
        {
            SessionConfig.SetUser(null);
            return RedirectToAction("Login");
        }
    }
}