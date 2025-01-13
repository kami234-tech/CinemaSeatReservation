using CinemaSeatReservation.Models;

namespace CinemaSeatReservation;

public partial class PastReservationsPage : ContentPage
{
    private readonly User _user;

    public PastReservationsPage(User user)
    {
        InitializeComponent();
        _user = user; // Save the logged-in user's information
        LoadPastReservations();
    }

    private void LoadPastReservations()
    {
        var pastReservations = App.Database.Table<Reservation>()
            .Where(r => r.UserId == _user.Id) // Filter reservations by user ID
            .Join(App.Database.Table<Showtime>(), r => r.ShowtimeId, s => s.Id, (r, s) => new
            {
                r.Seats,
                s.MovieId,
                s.HallName,
                s.DateTime
            })
            .Join(App.Database.Table<Movie>(), rs => rs.MovieId, m => m.Id, (rs, m) => new
            {
                MovieTitle = m.Title,
                rs.HallName,
                rs.DateTime,
                rs.Seats
            })
            .OrderByDescending(r => r.DateTime) // Show the most recent reservations first
            .ToList();

        PastReservationsListView.ItemsSource = pastReservations;
    }
}
