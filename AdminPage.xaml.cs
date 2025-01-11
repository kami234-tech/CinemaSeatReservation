using Microsoft.Maui.Storage;
using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class AdminPage : ContentPage
{
    private string _selectedPhotoPath;
    public AdminPage()
	{
		InitializeComponent();
        LoadMovies();
    }
    private void LoadMovies()
    {
        MoviesListView.ItemsSource = App.Database.Table<Movie>().ToList();
    }

    private async void OnUploadPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            // Open file picker to select an image
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select a Movie Image"
            });

            if (result != null)
            {
                _selectedPhotoPath = result.FullPath; // Save the photo path
                SelectedImage.Source = ImageSource.FromFile(_selectedPhotoPath); // Display the photo
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while selecting the photo: {ex.Message}", "OK");
        }
    }

    private async void OnAddMovieClicked(object sender, EventArgs e)
    {
        var title = TitleEntry.Text;
        var genre = GenreEntry.Text;
        var description = DescriptionEditor.Text;
        int duration;

        if (int.TryParse(DurationEntry.Text, out duration) &&
            !string.IsNullOrWhiteSpace(title) &&
            !string.IsNullOrWhiteSpace(genre) &&
            !string.IsNullOrWhiteSpace(_selectedPhotoPath)) // Ensure a photo is selected
        {
            var movie = new Movie
            {
                Title = title,
                Genre = genre,
                Duration = duration,
                Description = description,
                PhotoPath = _selectedPhotoPath // Save the photo path in the database
            };

            App.Database.Insert(movie);
            LoadMovies();

            // Clear input fields
            TitleEntry.Text = "";
            GenreEntry.Text = "";
            DurationEntry.Text = "";
            DescriptionEditor.Text = "";
            SelectedImage.Source = null;
            _selectedPhotoPath = null;

            await DisplayAlert("Success", "Movie added successfully!", "OK");
        }
        else
        {
            await DisplayAlert("Error", "Please fill out all fields and upload a photo.", "OK");
        }
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

    private async void OnCorrelateMovieToHallClicked(object sender, EventArgs e)
    {
        // Navigate to Correlate Movie to Hall Page (to be implemented)
        await Navigation.PushAsync(new CorrelateMovieToHallPage());
    }
}