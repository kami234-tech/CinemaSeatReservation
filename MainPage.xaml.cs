namespace CinemaSeatReservation
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during MainPage initialization: {ex.Message}");
            }


        }
    }
}
