using FTS.AirportTicketBookingExercise.Attributes;
using FTS.AirportTicketBookingExercise.BookingManagement;
using FTS.AirportTicketBookingExercise.FlightManagement;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace FTS.AirportTicketBookingExercise.UserManagement
{
    public class Manager
    {
        public static void UploadFlights()
        {
            FlightRepository.LoadFromFile();
        }

        public static void ValidateFlightModel()
        {
            var outputText = new StringBuilder();

            var flightProperties = typeof(Flight).GetProperties();

            foreach (var prop in flightProperties)
            {
                outputText.AppendLine(prop.Name + ":");

                outputText.AppendLine($"\t- Type: {prop.PropertyType.Name}");

                var constraints = new List<string>();

                if (Attribute.IsDefined(prop, typeof(RequiredAttribute)))
                {
                    constraints.Add("Required");
                }

                if (Attribute.IsDefined(prop, typeof(RangeAttribute)))
                {
                    var range = prop.GetCustomAttribute<RangeAttribute>();
                    constraints.Add($"Range ({range?.Minimum} to {range?.Maximum})");
                }

                if (Attribute.IsDefined(prop, typeof(FutureDateAttribute)))
                {
                    constraints.Add("Allowed Range (today to future)");
                }

                if (constraints.Count > 0)
                {
                    outputText.Append($"\t- Constraints: ");
                    outputText.AppendLine($"{string.Join(", ", constraints)}\n");
                }
            }

            Console.WriteLine(outputText.ToString());
        }

        public static List<Booking>? SearchBookings(Dictionary<string, string> filters)
        {
            return BookingRepository.GetAllBookings()?.Where(booking =>
                {
                    var flight = booking.GetFlight();

                    if (flight is null)
                    {
                        return false;
                    }

                    // filter by departure country
                    if (filters.TryGetValue("DepartureCountry", out var depCountry)
                        && !string.Equals(depCountry, flight.DepartureCountry, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    // filter by destination country
                    if (filters.TryGetValue("DestinationCountry", out var desCountry)
                        && !string.Equals(desCountry, flight.DestinationCountry, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    // filter by booking price
                    if (filters.TryGetValue("Price", out var priceStr) &&
                        decimal.TryParse(priceStr, out var price))
                    {
                        if (price != booking.Price)
                        {
                            return false;
                        }
                    }

                    // filter by departure airport
                    if (filters.TryGetValue("DepartureAirport", out var depAirport) &&
                        !string.Equals(depAirport, flight.DepartureAirport))
                    {
                        return false;
                    }

                    // filter by arrival airport
                    if (filters.TryGetValue("ArrivalAirport", out var arrAirport) &&
                        !string.Equals(arrAirport, flight.ArrivalAirport))
                    {
                        return false;
                    }

                    // filter by booking date
                    if (filters.TryGetValue("DepartureDate", out var depDateStr) &&
                        DateTime.TryParse(depDateStr, out var depDate))
                    {
                        if (!DateTime.Equals(depDate.Date, booking.BookingDate.Date))
                        {
                            return false;
                        }
                    }

                    return true;
                    
                }).ToList();
        }
    }
}
