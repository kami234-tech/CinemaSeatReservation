using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class ViewReservationsPage : ContentPage
{
	public ViewReservationsPage()
	{
		InitializeComponent();
        LoadReservations();
    }
    private void LoadReservations()
    {
        ReservationsListView.ItemsSource = App.Database.Table<Reservation>().ToList();
    }
}