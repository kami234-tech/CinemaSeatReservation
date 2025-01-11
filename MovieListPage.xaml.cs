namespace CinemaSeatReservation;
using CinemaSeatReservation.Models;
public partial class MovieListPage : ContentPage
{
    private User _user;
    public MovieListPage(User user)
	{
		InitializeComponent();
        _user = user;
        LoadMovies();
    }
    private void LoadMovies()
    {
        var showtimes = App.Database.Table<Showtime>().ToList();
        var moviesWithShowtimes = showtimes.Select(st => new
        {
            Movie = App.Database.Table<Movie>().FirstOrDefault(m => m.Id == st.MovieId),
            Showtime = st
        }).ToList();

        MoviesListView.ItemsSource = moviesWithShowtimes;
    }
}