using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class CreateShowtimePage : ContentPage
{
	public CreateShowtimePage()
	{
		InitializeComponent();
        LoadMoviesAndHalls();
    }
    private void LoadMoviesAndHalls()
    {
        var movies = App.Database.Table<Movie>().ToList();
        var halls = App.Database.Table<Hall>().ToList();

        MoviePicker.ItemsSource = movies;
        MoviePicker.ItemDisplayBinding = new Binding("Title");

        HallPicker.ItemsSource = halls;
        HallPicker.ItemDisplayBinding = new Binding("Name");
    }

    private async void OnCreateShowtimeClicked(object sender, EventArgs e)
    {
        var selectedMovie = MoviePicker.SelectedItem as Movie;
        var selectedHall = HallPicker.SelectedItem as Hall;
        var selectedDate = DatePicker.Date;
        var selectedTime = TimePicker.Time;

        if (selectedMovie == null || selectedHall == null)
        {
            ErrorLabel.Text = "Please select a movie and a hall.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var showtime = new Showtime
        {
            MovieId = selectedMovie.Id,
            HallName = selectedHall.Name,
            DateTime = selectedDate + selectedTime
        };

        App.Database.Insert(showtime);

        await DisplayAlert("Success", "Showtime created successfully!", "OK");
        await Navigation.PopAsync();
    }
}