using SQLite;

namespace CinemaSeatReservation.Models;

public class Hall
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Hall name
    public int Capacity { get; set; } // Total seat capacity
    public string Layout { get; set; } = string.Empty; // JSON layout for the hall
}
