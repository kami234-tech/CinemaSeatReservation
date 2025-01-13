using CinemaSeatReservation.Models;
using System.Text.Json;

namespace CinemaSeatReservation;

public partial class ReservationPage : ContentPage
{
    private Movie _movie;
    private User _user;
    private Showtime? _selectedShowtime;
    private List<string> _selectedSeats = new List<string>();

    public ReservationPage(User user, Movie movie)
    {
        InitializeComponent();
        _user = user;
        _movie = movie;
        MovieTitleLabel.Text = movie.Title;
        _selectedShowtime = null;
        LoadShowtimes();
    }

    private void LoadShowtimes()
    {
        // Fetch showtimes for the selected movie
        var showtimes = App.Database.Table<Showtime>()
            .Where(s => s.MovieId == _movie.Id)
            .ToList();

        // Populate the Picker
        ShowtimePicker.ItemsSource = showtimes;
        ShowtimePicker.ItemDisplayBinding = new Binding("DateTime", stringFormat: "{0:MM/dd/yyyy hh:mm tt}");
    }

    private async void OnShowtimeSelected(object sender, EventArgs e)
    {
        _selectedShowtime = ShowtimePicker.SelectedItem as Showtime;
        if (_selectedShowtime == null)
        {
            await DisplayAlert("Error", "Please select a valid showtime.", "OK");
            return;
        }

        GenerateSeatGrid();
    }

    private async void GenerateSeatGrid()
    {
        if (_selectedShowtime == null) return;

        SeatGrid.Children.Clear();
        SeatGrid.RowDefinitions.Clear();
        SeatGrid.ColumnDefinitions.Clear();

        // Fetch the reserved seats for the selected showtime
        var reservedSeats = App.Database.Table<Reservation>()
            .Where(r => r.ShowtimeId == _selectedShowtime.Id)
            .SelectMany(r => r.Seats.Split(',')) // Split the reserved seats string into a list
            .ToList();

        // Define rows and columns (assuming a static 6x11 grid layout)
        int rowCount = 6; // Rows A-F
        int columnCount = 11; // Columns 1-11

        for (int row = 0; row < rowCount; row++)
        {
            SeatGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        for (int col = 0; col < columnCount; col++)
        {
            SeatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        }

        // Create seat buttons and add them to the grid
        char rowLetter = 'A';
        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                string seatId = $"{rowLetter}{col + 1}";

                var seatButton = new Button
                {
                    Text = seatId,
                    BackgroundColor = reservedSeats.Contains(seatId) ? Colors.Violet : Colors.LightGray,
                    TextColor = Colors.Black,
                    CornerRadius = 5,
                    IsEnabled = !reservedSeats.Contains(seatId) // Disable the button if the seat is taken
                };

                // Attach click event for seat selection only if the seat is not reserved
                if (!reservedSeats.Contains(seatId))
                {
                    seatButton.Clicked += (sender, e) => OnSeatSelected(seatButton, seatId);
                }

                // Add the button to the grid
                SeatGrid.Children.Add(seatButton);
                Grid.SetRow(seatButton, row);
                Grid.SetColumn(seatButton, col);
            }
            rowLetter++; // Move to the next row (A -> B -> C, etc.)
        }
    }



    private void OnSeatSelected(Button button, string seatId)
    {
        if (_selectedSeats.Contains(seatId))
        {
            // Deselect the seat
            _selectedSeats.Remove(seatId);
            button.BackgroundColor = Colors.LightGray;
        }
        else
        {
            // Select the seat
            _selectedSeats.Add(seatId);
            button.BackgroundColor = Colors.Green;
        }
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        if (_selectedSeats.Count == 0)
        {
            await DisplayAlert("Error", "Please select at least one seat.", "OK");
            return;
        }

        if (_selectedShowtime == null)
        {
            await DisplayAlert("Error", "Please select a showtime.", "OK");
            return;
        }

        // Fetch existing reservations for the selected showtime
        var existingReservations = App.Database.Table<Reservation>()
            .Where(r => r.ShowtimeId == _selectedShowtime.Id)
            .ToList();

        // Check for seat conflicts
        var reservedSeats = existingReservations
            .SelectMany(r => r.Seats.Split(',')) // Split seat strings into individual seats
            .ToList();

        var conflictingSeats = _selectedSeats.Intersect(reservedSeats).ToList();

        if (conflictingSeats.Any())
        {
            await DisplayAlert("Error", $"The following seats are already reserved: {string.Join(", ", conflictingSeats)}", "OK");
            return;
        }

        // Create a new reservation
        var reservation = new Reservation
        {
            UserId = _user.Id,
            MovieId = _movie.Id,
            ShowtimeId = _selectedShowtime.Id,
            Seats = string.Join(",", _selectedSeats) // Combine selected seats into a single string
        };

        // Save the reservation to the database
        App.Database.Insert(reservation);

        // Show success message
        await DisplayAlert("Success", $"Reservation Confirmed!\nSeats: {reservation.Seats}\nTime: {_selectedShowtime.DateTime}", "OK");

        // Navigate back to the previous page
        await Navigation.PopAsync();
    }

    public class SeatLayout
    {
        public List<Row> Rows { get; set; } = new();
    }

    public class Row
    {
        public string RowName { get; set; } = string.Empty;
        public List<string> Seats { get; set; } = new();
    }
}
