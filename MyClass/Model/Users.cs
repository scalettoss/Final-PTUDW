using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tài khoản không được để trống")]
        [Display(Name = "Tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Img { get; set; }

        [Display(Name = "Quyền hạn")]
        public string Role { get; set; }
      
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }
    }
}