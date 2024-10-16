//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TableReservation
    {
        public int Id_ReservationTable { get; set; }
        public System.DateTime Reservation_Date { get; set; }
        public System.TimeSpan Reservation_Time { get; set; }
        public int Id_User { get; set; }
        public int Id_Table { get; set; }
        public int TableNumber { get; set; }
        public int NumberOfPersons { get; set; }
        public int Id_ReservationStatus { get; set; }
        public bool HasUserBeenNotified { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    
        public virtual ReservateStatu ReservateStatu { get; set; }
        public virtual Table Table { get; set; }
        public virtual User User { get; set; }
    }
}
