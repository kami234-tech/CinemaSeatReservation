namespace CinemaSeatReservation;
using CinemaSeatReservation.Models;
public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text.ToLower(); // Normalize email to lowercase
        string password = PasswordEntry.Text;

        // Fetch user by email and password
        var user = App.Database.Table<User>().FirstOrDefault(u => u.Email.ToLower() == email && u.Password == password);

        if (user != null)
        {
            if (user.Role == "admin")
            {
                await Navigation.PushAsync(new AdminPage());
            }
            else
            {
                await Navigation.PushAsync(new UserPage(user));
            }
        }
        else
        {
            await DisplayAlert("Error", "Invalid email or password.", "OK");
        }
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}