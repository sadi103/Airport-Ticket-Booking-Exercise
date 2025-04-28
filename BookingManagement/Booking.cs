using FTS.AirportTicketBookingExercise.FlightManagement;

namespace FTS.AirportTicketBookingExercise.BookingManagement
{
    public class Booking(
        Guid passengerId,
        Guid flightId,
        decimal finalPrice,
        FlightClass flightClass,
        Guid? id = null,
        DateTime? bookingDate = null
        )
    {
        public Guid Id { get; private set; } = id ?? Guid.NewGuid();

        public Guid PassengerId { get; private set; } = passengerId;
        public Guid FlightId { get; private set; } = flightId;

        public decimal Price { get; private set; } = finalPrice;

        public DateTime BookingDate { get; private set; } = bookingDate ?? DateTime.Now;
        public FlightClass Class { get; private set; } = flightClass;

        public Flight GetFlight()
        {
            return FlightRepository.AvailableFlights.Single(flight => flight.Id == Id);
        }

        public string ToCSV()
        {
            return $"{Id},{PassengerId},{FlightId},{BookingDate:yyyy:MM:dd HH:mm},{Price},{Class}";
        }

        public override string ToString()
        {
            var flight = GetFlight();
            return $"Booking of flight from: {flight.DepartureCountry}, to: {flight.DestinationCountry}, departs on {flight.DepartureDate}, from {flight.DepartureAirport}, to {flight.ArrivalAirport}, booked on {BookingDate}, at the price of {Price}$, {Class} Class";
        }
    }
}
