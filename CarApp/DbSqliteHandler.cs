using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace CarApp
{
    /// <summary>
    /// Handles the SQLite database.
    /// </summary>
    public class DbSqliteHandler
    {
        private readonly string _connectionString; // Connection string for SQLite

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSqliteHandler"/> class.
        /// </summary>
        /// <param name="dbPath">The path to the SQLite database file.</param>
        public DbSqliteHandler(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}"; // Connection string for SQLite
            InitializeDatabase(dbPath); // Initialize the database
        }

        /// <summary>
        /// Initializes the database by creating it if it doesn't exist and applying migrations.
        /// </summary>
        /// <param name="dbPath">The path to the SQLite database file.</param>
        private void InitializeDatabase(string dbPath)
        {
            // Create database if it doesn't exist
            if (!File.Exists(dbPath))
            {
                using (var connection = new SqliteConnection(_connectionString)) // Create a new connection
                {
                    connection.Open(); // Open the connection
                    CreateDb(connection); // Create the database
                } // Close the connection
            }

            // Apply migrations
            using (var connection = new SqliteConnection(_connectionString)) // Create a new connection
            {
                connection.Open(); // Open the connection
                CreateMigrationVersionTable(connection); // Create the MigrationVersion table
                ApplyMigrations(connection); // Apply migrations
            } // Close the connection
        }

        /// <summary>
        /// Creates the database using the SQL script specified in the Constants.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        private static void CreateDb(IDbConnection connection) // IDbConnection is used to support multiple database providers
        {
            var sql = File.ReadAllText(Constants.dbSqliteCreateDbFileName); // Read the SQL script from the file
            connection.Execute(sql); // Execute the SQL script
        } // Close the connection


        /// <summary>
        /// Creates the MigrationVersion table if it doesn't exist.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        private static void CreateMigrationVersionTable(IDbConnection connection)
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS MigrationVersion (
                    Id INTEGER PRIMARY KEY,
                    Version INTEGER NOT NULL
                );
                INSERT INTO MigrationVersion (Id, Version)
                SELECT 1, 0
                WHERE NOT EXISTS (SELECT 1 FROM MigrationVersion WHERE Id = 1);
            "; // SQL script to create the MigrationVersion table
            connection.Execute(sql); // Execute the SQL script
        }

        /// <summary>
        /// Gets the current version of the database.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <returns>The current version of the database.</returns>
        private static int GetCurrentVersion(IDbConnection connection)
        {
            var sql = "SELECT Version FROM MigrationVersion WHERE Id = 1";
            return connection.QuerySingle<int>(sql);
        }

        /// <summary>
        /// Updates the current version of the database.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="version">The new version of the database.</param>
        private static void UpdateCurrentVersion(IDbConnection connection, int version)
        {
            var sql = "UPDATE MigrationVersion SET Version = @Version WHERE Id = 1";
            connection.Execute(sql, new { Version = version });
        }

        /// <summary>
        /// Applies the necessary migrations to the database.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        private void ApplyMigrations(IDbConnection connection)
        {
            var currentVersion = GetCurrentVersion(connection);
            var migrationFiles = GetMigrationFiles();

            foreach (var file in migrationFiles)
            {
                var version = int.Parse(Path.GetFileNameWithoutExtension(file).Split('.').Last());
                if (version > currentVersion)
                {
                    var sql = File.ReadAllText(file);
                    connection.Execute(sql);
                    UpdateCurrentVersion(connection, version);
                }
            }
        }

        /// <summary>
        /// Gets the list of migration files.
        /// </summary>
        /// <returns>The list of migration files.</returns>
        private IEnumerable<string> GetMigrationFiles()
        {
            var migrationFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "CreateDatabase.*.sql")
                                          .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f).Split('.').Last()))
                                          .ToList();
            return migrationFiles;
        }

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public IDbConnection Connection => new SqliteConnection(_connectionString);

        /// <summary>
        /// Gets the list of cars from the database.
        /// </summary>
        /// <returns>The list of cars.</returns>
        public IEnumerable<Car> GetCars()
        {
            using (var connection = Connection)
            {
                return connection.Query<Car>("SELECT * FROM Cars");
            }
        }

        /// <summary>
        /// Gets the list of fuel types from the database.
        /// </summary>
        /// <returns>The list of fuel types.</returns>
        public IEnumerable<FuelType> GetFuelTypes()
        {
            using (var connection = Connection)
            {
                return connection.Query<FuelType>("SELECT * FROM FuelTypes ORDER BY Name");
            }
        }

        /// <summary>
        /// Gets a fuel type by its ID.
        /// </summary>
        /// <param name="id">The ID of the fuel type.</param>
        /// <returns>The fuel type with the specified ID.</returns>
        public FuelType? GetFuelType(int id)
        {
            using (var connection = Connection)
            {
                return connection.QueryFirstOrDefault<FuelType>("SELECT * FROM FuelTypes WHERE Id = @Id", new { Id = id });
            }
        }

        /// <summary>
        /// Adds a new car to the database.
        /// </summary>
        /// <param name="car">The car to add.</param>
        public void AddCar(Car car)
        {
            using (var connection = Connection)
            {
                var sql = "INSERT INTO Cars (Brand, Model, Year, GearType, FuelTypeId, FuelEfficiency, Mileage, Description) " +
                          "VALUES (@Brand, @Model, @Year, @GearType, @FuelTypeId, @FuelEfficiency, @Mileage, @Description)";
                connection.Execute(sql, car);
            }
        }

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="car">The car to delete.</param>
        public void DeleteCar(Car car)
        {
            using (var connection = Connection)
            {
                var sql = "DELETE FROM Cars WHERE Id = @Id";
                connection.Execute(sql, new { Id = car.Id });
            }
        }

        /// <summary>
        /// Updates the details of an existing car in the database.
        /// </summary>
        /// <param name="car">The car object containing updated details.</param>
        public void UpdateCar(Car car)
        {
            using (var connection = Connection)
            {
                var sql = "UPDATE Cars SET Brand = @Brand, Model = @Model, Year = @Year, GearType = @GearType, " +
                          "FuelTypeId = @FuelTypeId, FuelEfficiency = @FuelEfficiency, Mileage = @Mileage, Description = @Description " +
                          "WHERE Id = @Id";
                connection.Execute(sql, car);
            }
        }

        /// <summary>
        /// Adds a new fuel type to the database.
        /// </summary>
        /// <param name="fuelType">The fuel type to add.</param>
        public void AddFuelType(FuelType fuelType)
        {
            using (var connection = Connection)
            {
                var sql = "INSERT INTO FuelTypes (Id, Name, Price) VALUES (@Id, @Name, @Price)";
                Console.WriteLine(sql);
                connection.Execute(sql, fuelType);
            }
        }

        /// <summary>
        /// Imports data from a JSON file into the database.
        /// </summary>
        public void ImportFromJson()
        {
            using (var connection = Connection)
            {
                var sql = "DELETE FROM Cars";
                connection.Execute(sql);
                Console.WriteLine("Cars table cleared.");

                sql = "DELETE FROM FuelTypes";
                connection.Execute(sql);
                Console.WriteLine("FuelTypes table cleared.");
            }

            JsonFileHandler jsonFileHandler = new JsonFileHandler();
            JsonFileHandler.DataContainer? data = jsonFileHandler.ImportData(Constants.jsonFileName);

            if (data != null)
            {
                // FuelTypes must come before Cars because of foreign keys in db
                foreach (var fuelType in data.FuelTypes)
                {
                    AddFuelType(fuelType);
                }
                Console.WriteLine("Fuel types imported.");
                foreach (var car in data.Cars)
                {
                    AddCar(car);
                }
                Console.WriteLine("Cars imported.");
            }
            else
            {
                Console.WriteLine("No data imported.");
                Console.WriteLine("Tryk på en tast for at fortsætte...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Exports the data from the database to a JSON file.
        /// </summary>
        public void ExportToJson()
        {
            JsonFileHandler jsonFileHandler = new JsonFileHandler();
            var cars = Program.DbSqlHandler.GetCars().ToList();
            var fuelTypes = Program.DbSqlHandler.GetFuelTypes().ToList();
            JsonFileHandler.DataContainer dataContainer = new JsonFileHandler.DataContainer
            {
                Cars = cars,
                FuelTypes = fuelTypes
            };

            jsonFileHandler.ExportData(Constants.jsonFileName, dataContainer);
        }
    }
}