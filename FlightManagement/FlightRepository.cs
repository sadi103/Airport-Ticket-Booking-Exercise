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
                Console.WriteLine("File doesn't exist");
                return;
            }

            AvailableFlights.Clear(); // Avoid duplicate entries if this method is called twice accedently

            string[] flightsAsString = File.ReadAllLines(FLIGHTS_PATH);

            foreach (var line in flightsAsString)
            {
                var flightSplit = line.Split(',');

                bool success = decimal.TryParse(flightSplit[2], out decimal price);
                if (!success)
                {
                    price = 100M;
                }

                success = DateTime.TryParse(flightSplit[3], out DateTime date);
                if (!success)
                {
                    var oneWeekLater = DateTime.Now.AddDays(7);
                    date = new DateTime(
                        oneWeekLater.Year,
                        oneWeekLater.Month,
                        oneWeekLater.Day,
                        oneWeekLater.Hour,
                        0,
                        0
                    );
                }

                var flight = new Flight()
                {
                    DepartureCountry = flightSplit[0],
                    DestinationCountry = flightSplit[1],
                    Price = price,
                    DepartureDate = date,
                    DepartureAirport = flightSplit[4],
                    ArrivalAirport = flightSplit[5]
                };

                AvailableFlights.Add(flight);
            }
        }
    }
}
