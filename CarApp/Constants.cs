namespace CarApp
{
    /// <summary>  
    /// Provides constant values used throughout the CarApp application.  
    /// </summary>  
    public static class Constants
    {
        /// <summary>  
        /// The name of the JSON file used for storing application data.  
        /// </summary>  
        public const string jsonFileName = "CarAppData.json";

        /// <summary>  
        /// The name of the SQLite database file.  
        /// </summary>  
        public const string dbSqliteFileName = "CarApp.db";

        /// <summary>  
        /// The name of the SQL file used for creating the SQLite database schema.  
        /// </summary>  
        public const string dbSqliteCreateDbFileName = "CreateDatabase.sql";
    }
}