using System.Text.Json;

namespace CarApp
{
    /// <summary>
    /// Handles JSON file operations for exporting and importing data.
    /// </summary>
    public class JsonFileHandler
    {
        /// <summary>
        /// Exports the data to a JSON file.
        /// </summary>
        /// <param name="filename">The name of the file to export the data to.</param>
        /// <param name="dataContainer">The data container holding the data to be exported.</param>
        public void ExportData(string filename, DataContainer dataContainer)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(dataContainer, options);
            File.WriteAllText(filename, jsonString);
        }

        /// <summary>
        /// Imports the data from a JSON file.
        /// </summary>
        /// <param name="filename">The name of the file to import the data from.</param>
        /// <returns>The data container holding the imported data, or null if an error occurs.</returns>
        public DataContainer? ImportData(string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    string jsonString = File.ReadAllText(filename);
                    var data = JsonSerializer.Deserialize<DataContainer>(jsonString);
                    if (data != null)
                    {
                        return data;
                    }
                }
                else
                {
                    Console.WriteLine("No data file found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing data: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Represents a container for data to be serialized or deserialized.
        /// </summary>
        public class DataContainer
        {
            /// <summary>
            /// Gets or sets the file version.
            /// </summary>
            public int? FileVersion { get; set; } = null;

            /// <summary>
            /// Gets or sets the list of cars.
            /// </summary>
            public List<Car> Cars { get; set; } = new List<Car>();

            /// <summary>
            /// Gets or sets the list of fuel types.
            /// </summary>
            public List<FuelType> FuelTypes { get; set; } = new List<FuelType>();
        }
    }
}
