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
            bool confirm = await DisplayAlert("Delete Movie", $"Are you sure you want to delete '{movie.Title}'?", "Yes", "No");
            if (confirm)
            {
                App.Database.Delete(movie);
                LoadMovies();
            }
        }
    }

    private async void OnAddMovieImageClicked(object sender, EventArgs e)
    {
        // Navigate to the AddImagePage
        await Navigation.PushAsync(new AddIamgePage());
    }


    private async void OnAddHallClicked(object sender, EventArgs e)
    {
        // Navigate to Add Hall Page (to be implemented)
        await Navigation.PushAsync(new AddHallPage());
    }
    

    private async void OnViewReservationsClicked(object sender, EventArgs e)
    {
        // Navigate to View Reservations Page (to be implemented)
        await Navigation.PushAsync(new ViewReservationsPage());
    }

}