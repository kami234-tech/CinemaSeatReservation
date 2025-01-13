using CinemaSeatReservation.Models;
using Microsoft.Maui.Storage;
namespace CinemaSeatReservation;

public partial class CreateMoviePage : ContentPage
{
    private string _selectedPhotoPath;
    public CreateMoviePage()
	{
		InitializeComponent();
	}
    private async void OnUploadPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select a Movie Image"
            });

            if (result != null)
            {
                _selectedPhotoPath = result.FullPath;
                SelectedImage.Source = ImageSource.FromFile(_selectedPhotoPath);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while selecting the photo: {ex.Message}", "OK");
        }
    }

    private async void OnAddMovieClicked(object sender, EventArgs e)
    {
        try
        {
            var title = TitleEntry.Text;
            var genre = GenreEntry.Text;
            var description = DescriptionEditor.Text;
            int duration;

            Console.WriteLine("Validating input...");
            Console.WriteLine($"Title: {title}, Genre: {genre}, Duration: {DurationEntry.Text}, PhotoPath: {_selectedPhotoPath}");

            if (int.TryParse(DurationEntry.Text, out duration) &&
                !string.IsNullOrWhiteSpace(title) &&
                !string.IsNullOrWhiteSpace(genre) &&
                !string.IsNullOrWhiteSpace(_selectedPhotoPath))
            {
                var movie = new Movie
                {
                    Title = title,
                    Genre = genre,
                    Duration = duration,
                    Description = description,
                    PhotoPath = _selectedPhotoPath
                };

                Console.WriteLine("Inserting movie into database...");
                App.Database.Insert(movie);

                await DisplayAlert("Success", "Movie added successfully!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                Console.WriteLine("Validation failed.");
                ErrorLabel.Text = "Please fill out all fields and upload a photo.";
                ErrorLabel.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            await DisplayAlert("Error", $"Failed to add movie: {ex.Message}", "OK");
        }
    }
}
