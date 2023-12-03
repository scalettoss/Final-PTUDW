using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStore.App_Start
{
    public class SessionConfig
    {
        private MyDBContext db = new MyDBContext();
        public static void SetUser(Users user)
        {
            HttpContext.Current.Session["user"] = user;
        }
        public static Users GetUser()
        {
            return (Users)HttpContext.Current.Session["user"];
        }
    }
}