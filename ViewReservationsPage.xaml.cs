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
                r.Seats,
                //r.ReservationDate,
                MovieId = s.MovieId,
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
            .ToList();

        ReservationsListView.ItemsSource = reservations;
    }
}