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
        RestorantEntities1 hotel = new RestorantEntities1();
        public JsonResult Login(string Username, string Password)
        {
            string encryptedPassword = EncryptPassword(Password);

            var user = hotel.Users.FirstOrDefault(u => u.Username == Username && u.Password == encryptedPassword);

            if (user != null)
            {
                Session["UserId"] = user.Id_User;
                Session["Username"] = user.Username;

                var reservations = hotel.TableReservations
                .Where(r => r.Id_User == user.Id_User)
                .ToList(); 

                var latestReservation = reservations
                    .Where(r => !r.HasUserBeenNotified)
                    .OrderByDescending(r => r.CreatedAt) 
                    .FirstOrDefault();

                var notification = latestReservation != null ? new
                {
                    latestReservation.Id_ReservationTable,
                    latestReservation.Id_ReservationStatus,
                    Date = latestReservation.Reservation_Date.ToString("yyyy-MM-dd"),
                    Time = latestReservation.Reservation_Time.ToString(@"hh\:mm")
                } : null;

                Session["Notification"] = notification;

                string redirectUrl = user.Id_Roli == 1
                                     ? Url.Action("Manager", "Home")
                                     : Url.Action("Tables", "Home");

                return Json(new { redirect = true, redirectUrl, notification });
            }

            return Json(new { redirect = false, message = "Përdorues ose fjalëkalim i pasaktë. Ju lutem provoni përsëri." });
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