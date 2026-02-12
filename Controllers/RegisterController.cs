using Rest.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Rest.Controllers
{
    public class RegisterController : Controller
    {
        RestorantEntities1 hotel = new RestorantEntities1();
                
        [HttpPost]
        public JsonResult Register(string Username, string Email, string Password, string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                return Json(new { redirect = false, message = "Passwords do not match" });
            }

            if (hotel.Users.Any(u => u.Username == Username))
            {
                return Json(new { redirect = false, message = "Username already exists" });
            }

            var newUser = new User
            {
                Username = Username,
                Email = Email,
                Password = EncryptPassword(Password),
                Id_Roli = 2
            };

            hotel.Users.Add(newUser);
            hotel.SaveChanges();

            return Json(new { redirect = true, redirectUrl = Url.Action("LoginForm", "Home") });
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

        public ActionResult Logout()
        {
            // Clear session and redirect to login page
            Session.Clear();
            Session["Username"] = null;
            return RedirectToAction("LoginForm", "Home");
        }
    }
}
