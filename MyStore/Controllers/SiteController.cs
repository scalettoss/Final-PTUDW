using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MyClass.DAO;
using MyClass.Model;
using MyStore.App_Start;
using MyStore.Libarary;

namespace MyStore.Controllers
{
    public class SiteController : Controller
    {
        private MyDBContext db = new MyDBContext();
        ProductsDAO productsDAO = new ProductsDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        public ActionResult Index()
        {
            var listSP = productsDAO.getList();
            var listCat = categoriesDAO.getList();
            ViewBag.Products = listSP;
            ViewBag.Categories = listCat;
            
            return View(listSP);

        }
        public ActionResult Product() 
        {
            var listSP = productsDAO.getList();
            var listCat = categoriesDAO.getList();
            ViewBag.Categories = listCat;
            return View(listSP);
        }
        [HttpPost]
        public ActionResult Product(string filter)
        {
            var listSP = productsDAO.getList();
            var listCat = categoriesDAO.getList();
            ViewBag.Categories = listCat;
            List<Products> listNews = db.Products.Where(m => m.Name.ToLower().Contains(filter.ToLower()) == true).ToList();
            if(listNews.Count > 0)
            {
                return View(listNews);
            }
            return View(listSP);
        }
        public ActionResult DetailProduct(int? id)
        {
            var listCat = categoriesDAO.getList();
            ViewBag.Categories = listCat;
            if (id != null)
            {
                var pro = productsDAO.getRow(id);
                return View(pro);
            }
            else
            {
                return View();
            }
        }
        
    }
}