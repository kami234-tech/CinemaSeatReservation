using CinemaSeatReservation.Models;
using System.IO;

namespace CinemaSeatReservation;

public partial class AddHallPage : ContentPage
{
    private string _defaultLayout;

    public AddHallPage()
    {
        InitializeComponent();
        LoadDefaultLayout();
    }

    private void LoadDefaultLayout()
    {
        try
        {
            var assembly = typeof(App).Assembly;
            var resourceName = "CinemaSeatReservation.Resources.Raw.DefaultHallLayout.json";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException("Default hall layout could not be found.");
            }

            using var reader = new StreamReader(stream);
            _defaultLayout = reader.ReadToEnd();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading default layout: {ex.Message}");
            _defaultLayout = string.Empty;
        }
    }

    private async void OnAddHallClicked(object sender, EventArgs e)
    {
        // Validate input fields
        var hallName = HallNameEntry.Text;
        if (string.IsNullOrWhiteSpace(hallName) || !int.TryParse(CapacityEntry.Text, out int capacity))
        {
            await DisplayAlert("Error", "Please fill in all fields correctly.", "OK");
            return;
        }

        // Check if "Use Default Layout" is checked
        if (!UseDefaultLayoutCheckbox.IsChecked)
        {
            await DisplayAlert("Error", "Please check the 'Use Default Layout' option to proceed.", "OK");
            return;
        }

        // Use the default layout directly
        if (string.IsNullOrWhiteSpace(_defaultLayout))
        {
            await DisplayAlert("Error", "Default layout could not be loaded.", "OK");
            return;
        }

        // Create the hall object
        var hall = new Hall
        {
            Name = hallName,
            Capacity = capacity,
            Layout = _defaultLayout
        };

        // Save the hall to the database
        try
        {
            App.Database.Insert(hall);
            await DisplayAlert("Success", "Hall added successfully!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while saving the hall: {ex.Message}", "OK");
        }
    }
}
