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
        RestorantEntities1 restorant = new RestorantEntities1();

        [HttpPost]
        public JsonResult AddReservation(int tableId, int numPeople, DateTime reservationDate, TimeSpan reservationTime)
        {
            try
            {
                using (var db = new RestorantEntities1())
                {
                    TimeSpan maxSqlTime = new TimeSpan(23, 59, 59);

                    var lowerTimeLimit = reservationTime - TimeSpan.FromHours(1);
                    if (lowerTimeLimit < TimeSpan.Zero)
                        lowerTimeLimit = TimeSpan.Zero;

                    var upperTimeLimit = reservationTime + TimeSpan.FromHours(2);
                    if (upperTimeLimit > maxSqlTime)
                        upperTimeLimit = maxSqlTime;


                    var existingReservation = db.TableReservations
                        .Where(r => r.Id_Table == tableId
                                    && r.Reservation_Date == reservationDate
                                    && r.Reservation_Time >= lowerTimeLimit
                                    && r.Reservation_Time <= upperTimeLimit
                                    && r.Id_ReservationStatus == 1) 
                        .FirstOrDefault();

                    if (existingReservation != null)
                    {
                        return Json(new { success = false, message = "Koha e përzgjedhur për rezervim në këtë tavolinë është aktualisht e rezervuar. Ju lutem provoni të rezervoni një tavolinë tjetër ose një orar tjetër për këtë tavolinë.Faleminderit." });
                    }

                    var newReservation = new TableReservation
                    {
                        Reservation_Date = reservationDate,
                        Reservation_Time = reservationTime,
                        Id_User = (int)Session["UserId"], 
                        Id_Table = tableId,
                        NumberOfPersons = numPeople,
                        CreatedAt = DateTime.Now,
                        TableNumber = tableId,
                        Id_ReservationStatus = 4 
                    };

                    db.TableReservations.Add(newReservation);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Kërkesa për rezervim u dërgua me sukses. Do të njoftoheni në lidhje me përgjigjen e rezervimit." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ndodhi një gabim gjatë dërgimit të rezervimit: " + ex.Message });
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
                    reservation.HasUserBeenNotified = false;
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

            return PartialView("~/Views/_ReservationsTablePartial.cshtml", reservations);
        }
    }
}