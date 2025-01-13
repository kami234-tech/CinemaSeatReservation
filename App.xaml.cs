using System.IO;
using SQLite;
using CinemaSeatReservation.Models;
namespace CinemaSeatReservation
{
    public partial class App : Application
    {
        public static User? CurrentUser { get; set; } = null; // Allow nullable or provide a default value
        public static SQLiteConnection? Database { get; set; } = null; // Allow nullable or initialize elsewhere

        public App()
        {
            InitializeComponent();
            SQLitePCL.Batteries_V2.Init();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cinema.db");

            // Initialize the SQLite connection
            Database = new SQLiteConnection(dbPath);
            Console.WriteLine($"Database Path: {dbPath}");
            // Create other tables
            Database.CreateTable<User>();
            Database.CreateTable<Movie>();
            Database.CreateTable<Showtime>();
            Database.CreateTable<Reservation>();
            Database.CreateTable<Hall>();


            SeedData();

            MainPage = new NavigationPage(new LoginPage());
        }

        private void SeedData()
        {
            try
            {
                // Check if users already exist before inserting default data
                if (!Database.Table<User>().Any())
                {
                    Database.Insert(new User { Name = "Admin", Email = "admin@cinema.com", Password = "admin", Role = "admin" });
                    Database.Insert(new User { Name = "User", Email = "user@cinema.com", Password = "user", Role = "user" });
                    Console.WriteLine("Default users added.");
                }
                else
                {
                    Console.WriteLine("Default users already exist. No need to add.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during SeedData: {ex.Message}");
            }
        }
    }
}
