using FTS.AirportTicketBookingExercise.BookingManagement;
using FTS.AirportTicketBookingExercise.FlightManagement;

namespace FTS.AirportTicketBookingExercise.UserManagement
{
    public class Passenger
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        #region Instance Methods
        public List<Booking>? GetBookings()
        {
            return BookingRepository.GetAllBookings()?.Where(booking => booking.PassengerId == Id).ToList();
        }

        public bool TryBookFlight(Flight flight, FlightClass flightClass)
        {
            var previousBookings = GetBookings();

            if (previousBookings is null || !(previousBookings.Any(booking => booking.FlightId == flight.Id)))
            {
                var finalPrice = flight.Price + (int)flightClass;

                var booking = new Booking(Id, flight.Id, finalPrice, flightClass);
                BookingRepository.SaveBooking(booking);

                return true;
            }
            else
            {
                Console.WriteLine("\nYou already have a ticket of this flight");
                return false;
            }
        }
        #endregion

        #region Static Methods
        public static bool TryRemoveBooking(Guid bookingId)
        {
            return BookingRepository.RemoveBooking(bookingId);
        }

        public static bool TryModifyBooking(Guid bookingId, decimal newPrice, FlightClass newClass)
        {
            return BookingRepository.ModifyBooking(bookingId, newPrice, newClass);
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
        #endregion
    }
}
