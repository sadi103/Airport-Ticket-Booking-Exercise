using System.ComponentModel.DataAnnotations;

namespace FTS.AirportTicketBookingExercise.FlightManagement
{
    public static class FlightRepository
    {
        private const string FLIGHTS_PATH = @"C:\Users\MindOptemizer\source\repos\FTS.AirportTicketBookingExercise\FileStorage\flights.csv";
        public static List<Flight> AvailableFlights { get; private set; } = [];

        public static void LoadFromFile()
        {
            if (!File.Exists(FLIGHTS_PATH))
            {
                Console.WriteLine("\nFile doesn't exist");
                return;
            }

            Console.WriteLine("\nUploading the flights...");

            AvailableFlights.Clear(); // Avoid duplicate entries if this method is called twice accedently

            var flightsAsString = File.ReadAllLines(FLIGHTS_PATH);

            for (int i = 0; i < flightsAsString.Length; i++)
            {
                var line = flightsAsString[i];

                var flightSplit = line.Split(',').Select(x => x.Trim()).ToArray();

                if (flightSplit.Length < 7)
                {
                    Console.WriteLine($"Skipping malformed line no. {i + 1}");
                    continue;
                }

                bool success = Guid.TryParse(flightSplit[0], out Guid flightId);
                if (! success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed flight id");
                    continue;
                }
                    
                success = decimal.TryParse(flightSplit[3], out decimal price);
                if (!success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed flight price");
                    continue;
                }

                success = DateTime.TryParse(flightSplit[4], out DateTime date);
                if (!success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed flight date");
                    continue;
                }

                var flight = new Flight()
                {
                    Id = flightId,
                    DepartureCountry = flightSplit[1],
                    DestinationCountry = flightSplit[2],
                    Price = price,
                    DepartureDate = date,
                    DepartureAirport = flightSplit[5],
                    ArrivalAirport = flightSplit[6]
                };

                var context = new ValidationContext(flight);
                Validator.ValidateObject(flight, context, validateAllProperties: true);

                AvailableFlights.Add(flight);
            }

            Console.WriteLine("Successfully loaded the flights data.");
        }
    }
}
