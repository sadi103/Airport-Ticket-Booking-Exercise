using FTS.AirportTicketBookingExercise.FlightManagement;

namespace FTS.AirportTicketBookingExercise.UserManagement
{
    public class Manager
    {
        public static void UploadFlights()
        {
            FlightRepository.LoadFromFile();
        }
    }
}
