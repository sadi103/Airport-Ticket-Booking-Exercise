using FTS.AirportTicketBookingExercise.BookingManagement;
using FTS.AirportTicketBookingExercise.FlightManagement;

namespace FTS.AirportTicketBookingExercise.UserManagement
{
    public class Passenger
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        private readonly List<Booking> _bookings = [];

        public bool TryBookFlight(Flight flight, FlightClass flightClass)
        {
            if (! _bookings.Any(booking => booking.FlightId == flight.Id))
            {
                var booking = new Booking(Id, flight.Id, flightClass);
                BookingRepository.SaveBooking(booking);

                _bookings.Add(booking);

                return true;
            }
            else
            {
                Console.WriteLine("You already have a ticket of this flight\n");
                return false;
            }
        }

        public static List<Flight>? SearchFlights(Dictionary<string, string> searchParameters)
        {
            var filteredFlights = FlightRepository.AvailableFlights.Where(flight =>
            {
                // Filter by price
                if (searchParameters.TryGetValue("Price", out var priceStr) &&
                    decimal.TryParse(priceStr, out var price))
                {
                    if (flight.Price != price)
                    {
                        return false;
                    }
                }

                // Departure Country
                if (searchParameters.TryGetValue("DepartureCountry", out var depCountry) &&
                    !string.Equals(flight.DepartureCountry, depCountry, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                // Destination Country
                if (searchParameters.TryGetValue("DestinationCountry", out var destCountry) &&
                    !string.Equals(flight.DestinationCountry, destCountry, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                // Departure Date
                if (searchParameters.TryGetValue("DepartureDate", out var depDateStr) &&
                    DateTime.TryParse(depDateStr, out var depDate))
                {
                    if (!DateTime.Equals(flight.DepartureDate.Date, depDate.Date))
                    {
                        return false;
                    }
                }

                // Departure Airport
                if (searchParameters.TryGetValue("DepartureAirport", out var depAirport) &&
                    !string.Equals(flight.DepartureAirport, depAirport, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                // Arrival Airport
                if (searchParameters.TryGetValue("ArrivalAirport", out var arrAirport) &&
                    !string.Equals(flight.ArrivalAirport, arrAirport, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return true; // Passes all conditions
            }).ToList();

            return filteredFlights;
        }

        public override string ToString()
        {
            return $"Passenger {{ Id = {Id} }}";
        }
    }
}
