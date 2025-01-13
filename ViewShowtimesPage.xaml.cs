using CinemaSeatReservation.Models;

namespace CinemaSeatReservation;

public partial class ViewShowtimesPage : ContentPage
{
    public ViewShowtimesPage()
    {
        InitializeComponent();
        LoadShowtimes();
    }

    private void LoadShowtimes()
    {
        try
        {
            // Fetch showtimes with movie titles
            var showtimes = App.Database.Table<Showtime>()
                .Join(
                    App.Database.Table<Movie>(),
                    showtime => showtime.MovieId,
                    movie => movie.Id,
                    (showtime, movie) => new ShowtimeViewModel
                    {
                        Id = showtime.Id,
                        MovieTitle = movie.Title,
                        HallName = showtime.HallName,
                        DateTime = showtime.DateTime
                    })
                .ToList();

            // Assign the showtimes to the ListView
            ShowtimesListView.ItemsSource = showtimes;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Failed to load showtimes: {ex.Message}", "OK");
        }
    }

    private async void OnShowtimeSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Check if the selected item is valid
        if (e.SelectedItem is ShowtimeViewModel selectedShowtime)
        {
            bool confirm = await DisplayAlert(
                "Delete Showtime",
                $"Are you sure you want to delete the showtime for '{selectedShowtime.MovieTitle}' in Hall '{selectedShowtime.HallName}'?",
                "Yes",
                "No"
            );

            if (confirm)
            {
                try
                {
                    // Find the Showtime by ID and delete it
                    var showtimeToDelete = App.Database.Table<Showtime>().First(s => s.Id == selectedShowtime.Id);
                    App.Database.Delete(showtimeToDelete);

                    // Reload the list
                    LoadShowtimes();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete showtime: {ex.Message}", "OK");
                }
            }

            // Clear the selection
            ShowtimesListView.SelectedItem = null;
        }
    }
}

// ViewModel to hold the showtime data
public class ShowtimeViewModel
{
    public int Id { get; set; }
    public string MovieTitle { get; set; }
    public string HallName { get; set; }
    public DateTime DateTime { get; set; }
}
