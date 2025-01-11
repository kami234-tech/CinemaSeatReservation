using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text;
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;
        var role = RolePicker.SelectedItem?.ToString();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
        {
            ErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Please fill out all fields.";
            return;
        }

        var existingUser = App.Database.Table<User>().FirstOrDefault(u => u.Email == email);
        if (existingUser != null)
        {
            ErrorLabel.IsVisible = true;
            ErrorLabel.Text = "An account with this email already exists.";
            return;
        }

        var newUser = new User
        {
            Name = name,
            Email = email,
            Password = password,
            Role = role
        };

        App.Database.Insert(newUser);

        await DisplayAlert("Success", "Account registered successfully!", "OK");
        await Navigation.PopAsync(); // Go back to the Login Page
    }
}