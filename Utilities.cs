using FTS.AirportTicketBookingExercise.FlightManagement;
using FTS.AirportTicketBookingExercise.UserManagement;

namespace FTS.AirportTicketBookingExercise
{
    // class for console logic
    public static class Utilities
    {
        private static readonly Manager manager = new();
        private static readonly Passenger passenger = new();

        public static void ImportFlights()
        {
            Manager.UploadFlights();
        }

        public static void ShowMainMenu()
        {
            Console.WriteLine("*******************************");
            Console.WriteLine("**** Flight booking System ****");
            Console.WriteLine("*******************************");
            Console.WriteLine();

            string? userSelection;

            do
            {
                Console.WriteLine("Login as:");
                Console.WriteLine("1. Manager");
                Console.WriteLine("2. Passenger");
                Console.WriteLine("3. Exit");

                Console.Write("Your selection: ");
                userSelection = Console.ReadLine()?.Trim();

                Console.WriteLine();

                switch(userSelection)
                {
                    case "1":
                        ShowManagerDashboard();
                        break;
                    case "2":
                        ShowPassengerDashboard();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                }
            } while (userSelection != "1" || userSelection != "2");
        }

        private static void ShowManagerDashboard()
        {
            Console.WriteLine("****** Manager Operations ******");

            string? userChoice;

            do
            {
                Console.WriteLine("What would you like to do");
                Console.WriteLine("1. Filter Bookings");
                Console.WriteLine("2. Validate Flight Data");
                Console.WriteLine("3. Validate Model Details");
                Console.WriteLine("4. Return to Main Menu");
                
                Console.Write("Your Selection: ");
                userChoice = Console.ReadLine()?.Trim();
                
                Console.WriteLine();

                switch (userChoice)
                {
                    case "1":
                        throw new NotImplementedException();
                    case "2":
                        throw new NotImplementedException();
                    case "3":
                        throw new NotImplementedException();
                    case "4":
                        return;
                }
            }
            while (userChoice != "4");
        }

        private static void ShowPassengerDashboard()
        {
            Console.WriteLine("***** Passenger Operations *****");

            string? userChoice;

            do
            {
                Console.WriteLine("1. Book a Flight");
                Console.WriteLine("2. Search Available Flights");
                Console.WriteLine("3. Manage Your Bookings");
                Console.WriteLine("4. Return to Main Menu");

                Console.Write("Your Selection: ");
                userChoice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (userChoice)
                {
                    case "1":
                        BookFlight();
                        break;
                    case "2":
                        SearchFlights();
                        break;
                    case "3":
                        ManageBookings();
                        throw new NotImplementedException();
                    case "4":
                        return;
                }
            } while (userChoice != "4");

        }

        private static void BookFlight()
        {
            var searchedFlights = SearchFlights();

            if (searchedFlights.Count == 0)
            {
                return;
            }

            var flightsCount = searchedFlights.Count;
            var exitChoiceNumber = flightsCount + 1;

            Console.WriteLine($"{exitChoiceNumber}. Exit\n");
                
            string? userChoiceStr;

            bool innerFlag = true;

            do
            {
                Console.WriteLine("Which flight would you like to book?");
                Console.Write("Your Selection (Enter flight number): ");
                userChoiceStr = Console.ReadLine();

                Console.WriteLine();

                _ = int.TryParse(userChoiceStr, out int userChoice);

                if (userChoice == exitChoiceNumber)
                {
                    return;
                }
                else if (userChoice >= 1 && userChoice <= flightsCount)
                {
                    do
                    {
                        Console.Write(
                            """
                            Please choose class
                                1. Economy
                                2. Business (+120 dollars)
                                3. First Class (+200 dollars))
                                4. Exit and choose another flight
                            
                            Your Selection: 
                            """
                        );

                        userChoiceStr = Console.ReadLine()?.Trim();
                        Console.WriteLine();

                        switch (userChoiceStr)
                        {
                            case "1":
                                bool success = passenger.TryBookFlight(searchedFlights.ElementAt(userChoice - 1), FlightClass.Economy);
                                if (success)
                                {
                                    Console.WriteLine("Successfully booked a ticket\n");
                                }
                                return;
                            case "2":
                                success = passenger.TryBookFlight(searchedFlights.ElementAt(userChoice - 1), FlightClass.Business);
                                if (success)
                                {
                                    Console.WriteLine("Successfully booked a ticket\n");
                                }
                                return;
                            case "3":
                                success = passenger.TryBookFlight(searchedFlights.ElementAt(userChoice - 1), FlightClass.FirstClass);
                                if (success)
                                {
                                    Console.WriteLine("Successfully booked a ticket\n");
                                }
                                return;
                            case "4":
                                innerFlag = false;
                                break;
                            default:
                                Console.WriteLine("Invalid Option\n");
                                break;
                        }
                    }
                    while (innerFlag);
                }
                else
                {
                    Console.WriteLine("Invalid Option!\n");
                }
            }
            while (true);
        }

        private static void AddIfNotNull(Dictionary<string, string> dict, string key, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                dict[key] = value.Trim();
            }
        }

        private static List<Flight> SearchFlights()
        {
            var searchParameters = new Dictionary<string, string>();

            Console.Write("Enter departure country: ");
            AddIfNotNull(searchParameters, "DepartureCountry", Console.ReadLine());

            Console.Write("Enter destination country: ");
            AddIfNotNull(searchParameters, "DestinationCountry", Console.ReadLine());

            Console.Write("Enter price: ");
            AddIfNotNull(searchParameters, "Price", Console.ReadLine());

            Console.Write("Enter departure airport: ");
            AddIfNotNull(searchParameters, "DepartureAirport", Console.ReadLine());
            
            Console.Write("Enter arrival airport: ");
            AddIfNotNull(searchParameters, "ArrivalAirport", Console.ReadLine());
            
            Console.Write("""Enter flight date (in "yyyy-mm-dd hh:mm" format): """);
            AddIfNotNull(searchParameters, "DepartureDate", Console.ReadLine());

            var foundFlights = Passenger.SearchFlights(searchParameters);

            Console.WriteLine();
            Console.WriteLine("Search Results:");

            if (foundFlights is null || foundFlights.Count == 0)
            {
                Console.WriteLine("No flights found that match your search");
                Console.WriteLine();
                return foundFlights ?? [];
            }

            for (int i = 0; i < foundFlights.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"{i + 1}: {foundFlights[i]}");
                Console.WriteLine();
            }

            return foundFlights;
        }

        private static void ManageBookings()
        {
            Console.WriteLine("***** Manage Your Bookings *****");

            string? userChoice;
            do
            {
                Console.WriteLine("1. Cancel a booking");
                Console.WriteLine("2. Modify a booking");
                Console.WriteLine("3. View your bookings");
                Console.WriteLine("4. Exit booking management dashboard");

                Console.Write("Your selection: ");
                userChoice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (userChoice)
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        return;
                }
            }
            while (userChoice != "4");
        }
    }
}
