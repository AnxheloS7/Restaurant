using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rest.Controllers
{
    public class HomeController : Controller
    {
        RestorantEntities1 hotel = new RestorantEntities1();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tables()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            var user = hotel.Users.FirstOrDefault(u => u.Id_User == userId);

            if (user == null)
            {
                return RedirectToAction("LoginForm", "Home");
            }

            ViewBag.FullName = user.Username;

            var tables = hotel.Tables.Select(t => new TableViewModel
            {
                TableNumber = t.Table_Number,
                Capacity = t.Capacity,
                Status = t.StatusTable.Emer_StatusTable
            }).ToList();

            if (!tables.Any())
            {
                return Content("No tables available");
            }

            return View(tables);
        }


        public ActionResult Manager()
        {
            using (var context = new RestorantEntities1())
            {
                // Fetch the table reservation data

                var reservations = context.TableReservations
                .Include("User")
                .Include("Table")
                .Include("ReservateStatu")
                .ToList();

                // Pass the data to the view
                return View(reservations);
            }
        }

        public ActionResult LoginForm()
        {
            return View("LoginForm");
        }
    }
}