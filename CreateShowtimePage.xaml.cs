using CinemaSeatReservation.Models;

namespace CinemaSeatReservation;

public partial class CreateShowtimePage : ContentPage
{
    public CreateShowtimePage()
    {
        InitializeComponent();
        LoadMoviesAndHalls();
        LoadTimeSlots();
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

    private void LoadTimeSlots()
    {
        // Generate time slots starting from 10:00 AM to 10:00 PM in 2:30-hour intervals
        var timeSlots = new List<TimeSpan>();
        var startTime = TimeSpan.FromHours(10); // 10:00 AM
        var endTime = TimeSpan.FromHours(22); // 10:00 PM

        while (startTime <= endTime)
        {
            timeSlots.Add(startTime);
            startTime = startTime.Add(TimeSpan.FromMinutes(150)); // Add 2:30 hours
        }

        TimeSlotPicker.ItemsSource = timeSlots.Select(t => t.ToString(@"hh\:mm")).ToList();
    }

    private async void OnCreateShowtimeClicked(object sender, EventArgs e)
    {
        // Validate selections
        var selectedMovie = MoviePicker.SelectedItem as Movie;
        var selectedHall = HallPicker.SelectedItem as Hall;
        var selectedTimeSlot = TimeSlotPicker.SelectedItem as string;

        if (selectedMovie == null || selectedHall == null || string.IsNullOrWhiteSpace(selectedTimeSlot))
        {
            await DisplayAlert("Error", "Please select a movie, hall, and time slot.", "OK");
            return;
        }

        var showtimeDateTime = ShowtimeDatePicker.Date + TimeSpan.Parse(selectedTimeSlot);

        // Check for duplicate showtime in the same hall at the same time
        var conflictingShowtime = App.Database.Table<Showtime>()
            .FirstOrDefault(s => s.HallName == selectedHall.Name && s.DateTime == showtimeDateTime);

        if (conflictingShowtime != null)
        {
            await DisplayAlert("Error", $"A showtime already exists in {selectedHall.Name} at {selectedTimeSlot}.", "OK");
            return;
        }

        // Create a new showtime object
        var showtime = new Showtime
        {
            MovieId = selectedMovie.Id,
            HallName = selectedHall.Name,
            DateTime = showtimeDateTime
        };

        // Insert the new showtime into the database
        App.Database.Insert(showtime);
        await DisplayAlert("Success", "Showtime created successfully!", "OK");
        await Navigation.PopAsync();
    }
}
