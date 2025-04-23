using FTS.AirportTicketBookingExercise.FlightManagement;

namespace FTS.AirportTicketBookingExercise.UserManagement
{
    public class Manager
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public static void UploadFlights()
        {
            FlightRepository.LoadFromFile();
        }

        public override string ToString()
        {
            return $"Manager {{ Id = {Id} }}";
        }
    }
}
