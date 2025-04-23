namespace FTS.AirportTicketBookingExercise.BookingManagement
{
    public static class BookingRepository
    {
        private const string BOOKINGS_PATH = @"C:\Users\MindOptemizer\source\repos\FTS.AirportTicketBookingExercise\FileStorage\bookings.csv";

        public static void SaveBooking(Booking booking)
        {
            File.AppendAllText(BOOKINGS_PATH, $"{booking.ToCSV()}\n");
        }
    }
}
