namespace CarApp
{
    /// <summary>
    /// Represents a type of fuel with a name and price.
    /// </summary>
    public class FuelType
    {
        /// <summary>
        /// Gets or sets the ID of the fuel type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the fuel type.
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the price of the fuel type.
        /// </summary>
        public decimal Price { get; set; } = 0;
    }

    /// <summary>
    /// Represents a car with various properties.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Gets and sets the Id of the car.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the brand of the car.
        /// </summary>
        public string Brand { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the model of the car.
        /// </summary>
        public string Model { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the year of the car.
        /// </summary>
        public int Year { get; set; } = 0;

        /// <summary>
        /// Gets or sets the gear type of the car.
        /// </summary>
        public char GearType { get; set; } = 'M';

        public int? FuelTypeId { get; set; } // Foreign Key
        public FuelType? FuelType { get; set; } // Navigation property

        /// <summary>
        /// Gets or sets the fuel efficiency of the car.
        /// </summary>
        public float FuelEfficiency { get; set; } = 0;

        /// <summary>
        /// Gets or sets the mileage of the car.
        /// </summary>
        public int Mileage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the description of the car.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        // Non database properties

        public bool IsEngineRunning { get; set; } = false;

        public void StartEngine()
        {
            IsEngineRunning = true;
        }

        public void StopEngine()
        {
            IsEngineRunning = false;
        }

        /// <summary>
        /// Adds a tour distance to the car's mileage if engine is running else it will simulate tour.
        /// </summary>
        /// <param name="km">The distance of the tour in kilometers.</param>
        public void CalculateTour(int km)
        {
            if (IsEngineRunning)
            {
                Mileage += km;
            }
        }

        /// <summary>
        /// Calculates the amount of fuel needed for a given distance.
        /// </summary>
        /// <param name="car">The car object containing fuel efficiency information.</param>
        /// <param name="distance">The distance to be traveled in kilometers.</param>
        /// <returns>The amount of fuel needed for the given distance.</returns>
        public static double CalculateFuelNeeded(Car car, int distance)
        {
            return (double)(distance / car.FuelEfficiency);
        }

        /// <summary>
        /// Calculates the trip cost based on the fuel needed and the fuel price.
        /// </summary>
        /// <param name="car">The car object containing the fuel type information.</param>
        /// <param name="fuelNeeded">The amount of fuel needed for the trip.</param>
        /// <returns>The cost of the trip.</returns>
        public static decimal CalculateTripCost(Car car, double fuelNeeded)
        {
            IEnumerable<FuelType> fuelTypes = Program.DbSqlHandler.GetFuelTypes(); // Get the fuel types from the database
            return (decimal)(fuelNeeded * (float)fuelTypes.First(ft => ft.Id == car.FuelTypeId).Price);
        }

        /// <summary>
        /// Checks if a car's mileage is a palindrome.
        /// </summary>
        /// <param name="car">The car object containing the mileage information.</param>
        /// <returns>True if the mileage is a palindrome, otherwise false.</returns>
        public static bool IsPalindrome(Car car)
        {
            string odometer = car.Mileage.ToString();

            for (int i = 0; i < odometer.Length / 2; i++)
            {
                if (odometer[i] != odometer[odometer.Length - i - 1])
                {
                    return false;
                }
            }
            return true;
        }
    }
}