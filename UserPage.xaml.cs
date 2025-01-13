
using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;
public partial class UserPage : ContentPage
{
    private User _user;
    public UserPage(User user)
	{
		InitializeComponent();
        LoadMovies();
        _user = user;
    }
    private void LoadMovies()
    {
        // Load movies from the database into the CollectionView
        MoviesCollectionView.ItemsSource = App.Database.Table<Movie>().ToList();
    }

    private async void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedMovie = e.CurrentSelection.FirstOrDefault() as Movie;
        if (selectedMovie != null)
        {
            // Pass both the user and the selected movie to the ReservationPage
            await Navigation.PushAsync(new ReservationPage(_user, selectedMovie));
        }
    }
    private async void OnViewPastReservationsClicked(object sender, EventArgs e)
    {
        // Navigate to the past reservations page
        await Navigation.PushAsync(new PastReservationsPage(_user));
    }
}