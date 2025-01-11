using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class CorrelateMovieToHallPage : ContentPage
{
	public CorrelateMovieToHallPage()
	{
		InitializeComponent();
        LoadMovies();
        LoadHalls();
    }
    private void LoadMovies()
    {
        // Load movies into the MoviePicker
        MoviePicker.ItemsSource = App.Database.Table<Movie>().ToList();
    }

    private void LoadHalls()
    {
        // Load halls into the HallPicker
        HallPicker.ItemsSource = App.Database.Table<Hall>().ToList();
    }

    private async void OnCorrelateClicked(object sender, EventArgs e)
    {
        var selectedMovie = MoviePicker.SelectedItem as Movie;
        var selectedHall = HallPicker.SelectedItem as Hall;

        if (selectedMovie != null && selectedHall != null)
        {
            // Correlate the movie to the hall
            var showtime = new Showtime
            {
                MovieId = selectedMovie.Id,
                HallName = selectedHall.Name,
                DateTime = DateTime.Now // Example: You can let admin choose a custom date/time
            };

            App.Database.Insert(showtime);

            MessageLabel.Text = $"Successfully correlated '{selectedMovie.Title}' to '{selectedHall.Name}'.";
            MessageLabel.IsVisible = true;

            await DisplayAlert("Success", "Movie correlated to Hall!", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Please select both a movie and a hall.", "OK");
        }
    }
}