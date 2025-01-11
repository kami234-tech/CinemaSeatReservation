using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSeatReservation.Models
{
    public class Reservation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Seats { get; set; } // Reserved seats
        public DateTime ReservationDate { get; set; } // Reservation creation timestamp
        public string Showtime { get; set; } // Time selected by the user
    }
}
