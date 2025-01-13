using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class ViewHallsPage : ContentPage
{
	public ViewHallsPage()
	{
		InitializeComponent();
        LoadHalls();

    }
    private void LoadHalls()
    {
        try
        {
            // Fetch halls from the database
            var halls = App.Database.Table<Hall>().ToList();

            // Assign halls to the ListView
            HallsListView.ItemsSource = halls;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Failed to load halls: {ex.Message}", "OK");
        }
    }

    private async void OnHallSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Hall selectedHall)
        {
            bool confirm = await DisplayAlert(
                "Delete Hall",
                $"Are you sure you want to delete the hall '{selectedHall.Name}'?",
                "Yes",
                "No"
            );

            if (confirm)
            {
                try
                {
                    // Delete the hall from the database
                    App.Database.Delete(selectedHall);
                    await DisplayAlert("Success", "Hall deleted successfully.", "OK");

                    // Refresh the list of halls
                    LoadHalls();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete hall: {ex.Message}", "OK");
                }
            }

            // Clear the selected item to avoid re-triggering
            HallsListView.SelectedItem = null;
        }
    }
}