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
            SQLitePCL.Batteries_V2.Init();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Cinema.db");
            Console.WriteLine($"Database Path: {dbPath}");
            try
            {
                Database = new SQLiteConnection(dbPath);
                Console.WriteLine("SQLite database initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQLite initialization failed: {ex.Message}");
            }

            // Drop and recreate the User table
            Database.CreateTable<User>();

            // Create other tables
            Database.CreateTable<Movie>();
            Database.CreateTable<Showtime>();
            Database.CreateTable<Reservation>();


            SeedData();

            MainPage = new NavigationPage(new LoginPage());
        }

        private void SeedData()
        {
            try
            {
                // Drop and recreate the User table (ONLY FOR DEVELOPMENT!)
#if DEBUG
                Database.DropTable<User>(); // WARNING: Deletes all existing User data
                Database.CreateTable<User>();
#endif

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
