using Microsoft.Maui.Storage;
using CinemaSeatReservation.Models;

namespace CinemaSeatReservation;

public partial class AdminPage : ContentPage
{
    private string? _selectedPhotoPath;

    public AdminPage()
    {
        InitializeComponent();
        LoadMovies();
        _selectedPhotoPath = null;
    }

    private void LoadMovies()
    {
        MoviesListView.ItemsSource = App.Database.Table<Movie>().ToList();
    }

    private async void OnCreateMovieClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateMoviePage());
    }

    private async void OnMovieSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var movie = e.SelectedItem as Movie;
        if (movie != null)
        {
            // Check if the movie is linked to reservations
            var linkedReservations = App.Database.Table<Reservation>()
                .Where(r => r.MovieId == movie.Id)
                .ToList();

            if (linkedReservations.Any())
            {
                await DisplayAlert("Error", "This movie is linked to existing reservations and cannot be deleted.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Delete Movie", $"Are you sure you want to delete '{movie.Title}'?", "Yes", "No");
            if (confirm)
            {
                App.Database.Delete(movie);
                LoadMovies(); // Refresh the movie list
            }
        }
    }
    private async void OnViewShowtimesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ViewShowtimesPage());
    }

    private async void OnViewHallsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ViewHallsPage());
    }
    private async void OnAddHallClicked(object sender, EventArgs e)
    {
        // Navigate to Add Hall Page
        await Navigation.PushAsync(new AddHallPage());
    }

    private async void OnViewReservationsClicked(object sender, EventArgs e)
    {
        // Navigate to View Reservations Page
        await Navigation.PushAsync(new ViewReservationsPage());
    }

    private async void OnCreateShowtimeClicked(object sender, EventArgs e)
    {
        // Navigate to Create Showtime Page
        await Navigation.PushAsync(new CreateShowtimePage());
    }
    private void OnMouseEntered(object sender, EventArgs e)
    {
        if (sender is StackLayout stackLayout && stackLayout.BindingContext is Movie movie)
        {
            // Update hover card with movie details
            MovieDetailsTitle.Text = movie.Title;
            MovieDetailsGenre.Text = $"Genre: {movie.Genre}";
            MovieDetailsDuration.Text = $"Duration: {movie.Duration} minutes";
            MovieDetailsDescription.Text = $"Description: {movie.Description}";

            MovieDetailsFrame.IsVisible = true; // Show the hover card
        }
    }

    private void OnMouseExited(object sender, EventArgs e)
    {
        MovieDetailsFrame.IsVisible = false; // Hide the hover card
    }
}
