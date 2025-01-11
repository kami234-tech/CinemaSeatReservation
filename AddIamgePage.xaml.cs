using Microsoft.Maui.Storage;
namespace CinemaSeatReservation;

public partial class AddIamgePage : ContentPage
{
	public AddIamgePage()
	{
		InitializeComponent();
	}
    private async void OnSelectImageClicked(object sender, EventArgs e)
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
                // Display the selected image
                SelectedImage.Source = ImageSource.FromFile(result.FullPath);

                // Save the path to the database (optional)
                // This can be integrated with the movie addition logic
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // Navigate back to AdminPage
    }
}