using CinemaSeatReservation.Models;

namespace CinemaSeatReservation;

public partial class ViewReservationsPage : ContentPage
{
    public ViewReservationsPage()
    {
        InitializeComponent();
        LoadReservations();
    }

    private void LoadReservations()
    {
        var reservations = App.Database.Table<Reservation>()
            .Join(App.Database.Table<Showtime>(), r => r.ShowtimeId, s => s.Id, (r, s) => new
            {
                r.Id, // Reservation ID
                r.Seats,
                MovieId = s.MovieId,
                s.HallName,
                s.DateTime
            })
            .Join(App.Database.Table<Movie>(), rs => rs.MovieId, m => m.Id, (rs, m) => new ReservationViewModel
            {
                ReservationId = rs.Id,
                MovieTitle = m.Title,
                HallName = rs.HallName,
                DateTime = rs.DateTime,
                Seats = rs.Seats
            })
            .ToList();

        ReservationsListView.ItemsSource = reservations;
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ReservationViewModel reservation)
        {
            var result = await DisplayAlert("Confirm", $"Are you sure you want to delete this reservation for {reservation.MovieTitle}?", "Yes", "No");
            if (result)
            {
                try
                {
                    // Remove reservation from the database
                    App.Database.Delete<Reservation>(reservation.ReservationId);
                    await DisplayAlert("Success", "Reservation deleted successfully.", "OK");
                    LoadReservations(); // Refresh the list
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred while deleting the reservation: {ex.Message}", "OK");
                }
            }
        }
    }

    private async void OnDoneButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ReservationViewModel reservation)
        {
            // Optionally, mark the reservation as processed
            await DisplayAlert("Info", $"The reservation for {reservation.MovieTitle} has been processed.", "OK");
            // Here, you can update a 'Status' field in the database if needed
            LoadReservations(); // Refresh the list
        }
    }
}

// ViewModel for Reservations
public class ReservationViewModel
{
    public int ReservationId { get; set; }
    public string MovieTitle { get; set; }
    public string HallName { get; set; }
    public DateTime DateTime { get; set; }
    public string Seats { get; set; }
}
