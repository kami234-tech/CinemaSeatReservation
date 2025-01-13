using CinemaSeatReservation.Models;
using System.Text.Json;
using Microsoft.Maui.Storage;
using System.IO;
namespace CinemaSeatReservation;

public partial class SeatSelectionPage : ContentPage
{
    private Showtime _showtime;
    private List<string> _selectedSeats = new List<string>();

    public SeatSelectionPage(int showtimeId)
    {
        InitializeComponent();
        // Fetch the showtime based on the ID
        _showtime = App.Database.Table<Showtime>().First(s => s.Id == showtimeId);
        GenerateSeatGrid();
    }
    private async void OnGenerateSeatLayoutJsonClicked(object sender, EventArgs e)
    {
        try
        {
            // Define the default seat layout
            var seatLayout = new SeatLayout
            {
                Rows = new List<Row>
            {
                new Row { RowName = "A", Seats = new List<string> { "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "A10", "A11" } },
                new Row { RowName = "B", Seats = new List<string> { "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "B10", "B11" } },
                new Row { RowName = "C", Seats = new List<string> { "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11" } },
                new Row { RowName = "D", Seats = new List<string> { "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10", "D11" } },
                new Row { RowName = "E", Seats = new List<string> { "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "E10", "E11" } },
                new Row { RowName = "F", Seats = new List<string> { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11" } }
            }
            };

            // Serialize the seat layout to JSON
            string json = JsonSerializer.Serialize(seatLayout, new JsonSerializerOptions { WriteIndented = true });

            // Get the path to save the JSON file
            string path = Path.Combine(FileSystem.AppDataDirectory, "GeneratedSeatLayout.json");

            // Write the JSON content to the file
            await File.WriteAllTextAsync(path, json);

            // Notify the user of success
            await DisplayAlert("Success", $"Seat layout JSON has been saved to: {path}", "OK");
        }
        catch (Exception ex)
        {
            // Handle and display errors
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private void GenerateSeatGrid()
    {
        // Fetch the hall associated with the showtime
        var hall = App.Database.Table<Hall>().First(h => h.Name == _showtime.HallName);

        // Deserialize the hall's seat layout
        var layout = JsonSerializer.Deserialize<SeatLayout>(hall.Layout);

        // Fetch reserved seats for the specific showtime
        var reservedSeats = App.Database.Table<Reservation>()
            .Where(r => r.ShowtimeId == _showtime.Id)
            .SelectMany(r => r.Seats.Split(','))
            .ToList();

        SeatGrid.Children.Clear(); // Clear existing elements in the grid

        // Create seat grid dynamically
        foreach (var row in layout.Rows)
        {
            foreach (var seat in row.Seats)
            {
                var button = new Button
                {
                    Text = seat,
                    BackgroundColor = reservedSeats.Contains(seat) ? Colors.Black : Colors.LightGray,
                    IsEnabled = !reservedSeats.Contains(seat) // Disable reserved seats
                };

                button.Clicked += (sender, e) => OnSeatSelected(button, seat);
                SeatGrid.Children.Add(button);
            }
        }
    }

    private void OnSeatSelected(Button button, string seatId)
    {
        if (_selectedSeats.Contains(seatId))
        {
            // Deselect seat
            _selectedSeats.Remove(seatId);
            button.BackgroundColor = Colors.LightGray;
        }
        else
        {
            // Select seat
            _selectedSeats.Add(seatId);
            button.BackgroundColor = Colors.Green;
        }
    }

    private async void OnConfirmReservationClicked(object sender, EventArgs e)
    {
        if (_selectedSeats.Count == 0)
        {
            await DisplayAlert("Error", "Please select at least one seat.", "OK");
            return;
        }

        var reservation = new Reservation
        {
            UserId = App.CurrentUser.Id,
            ShowtimeId = _showtime.Id,
            Seats = string.Join(",", _selectedSeats)
        };

        App.Database.Insert(reservation);

        await DisplayAlert("Success", "Seats reserved successfully!", "OK");
        await Navigation.PopAsync();
    }

}

