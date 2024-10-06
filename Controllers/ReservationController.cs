using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rest.Controllers
{
    public class ReservationController : Controller
    {
        //// GET: Reservation
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        public JsonResult AddReservation(int tableId, int numPeople, DateTime reservationDate, TimeSpan reservationTime)
        {
            try
            {
                using (var db = new RestorantEntities())
                {
                    var newReservation = new TableReservation
                    {
                        Reservation_Date = reservationDate,
                        Reservation_Time = reservationTime,
                        Id_User = (int)Session["UserId"], // Assuming user session is set
                        Id_Table = tableId,
                        NumberOfPersons = numPeople,
                        TableNumber = tableId,
                        Id_ReservationStatus = 4
                    };

                    db.TableReservations.Add(newReservation);
                    db.SaveChanges();
                }

                // Return success response as JSON
                return Json(new { success = true, message = "Reservation request successfully made." });
            }
            catch (Exception ex)
            {
                // Handle any errors and return an error message
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }


    }
}