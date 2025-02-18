using System.Text;

namespace CarApp
{
    internal class Program
    {
        public static DbSqliteHandler DbSqlHandler = new DbSqliteHandler(Constants.dbSqliteFileName);


        // Car methods

        /// <summary>
        /// Prompts the user to input car information and returns the car object.
        /// </summary>
        /// <returns>The populated car object.</returns>
        static Car InputCar()
        {
            Car car = new(); // Create a new car object

            IEnumerable<FuelType> fuelTypes = Program.DbSqlHandler.GetFuelTypes(); // Get the fuel types from the database

            char gearType; // Gear type as a character

            Console.Clear(); // Clear the console window

            Console.WriteLine("Tilføj bil");
            Console.WriteLine("==========");
            Console.Write("Indtast bilmærke: ");
            car.Brand = Console.ReadLine() ?? string.Empty;
            Console.Write("Indtast bilmodel: ");
            car.Model = Console.ReadLine() ?? string.Empty;
            Console.Write("Indtast årgang: ");
            car.Year = Convert.ToInt32(Console.ReadLine());
            do
            {
                Console.Write("Indtast geartype ([A]utomatisk/[M]anuel): ");
                gearType = char.ToUpper(Convert.ToChar(Console.Read())); // Read a character and convert it to uppercase
                Console.ReadLine();
            } while (gearType != 'A' && gearType != 'M'); // Repeat until a valid gear type is entered
            car.GearType = gearType;

            Console.WriteLine();
            Console.WriteLine("Brændstoftyper");
            Console.WriteLine("==============");

            for (int i = 0; i < fuelTypes.Count(); i++) // Loop through the fuel types and display them
            {
                Console.WriteLine($"{i + 1}. {fuelTypes.ElementAt(i).Name}");
            }

            int fuelTypeIndex;
            do // Repeat until a valid fuel type is entered
            {
                Console.Write("Vælg brændstoftype: ");
                fuelTypeIndex = Convert.ToInt32(Console.ReadLine()) - 1;
            } while (fuelTypeIndex < 0 || fuelTypeIndex >= fuelTypes.Count());
            car.FuelTypeId = fuelTypes.ElementAt(fuelTypeIndex).Id;

            Console.Write("Indtast forbrug: ");
            car.FuelEfficiency = Convert.ToSingle(Console.ReadLine());
            Console.Write("Indtast kilometerstand: ");
            car.Mileage = Convert.ToInt32(Console.ReadLine());

            return car; // Return the car object
        }

        /// <summary>
        /// Displays a list of cars and allows the user to choose one.
        /// </summary>
        /// <returns>The chosen car object.</returns>
        static Car? SelectCar()
        {
            List<Car> cars = Program.DbSqlHandler.GetCars().ToList(); // Get the cars from the database
            List<int> columns = new() { 3, 20, 20, 20 }; // number of columns and their width
            System.ConsoleKeyInfo choice; // The user's choice
            int elementCounter = 0;
            int i;

            do
            {
                Console.Clear();
                Console.WriteLine("Vælg bil");
                Console.WriteLine("========");
                Console.WriteLine();

                // Create Table for console
                CreateTableFrameH(columns); // Create a horizontal table frame
                Console.WriteLine(
                    "| " + "#".PadLeft(columns[0]) +
                    " | " + CenterString("Mærke", columns[1]) +
                    " | " + CenterString("Model", columns[2]) +
                    " | " + CenterString("Kilemetertal", columns[3]) +
                    " |");
                CreateTableFrameH(columns); // Create a horizontal table frame

                for (i = 1 + elementCounter; i < cars.Count + 1; i++)
                {
                    if (i > elementCounter + 10)
                    {
                        break;
                    }
                    int ii = i - 1;

                    string option = $"F{(i - elementCounter)}";
                    Console.WriteLine(
                        "| " + $"{option.PadLeft(columns[0])}" +
                        " | " + $"{cars[ii].Brand}".PadRight(columns[1]) +
                        " | " + $"{cars[ii].Model}".PadRight(columns[2]) +
                        " | " + $"{cars[ii].Mileage}".PadLeft(columns[3]) +
                        " |");
                }


                CreateTableFrameH(columns); // Create a horizontal table frame
                Console.WriteLine();
                if (elementCounter > 0)
                {
                    Console.WriteLine("F11: Forrige side ");
                }
                if (elementCounter + 10 < cars.Count - 1)
                {
                    Console.WriteLine("F12: Næste side ");
                }
                Console.WriteLine("ESC: Afslut");
                choice = Console.ReadKey();

                switch (choice.Key)
                {
                    case ConsoleKey.F1:
                        return cars[elementCounter];
                    case ConsoleKey.F2:
                        if (elementCounter + 1 < cars.Count)
                        {
                            return cars[elementCounter + 1];
                        }
                        break;
                    case ConsoleKey.F3:
                        if (elementCounter + 2 < cars.Count)
                        {
                            return cars[elementCounter + 2];
                        }
                        break;
                    case ConsoleKey.F4:
                        if (elementCounter + 3 < cars.Count)
                        {
                            return cars[elementCounter + 3];
                        }
                        break;
                    case ConsoleKey.F5:
                        if (elementCounter + 4 < cars.Count)
                        {
                            return cars[elementCounter + 4];
                        }
                        break;
                    case ConsoleKey.F6:
                        if (elementCounter + 5 < cars.Count)
                        {
                            return cars[elementCounter + 5];
                        }
                        break;
                    case ConsoleKey.F7:
                        if (elementCounter + 6 < cars.Count)
                        {
                            return cars[elementCounter + 6];
                        }
                        break;
                    case ConsoleKey.F8:
                        if (elementCounter + 7 < cars.Count)
                        {
                            return cars[elementCounter + 7];
                        }
                        break;
                    case ConsoleKey.F9:
                        if (elementCounter + 8 < cars.Count)
                        {
                            return cars[elementCounter + 8];
                        }
                        break;
                    case ConsoleKey.F10:
                        if (elementCounter + 9 < cars.Count)
                        {
                            return cars[elementCounter + 9];
                        }
                        break;
                    case ConsoleKey.F11:
                        if (elementCounter > 0)
                        {
                            elementCounter -= 10;
                        }
                        break;
                    case ConsoleKey.F12:
                        if (elementCounter + 10 < cars.Count)
                        {
                            elementCounter += 10;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return null;
                    default:
                        Console.WriteLine("Ugyldigt valg.");
                        Console.WriteLine("Tast for at forsætte.");
                        Console.ReadKey();
                        break;
                }
            } while (true);
        }

        /// <summary>
        /// Displays a report of the car's information.
        /// </summary>
        /// <param name="car">The car object to display the report for.</param>
        static void PrintCarDetails(Car car)
        {
            IEnumerable<FuelType> fuelTypes = DbSqlHandler.GetFuelTypes(); // Get the fuel types from the database

            Console.Clear();
            Console.WriteLine("Bilrapport");
            Console.WriteLine("==========");
            Console.WriteLine();
            Console.WriteLine("Biler");
            Console.WriteLine("=====");
            Console.WriteLine(
                $"Bilmærke: {car.Brand}" + "\n" +
                $"Bilmodel: {car.Model}" + "\n" +
                $"Årgang: {car.Year}" + "\n" +
                $"Gear: {car.GearType}" + "\n" +
                $"Brændstof: {fuelTypes.First(ft => ft.Id == car.FuelTypeId).Name}" + "\n" +
                $"Forbrug: {car.FuelEfficiency} km/l" + "\n" +
                $"Kilometerstand: {car.Mileage}" +
                (Car.IsPalindrome(car) ? " ** Palindrome nummer **" : "") + "\n" + // Check if the mileage is a palindrome
                $"Beskrivelse: {car.Description}" + "\n" +
                (car.IsEngineRunning ? "Bilen er tændt" : "Bilen er slukket") + "\n"
            );

            Console.WriteLine();
            Console.WriteLine("\nTryk på en tast for at fortsætte...");
            Console.ReadKey(); // Wait for a key press
        }


        // Table methods

        /// <summary>
        /// Creates a horizontal table frame for the console output.
        /// </summary>
        /// <param name="columns">A list of integers representing the width of each column.</param>
        static void CreateTableFrameH(List<int> columns)
        {
            Console.Write("+");
            foreach (int column in columns)
            {
                Console.Write(new string('-', column + 2));
                Console.Write("+");
            }
            Console.WriteLine();
        }

        // String methods

        /// <summary>
        /// Centers the given text within a specified width.
        /// </summary>
        /// <param name="text">The text to center.</param>
        /// <param name="width">The width within which to center the text.</param>
        /// <returns>The centered text with padding.</returns>
        static string CenterString(string text, int width)
        {
            if (width <= text.Length)
            {
                return text; // Or throw an exception, or truncate the string
            }

            int padding = width - text.Length;
            int leftPadding = padding / 2;
            int rightPadding = padding - leftPadding;

            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }

        // Menu methods

        /// <summary>
        /// Displays the menu and handles user input.
        /// </summary>
        private static void Menu()
        {
            // Declare variables
            Car? car = null; // Create a car object
            System.ConsoleKeyInfo choice; // The user's choice
            IEnumerable<FuelType> fuelTypes = DbSqlHandler.GetFuelTypes(); // Get the fuel types from the database

            // Loop until the user exits the program
            do
            {
                Console.Clear(); // Clear the console window
                Console.WriteLine("Menu");
                Console.WriteLine("====");
                Console.WriteLine("F1: Tilføj en bil...");
                Console.WriteLine("F2: Slet en bil...");
                Console.WriteLine("F3: Vælg bil...");
                if (car != null) // If a car is selected
                {
                    Console.WriteLine("F4: Beregn tur omkostning...");
                    Console.WriteLine($"F5: Vis {car.Brand} {car.Model} detajler");
                    Console.WriteLine($"F6: Tjek om {car.Brand} {car.Model} kilometerstanden er et palindrom");
                } // End if a car is selected
                Console.WriteLine("F7: Database Menu...");
                Console.WriteLine("ESC: Afslut");
                Console.WriteLine();

                choice = Console.ReadKey(); // Wait for a key press

                switch (choice.Key) // Switch on the user's choice
                {
                    case ConsoleKey.F1: // If the user pressed F1
                        car = InputCar(); // Pass a new Car object to InputCar
                        DbSqlHandler.AddCar(car); // Add the car to the database
                        break;
                    case ConsoleKey.F2:
                        car = SelectCar();
                        if (car != null)
                        {
                            DbSqlHandler.DeleteCar(car); // Delete the car from the database
                        }
                        break;
                    case ConsoleKey.F3:
                        car = SelectCar();
                        break;
                    case ConsoleKey.F4:
                        if (car != null)
                        {
                            car.CalculateTour(car.Mileage);
                            if (car.IsEngineRunning)
                            {
                                DbSqlHandler.UpdateCar(car); // Update the car in the database
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ingen bil valgt. Tilføj eller vælg en bil først.");
                            Console.ReadKey();
                        }
                        break;
                    case ConsoleKey.F5:
                        if (car != null)
                        {
                            PrintCarDetails(car);
                        }
                        else
                        {
                            Console.WriteLine("Ingen bil valgt. Tilføj eller vælg en bil først.");
                            Console.WriteLine("\nTryk på en tast for at fortsætte...");
                            Console.ReadKey(); // Wait for a key press
                        }
                        break;
                    case ConsoleKey.F6:
                        if (car != null)
                        {
                            Console.WriteLine(Car.IsPalindrome(car) ? "Kilometertallet er et palindrom." : "Kilometertallet er ikke et palindrom.");
                            Console.WriteLine("\nTryk på en tast for at fortsætte...");
                            Console.ReadKey(); // Wait for a key press
                        }
                        else
                        {
                            Console.WriteLine("Ingen bil valgt. Tilføj eller vælg en bil først.");
                            Console.WriteLine("\nTryk på en tast for at fortsætte...");
                            Console.ReadKey(); // Wait for a key press
                        }
                        break;
                    case ConsoleKey.F7:
                        MenuDatabase();
                        break;
                    case ConsoleKey.Escape: // If the user pressed ESC
                        return; // Exit the method (and the program)
                    default:
                        Console.WriteLine("Ugyldigt valg.");
                        Console.WriteLine("\nTryk på en tast for at fortsætte...");
                        Console.ReadKey(); // Wait for a key press
                        break;
                }
            } while (true); // End loop
        }

        /// <summary>
        /// Displays the database menu and handles user input.
        /// </summary>
        static void MenuDatabase()
        {
            do // Loop until the user exits the database menu
            {
                Console.Clear(); // Clear the console window
                Console.WriteLine("Database Menu");
                Console.WriteLine("=============");
                Console.WriteLine("F1: Import json to database (It will clear all existing data in db");
                Console.WriteLine("F2: Export database to json");
                // TODO Console.WriteLine("F3: Clear database");
                Console.WriteLine("ESC: Afslut");
                ConsoleKeyInfo choice = Console.ReadKey(); // Wait for a key press
                switch (choice.Key)
                {
                    case ConsoleKey.F1:
                        DbSqlHandler.ImportFromJson();
                        break;
                    case ConsoleKey.F2:
                        DbSqlHandler.ExportToJson();
                        break;
                    /* TODO
                case ConsoleKey.F3:
                Globals.DbSqlHandler.ClearDatabase();
                break;
                    */
                    case ConsoleKey.Escape:
                        Console.WriteLine("a"); //BUG: If we don not write anything in the case, it will loose first character in Menu 
                        return; // Exit the method
                    default:
                        Console.WriteLine("Ugyldigt valg.");
                        Console.WriteLine("Tast for at forsætte.");
                        Console.ReadKey();
                        break;
                }
            } while (true);
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(/* string[] args */)
        {
            // Set the console output encoding to UTF-8 so æøå are displayed correctly
            Console.OutputEncoding = Encoding.UTF8;

            Menu();
        }
    }
}
