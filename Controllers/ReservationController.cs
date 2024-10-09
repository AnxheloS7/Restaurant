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

        RestorantEntities1 restorant = new RestorantEntities1();

        [HttpPost]
        public JsonResult AddReservation(int tableId, int numPeople, DateTime reservationDate, TimeSpan reservationTime)
        {
            try
            {
                using (var db = new RestorantEntities1())
                {
                    var newReservation = new TableReservation
                    {
                        Reservation_Date = reservationDate,
                        Reservation_Time = reservationTime,
                        Id_User = (int)Session["UserId"], // Assuming user session is set
                        Id_Table = tableId,
                        NumberOfPersons = numPeople,
                        CreatedAt = DateTime.Now,
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


        [HttpPost]
        public JsonResult MarkAsNotified(int id)
        {
            try
            {
                string sql = "UPDATE TableReservations SET HasUserBeenNotified = 1 WHERE Id_ReservationTable = @p0";
                restorant.Database.ExecuteSqlCommand(sql, id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }




        [HttpPost]
        public JsonResult UpdateReservationStatus(int id, int status)
        {
            try
            {
                // Update the reservation status in the database
                var reservation = restorant.TableReservations.Find(id);
                if (reservation != null)
                {
                    reservation.Id_ReservationStatus = status;
                    reservation.HasUserBeenNotified = false; // New column in the database to track notification
                    restorant.SaveChanges();

                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public PartialViewResult GetUpdatedReservations(int status)
        {
            // Fetch the updated reservations based on their status (Accepted = 1, Rejected = 2)
            var reservations = restorant.TableReservations
                                        .Where(r => r.Id_ReservationStatus == status)
                                        .ToList();

            // Assuming the partial is located in Views/Shared folder
            return PartialView("~/Views/_ReservationsTablePartial.cshtml", reservations);
        }
    }
}