using SkinCareTracker.Services.Database;

namespace SkinCareTracker
{
    public partial class App : Application
    {
        private readonly DatabaseService _databaseService;

        public App(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            // DEBUG: Print the database path
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "skincare.db");
            System.Diagnostics.Debug.WriteLine($"Database path: {dbPath}");
            System.Diagnostics.Debug.WriteLine($"Directory exists: {Directory.Exists(FileSystem.AppDataDirectory)}");

            try
            {
                await _databaseService.InitializeDatabaseAsync();
                System.Diagnostics.Debug.WriteLine("Database initialized successfully");

                // Check if file was created
                System.Diagnostics.Debug.WriteLine($"Database file exists: {File.Exists(dbPath)}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
            }
        }
    }
}