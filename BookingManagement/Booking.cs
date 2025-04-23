using FTS.AirportTicketBookingExercise.FlightManagement;

namespace FTS.AirportTicketBookingExercise.BookingManagement
{
    public class Booking
    {
        public Guid Id { get; private set; }

        public Guid PassengerId { get; init; }
        public Guid FlightId { get; init; }

        public decimal FinalPrice { get; private set; }

        public DateTime Date { get; private set; } = DateTime.Now;
        public FlightClass Class { get; private set; }

        public Booking(Guid passengerId, Guid flightId, FlightClass flightClass)
        {
            Id = Guid.NewGuid();
            PassengerId = passengerId;
            FlightId = flightId;
            Class = flightClass;

            SetFinalPrice();
        }

        private void SetFinalPrice()
        {
            var flightPrice = FlightRepository.AvailableFlights
                .Single(flight => flight.Id == FlightId).Price;

            if (Class is FlightClass.Economy)
            {
                FinalPrice = flightPrice;
            }
            else
            {
                FinalPrice = flightPrice + (int) Class;
            }
        }

        public string ToCSV()
        {
            return $"{Id},{PassengerId},{FlightId},{Date},{FinalPrice},{Class}";
        }
    }
}
