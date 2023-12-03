using MyClass.DAO;
using MyClass.Model;
using MyStore.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStore.Controllers
{
    public class MenuModuleController : Controller
    {
        // GET: MenuModule
        private MyDBContext db = new MyDBContext();
        MenusDAO menusDAO = new MenusDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        public ActionResult MainMenu()
        {
            var user = SessionConfig.GetUser();
            string userName = user != null ? user.Username : string.Empty;
            ViewBag.UserName = userName;
            return View(menusDAO.getListByParentId(0));
        }
    }
}