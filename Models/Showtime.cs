using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSeatReservation.Models
{
    public class Showtime
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int MovieId { get; set; } // Foreign Key to Movie
        public string HallName { get; set; } // Name of the Hall
        public DateTime DateTime { get; set; } // Date and time of the show

        // Navigation properties (optional)
        //public Movie Movie { get; set; }
        //public Hall Hall { get; set; }
    }
}
