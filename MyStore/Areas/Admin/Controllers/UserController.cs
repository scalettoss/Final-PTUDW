using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using MyStore.Libarary;
using MyStore.Services;
using BCrypt;

namespace MyStore.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly Services.AuthenticationService _authenticationService = new Services.AuthenticationService();
        private MyDBContext db = new MyDBContext();
        UsersDAO usersDAO = new UsersDAO();
        // GET: Admin/User
        [RoleUser]
        public ActionResult Index()
        {
            return View(usersDAO.getList("Index"));
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại người dùng");
                return RedirectToAction("Index");
            }
            Users user = usersDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại người dùng");
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Create()
        {
            List<SelectListItem> roleOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "admin", Text = "Admin" },
                new SelectListItem { Value = "user", Text = "User" }
            };
            ViewBag.listRole = new SelectList(roleOptions, "Value", "Text");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users user)
        {
            if (ModelState.IsValid)
            {
                var img = Request.Files["img"];
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string name = user.Username;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = name + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        user.Img = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/user";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                bool res = _authenticationService.RegisterUser(user.Username, hashedPassword, user.Email, user.Img, user.Role);
                if (res)
                {
                    TempData["message"] = new XMessage("success", "Tạo mới người dùng thành công");
                    return RedirectToAction("Index");
                }
            }
            TempData["message"] = new XMessage("danger", "Thao tác thất bại");
            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại người dùng");
                return RedirectToAction("Index");
            }
            Users user = usersDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại người dùng");
                return RedirectToAction("Index");
            }
            List<SelectListItem> roleOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "admin", Text = "Admin" },
                new SelectListItem { Value = "user", Text = "User" }
            };
            ViewBag.listRole = new SelectList(roleOptions, "Value", "Text");
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users user)
        {
            if (ModelState.IsValid)
            {
                user.UpdateAt = DateTime.Now;
                usersDAO.Update(user);
                TempData["message"] = new XMessage("success", "Cập nhật người dùng thành công");
                return RedirectToAction("Index");
            }
            List<SelectListItem> roleOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "admin", Text = "Admin" },
                new SelectListItem { Value = "user", Text = "User" }
            };
            ViewBag.listRole = new SelectList(roleOptions, "Value", "Text");
            return View(user);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            Users user = usersDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // POST: Admin/Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users user = usersDAO.getRow(id);
            var img = Request.Files["img"];//lay thong tin file
            string PathDir = "~/Public/img/supplier";
            if (usersDAO.Delete(user) == 1)
            {
                //Xu ly cho muc xoa hinh anh
                if (user.Img != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), user.Img);
                    System.IO.File.Delete(DelPath);
                }
            }
            TempData["message"] = new XMessage("success", "Xóa người dùng thành công");
            return RedirectToAction("Trash");
        }

        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //truy van dong co id = id yeu cau
            Users user = usersDAO.getRow(id);
            if (user == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                //chuyen doi trang thai cua Satus tu 1<->2
                user.Status = (user.Status == 1) ? 2 : 1;
                //cap nhat gia tri UpdateAt
                user.UpdateAt = DateTime.Now;
                //cap nhat lai DB
                usersDAO.Update(user);
                //thong bao cap nhat trang thai thanh cong
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
                return RedirectToAction("Index");
            }
        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            Users user = usersDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            else
            {
                //truy van id
                // khong hien len trang index
                user.Status = 0;
                //cap nhat gia tri UpdateAt
                user.UpdateAt = DateTime.Now;
                //cap nhat lai DB
                usersDAO.Update(user);
                //thong bao cap nhat trang thai thanh cong
                TempData["message"] = new XMessage("success", "Xóa sản phẩm thành công");
                return RedirectToAction("Index");
            }
        }

        public ActionResult Trash()
        {
            return View(usersDAO.getList("Trash"));
        }

        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            Users user = usersDAO.getRow(id);
            if (user == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            else
            {
                //truy van id
                user.Status = 2;
                //cap nhat gia tri UpdateAt
                user.UpdateAt = DateTime.Now;

                //cap nhat lai DB
                usersDAO.Update(user);
                //thong bao cap nhat trang thai thanh cong
                TempData["message"] = new XMessage("success", "Phục hồi sản phẩm thành công");
                return RedirectToAction("Index");
            }
        }
    }
}
