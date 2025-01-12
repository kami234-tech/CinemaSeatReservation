using CinemaSeatReservation.Models;
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
        var showtimes = App.Database.Table<Showtime>()
            .Where(s => s.MovieId == _movie.Id)
            .ToList();

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

        SeatGrid.Children.Clear();
        GenerateSeatGrid();
    }

    private void GenerateSeatGrid()
    {
        if (_selectedShowtime == null) return;

        // Fetch reserved seats for the selected showtime
        var reservedSeats = App.Database.Table<Reservation>()
            .Where(r => r.ShowtimeId == _selectedShowtime.Id)
            .SelectMany(r => r.Seats.Split(','))
            .ToList();

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                string seatId = $"{(char)('A' + row)}{col + 1}";

                var button = new Button
                {
                    Text = seatId,
                    BackgroundColor = reservedSeats.Contains(seatId) ? Colors.Red : Colors.LightGray,
                    TextColor = Colors.Black,
                    CornerRadius = 5,
                    IsEnabled = !reservedSeats.Contains(seatId) // Disable if reserved
                };

                button.Clicked += (sender, e) => OnSeatSelected(button, seatId);

                // Add the button to the Grid
                SeatGrid.Children.Add(button);
                Grid.SetRow(button, row); // Specify the row
                Grid.SetColumn(button, col); // Specify the column
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

        var reservation = new Reservation
        {
            UserId = _user.Id,
            MovieId = _movie.Id,
            ShowtimeId = _selectedShowtime.Id,
            //Seats = string.Join(",", _selectedSeats),
            //ReservationDate = DateTime.Now
        };

        App.Database.Insert(reservation);

        await DisplayAlert("Success", $"Reservation Confirmed!\nSeats: {reservation.Seats}\nTime: {_selectedShowtime.DateTime}", "OK");
        await Navigation.PopAsync(); // Navigate back to the UserPage
    }
}
