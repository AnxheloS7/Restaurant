using Rest.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rest.Controllers
{
    public class LoginController : Controller
    {
        RestorantEntities hotel = new RestorantEntities();
        public JsonResult Login(string Username, string Password)
        {
            string encryptedPassword = EncryptPassword(Password); // Implement your encryption method

            var user = hotel.Users.FirstOrDefault(u => u.Username == Username && u.Password == encryptedPassword);

            if (user != null)
            {
                Session["UserId"] = user.Id_User;
                Session["Username"] = user.Username;


                if (user.Id_Roli == 1)
                {
                    return Json(new { redirect = true, redirectUrl = Url.Action("Manager", "Home") });
                }
                else
                {
                    return Json(new { redirect = true, redirectUrl = Url.Action("Tables", "Home") });
                }
            }

            return Json(new { redirect = false, message = "Invalid username or password." });
        }

        private string EncryptPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

    }
}