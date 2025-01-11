using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class ReservationPage : ContentPage
{
    private Movie _movie;
    private User _user;
    private List<string> _selectedSeats = new List<string>(); // To track selected seats

    public ReservationPage(User user, Movie movie)
	{
		InitializeComponent();
        _user = user;
        _movie = movie;
        MovieTitleLabel.Text = movie.Title;
        GenerateSeatGrid();
    }
    private void GenerateSeatGrid()
    {
        // Create a 5x5 seat grid
        int rows = 5;
        int columns = 5;

        // Define grid rows and columns
        for (int row = 0; row < rows; row++)
        {
            SeatGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }
        for (int col = 0; col < columns; col++)
        {
            SeatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        }

        // Add buttons to the grid
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                string seatId = $"{(char)('A' + row)}{col + 1}"; // e.g., A1, A2, B1, etc.

                var button = new Button
                {
                    Text = seatId,
                    BackgroundColor = Colors.LightGray,
                    TextColor = Colors.Black,
                    CornerRadius = 5
                };

                button.Clicked += (sender, e) => OnSeatSelected(button, seatId);

                // Correct parameter order for Add method: (child, column, row)
                Grid.SetColumn(button, col); // Set column position
                Grid.SetRow(button, row);   // Set row position
                SeatGrid.Children.Add(button); // Add the button to the grid
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

        if (TimePicker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a time.", "OK");
            return;
        }

        var reservation = new Reservation
        {
            UserId = _user.Id,
            MovieId = _movie.Id,
            Seats = string.Join(",", _selectedSeats), // Save selected seats as comma-separated values
            ReservationDate = DateTime.Now,
            Showtime = TimePicker.SelectedItem.ToString()
        };

        App.Database.Insert(reservation);

        await DisplayAlert("Success", $"Reservation Confirmed!\nSeats: {reservation.Seats}\nTime: {reservation.Showtime}", "OK");
        await Navigation.PopAsync(); // Navigate back to the UserPage
    }
}
