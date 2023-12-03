using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt;
using System.Runtime.Remoting.Contexts;


namespace MyClass.DAO
{
    public class UsersDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Users> getList()
        {
            return db.Users.ToList();
        }
        public List<Users> getList(string status = "ALL")//status 0,1,2
        {
            List<Users> list = null;
            switch (status)
            {
                case "Index"://1,2
                    {
                        list = db.Users.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash"://0
                    {
                        list = db.Users.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Users.ToList();
                        break;
                    }
            }
            return list;
        }
        public Users getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Users.Find(id);
            }
        }
        public Users getUser(string username, string password)
        {
            var user = db.Users.SingleOrDefault(m => m.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        public int Insert(Users row)
        {
            db.Users.Add(row);
            return db.SaveChanges();
        }
        public int Update(Users row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        public int Delete(Users row)
        {
            db.Users.Remove(row);
            return db.SaveChanges();
        }
    }
}
