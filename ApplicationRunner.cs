using FTS.AirportTicketBookingExercise.BookingManagement;
using FTS.AirportTicketBookingExercise.FlightManagement;
using FTS.AirportTicketBookingExercise.UserManagement;

namespace FTS.AirportTicketBookingExercise
{
    public class ApplicationRunner
    {
        private readonly Passenger _passenger = new();

        public void Run()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            Console.WriteLine(
                """
                *******************************
                **** Flight booking System **** 
                *******************************
                """);

            string? userSelection;

            do
            {
                Console.Write(
                    """
                    
                    Login as:
                    1. Manager
                    2. Passenger
                    3. Exit
                    Your Selection: 
                    """
                );

                userSelection = Console.ReadLine()?.Trim();

                switch (userSelection)
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
            Console.WriteLine("\n****** Manager Operations ******");

            string? userChoice;

            do
            {
                Console.Write(
                    """

                    What would you like to do
                    1. Filter Bookings
                    2. Batch Flight Upload
                    3. Validate Flight Data
                    4. Dynamically Validate Flight Model
                    5. Return to Main Menu
                    Your Selection: 
                    """
                );

                userChoice = Console.ReadLine()?.Trim();

                Console.WriteLine();

                switch (userChoice)
                {
                    case "1":
                        throw new NotImplementedException();
                    case "2":
                        Manager.UploadFlights();
                        break;
                    case "3":
                        throw new NotImplementedException();
                    case "4":
                        throw new NotImplementedException();
                    case "5":
                        return;
                }
            }
            while (true);
        }

        private void ShowPassengerDashboard()
        {
            Console.WriteLine("\n***** Passenger Operations *****");

            string? userChoice;

            do
            {
                Console.Write(
                    """

                    1. Book a Flight
                    2. Search Available Flights
                    3. Manage Your Bookings
                    4. Return to Main Menu
                    Your Selection: 
                    """);

                userChoice = Console.ReadLine()?.Trim();

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
                        break;
                    case "4":
                        return;
                }
            } while (true);
        }

        private void BookFlight()
        {
            var searchedFlights = SearchFlights();

            if (searchedFlights.Count == 0)
            {
                return;
            }

            var flightsCount = searchedFlights.Count;
            var exitChoiceNumber = flightsCount + 1;

            Console.WriteLine($"\n{exitChoiceNumber}. Exit");

            string? userChoiceStr;

            bool innerFlag = true;

            do
            {
                Console.Write(
                    """
                    
                    Which flight would you like to book?
                    Your Selection (Enter flight number): 
                    """
                );

                userChoiceStr = Console.ReadLine();

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
                                bool success = _passenger.TryBookFlight(searchedFlights.ElementAt(userChoice - 1), FlightClass.Economy);
                                if (success)
                                {
                                    Console.WriteLine("\nSuccessfully booked a ticket");
                                }
                                return;
                            case "2":
                                success = _passenger.TryBookFlight(searchedFlights.ElementAt(userChoice - 1), FlightClass.Business);
                                if (success)
                                {
                                    Console.WriteLine("\nSuccessfully booked a ticket");
                                }
                                return;
                            case "3":
                                success = _passenger.TryBookFlight(searchedFlights.ElementAt(userChoice - 1), FlightClass.FirstClass);
                                if (success)
                                {
                                    Console.WriteLine("\nSuccessfully booked a ticket");
                                }
                                return;
                            case "4":
                                innerFlag = false;
                                break;
                            default:
                                Console.WriteLine("\nInvalid Option");
                                break;
                        }
                    }
                    while (innerFlag);
                }
                else
                {
                    Console.WriteLine("\nInvalid Option!");
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

            Console.Write("\nEnter departure country: ");
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

            Console.WriteLine("\nSearch Results:");

            if (foundFlights is null || foundFlights.Count == 0)
            {
                Console.WriteLine("\nNo flights found that match your search");
                return foundFlights ?? [];
            }

            for (int i = 0; i < foundFlights.Count; i++)
            {
                Console.WriteLine($"\n{i + 1}: {foundFlights[i]}");
            }

            return foundFlights;
        }

        private void ManageBookings()
        {
            Console.WriteLine("\n***** Manage Your Bookings *****");

            string? userChoice;

            do
            {
                Console.Write(
                    """

                    1. Cancel a booking
                    2. Modify a booking
                    3. View your bookings
                    4. Exit booking management dashboard
                    Your Selection: 
                    """
                );

                userChoice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (userChoice)
                {
                    case "1":
                        CancelBooking();
                        break;
                    case "2":
                        ModifyBooking();
                        break;
                    case "3":
                        ViewBookings();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("\nInvalid Option");
                        break;
                }
            }
            while (true);
        }

        private void CancelBooking()
        {
            var bookings = ViewBookings();

            if (bookings is null || bookings.Count == 0)
            {
                return;
            }

            var bookingsCount = bookings.Count;
            var exitChoiceNumber = bookingsCount + 1;

            Console.WriteLine($"\n{exitChoiceNumber}. Exit");

            string? userChoiceStr;

            do
            {
                Console.Write(
                    """

                    Which one would you like to cancel?
                    Your Selection: 
                    """
                );

                userChoiceStr = Console.ReadLine()?.Trim();
                Console.WriteLine();
                
                _ = int.TryParse(userChoiceStr, out int userChoice);

                if (userChoice == exitChoiceNumber)
                {
                    return;
                }
                else if (userChoice >= 1 && userChoice <= bookingsCount)
                {
                    bool success = Passenger.TryRemoveBooking(bookings.ElementAt(userChoice - 1).Id);
                    
                    if (success)
                    {
                        Console.WriteLine("\nYou have successfully unbooked your flight");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid Option!");
                }
            }
            while (true);
        }

        private void ModifyBooking()
        {
            var bookings = ViewBookings();

            if (bookings is null || bookings.Count == 0)
            {
                return;
            }

            var bookingsCount = bookings.Count;
            var exitChoiceNumber = bookingsCount + 1;

            Console.WriteLine($"\n{exitChoiceNumber}. Exit");

            string? userChoiceStr;

            do
            {
                Console.Write(
                    """

                    Which one would you like to modify?
                    Your Selection: 
                    """
                );

                userChoiceStr = Console.ReadLine()?.Trim();
                Console.WriteLine();

                _ = int.TryParse(userChoiceStr, out int userChoice);

                if (userChoice == exitChoiceNumber)
                {
                    return;
                }
                else if (userChoice >= 1 && userChoice <= bookingsCount)
                {
                    var booking = bookings.ElementAt(userChoice - 1);

                    Console.Write($"""

                        Your current ticket class is {booking.Class}
                        Change it to: 
                        """
                    );

                    userChoiceStr = Console.ReadLine()?.Trim();

                    if (Enum.TryParse(userChoiceStr, true, out FlightClass newClass))
                    {
                        /*
                         * If the transition if made from a less expensive class 
                         * to a more expensive one, add the difference between thier prices.
                         * Otherwise, subtract it.
                         */
                        var newPrice = booking.Class < newClass ?
                            booking.Price + (decimal)newClass : booking.Price - (decimal)booking.Class;

                        if (Passenger.TryModifyBooking(booking.Id, newPrice, newClass))
                        {
                            Console.WriteLine("\nYou have successfully changed the class");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid Class Type. Choose Economy, Business, or FirstClass");
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid Option!");
                }
            }
            while (true);
        }

        private List<Booking>? ViewBookings()
        {
            var bookings = _passenger.GetBookings();

            if (bookings is not null && bookings.Count != 0)
            {
                Console.WriteLine("\nYour upcomming flights:");

                for (int i = 0; i < bookings.Count; i++)
                {
                    var booking = bookings[i];

                    var flight = FlightRepository.AvailableFlights.Single(
                        flight => flight.Id == booking.FlightId
                        );

                    Console.WriteLine($"\n{i + 1}: {flight.DepartureCountry} -> {flight.DestinationCountry}, on {flight.DepartureDate}, at the price of {booking.Price} {booking.Class} Class");
                }
            }
            else
            {
                Console.WriteLine("\nYou haven't made any bookings");
            }

            return bookings;
        }
    }
}
