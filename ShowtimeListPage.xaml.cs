using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class ShowtimeListPage : ContentPage
{
    public ShowtimeListPage()
    {
        InitializeComponent();
        LoadShowtimes();
    }

    private void LoadShowtimes()
    {
        var showtimes = App.Database.Table<Showtime>()
            .Join(App.Database.Table<Movie>(), s => s.MovieId, m => m.Id, (s, m) => new
            {
                MovieTitle = m.Title,
                s.HallName,
                s.DateTime,
                s.Id
            })
            .ToList();

        ShowtimeListView.ItemsSource = showtimes; // Assigns the data to the ListView
    }

    private async void OnShowtimeSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            dynamic selectedShowtime = e.SelectedItem;
            int showtimeId = selectedShowtime.Id;

            // Navigate to SeatSelectionPage using showtimeId
            await Navigation.PushAsync(new SeatSelectionPage(showtimeId));

            // Clear selection
            ShowtimeListView.SelectedItem = null;
        }
    }
}