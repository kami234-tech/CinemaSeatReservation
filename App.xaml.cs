using System.IO;
using SQLite;
using CinemaSeatReservation.Models;
namespace CinemaSeatReservation
{
    public partial class App : Application
    {
        public static SQLiteConnection Database { get; private set; }
        public App()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cinema.db");
            Database = new SQLiteConnection(dbPath);

            // Create tables
            Database.CreateTable<User>();
            Database.CreateTable<Movie>();
            Database.CreateTable<Reservation>();

            SeedData();

            MainPage = new NavigationPage(new LoginPage());
        }

        private void SeedData()
        {
            try
            {
                // Check if users already exist
                if (Database.Table<User>().Count() == 0)
                {
                    // Insert default admin and user
                    Database.Insert(new User { Name = "Admin", Email = "admin@cinema.com", Password = "admin", Role = "admin" });
                    Database.Insert(new User { Name = "User", Email = "user@cinema.com", Password = "user", Role = "user" });
                }
            }
            catch (Exception ex)
            {
                // Log the error to console or debug output
                Console.WriteLine($"Error during SeedData: {ex.Message}");
            }
        }
    }
}
