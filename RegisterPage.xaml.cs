using CinemaSeatReservation.Models;
namespace CinemaSeatReservation;

public partial class RegisterPage : ContentPage
{
    private const string AdminSecretPassword = "CinemaCity";
    public RegisterPage()
	{
		InitializeComponent();
	}
    private void OnRoleChanged(object sender, EventArgs e)
    {
        // Show or hide the SecretPasswordEntry based on the selected role
        var selectedRole = RolePicker.SelectedItem?.ToString();
        SecretPasswordEntry.IsVisible = selectedRole == "admin";
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var name = NameEntry.Text;
        var email = EmailEntry.Text;
        var password = PasswordEntry.Text;
        var role = RolePicker.SelectedItem?.ToString();
        var secretPassword = SecretPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
        {
            ErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Please fill out all fields.";
            return;
        }

        // Validate secret password for admin role
        if (role == "admin" && secretPassword != AdminSecretPassword)
        {
            ErrorLabel.IsVisible = true;
            ErrorLabel.Text = "Invalid secret password for admin role.";
            return;
        }

        // Check if the email already exists
        var existingUser = App.Database.Table<User>().FirstOrDefault(u => u.Email == email);
        if (existingUser != null)
        {
            ErrorLabel.IsVisible = true;
            ErrorLabel.Text = "An account with this email already exists.";
            return;
        }

        // Create and insert the new user
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