using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class AddHallPage : ContentPage
{
	public AddHallPage()
	{
		InitializeComponent();
	}
    private async void OnAddHallClicked(object sender, EventArgs e)
    {
        var name = HallNameEntry.Text;
        int capacity;
        var layout = LayoutEditor.Text;

        if (!string.IsNullOrWhiteSpace(name) && int.TryParse(CapacityEntry.Text, out capacity) && !string.IsNullOrWhiteSpace(layout))
        {
            var hall = new Hall
            {
                Name = name,
                Capacity = capacity,
                Layout = layout
            };

            App.Database.Insert(hall);
            await DisplayAlert("Success", "Hall added successfully!", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Please fill out all fields correctly.", "OK");
        }
    }
}