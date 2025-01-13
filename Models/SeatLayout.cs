namespace CinemaSeatReservation.Models;

public class SeatLayout
{
    public List<Row> Rows { get; set; } = new List<Row>();
}

public class Row
{
    public string RowName { get; set; } = string.Empty; // Row name, e.g., "A"
    public List<string> Seats { get; set; } = new List<string>(); // List of seat IDs
}
