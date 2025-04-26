using FTS.AirportTicketBookingExercise.FlightManagement;

namespace FTS.AirportTicketBookingExercise.BookingManagement
{
    public static class BookingRepository
    {
        private const string BOOKINGS_DIRECTORY = @"C:\Users\MindOptemizer\source\repos\FTS.AirportTicketBookingExercise\FileStorage\";
        private const string BOOKING_FILE_NAME = "bookings.csv";

        private static string FULL_PATH => $"{BOOKINGS_DIRECTORY}{BOOKING_FILE_NAME}";

        public static List<Booking>? GetAllBookings()
        {
            if (! File.Exists($"{FULL_PATH}"))
            {
                Console.WriteLine("\nFile doesn't exist");
                return null;
            }

            var bookingsAsStrings = File.ReadAllLines(FULL_PATH);

            var bookingsToReturn = new List<Booking>();

            for (int i = 0; i < bookingsAsStrings.Length; i++)
            {
                var line = bookingsAsStrings[i];

                var bookingSplit = line.Split(',').Select(x => x.Trim()).ToArray();

                if (bookingSplit.Length < 6)
                {
                    Console.WriteLine($"\nWarning: skipping malformed booking line no. {i + 1}");
                    continue;
                }

                bool success = Guid.TryParse(bookingSplit[0], out Guid bookingId);
                if (!success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed booking id");
                    continue;
                }

                success = Guid.TryParse(bookingSplit[1], out Guid passengerId);
                if (!success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed passenger id");
                    continue;
                }

                success = Guid.TryParse(bookingSplit[2], out Guid flightId);
                if (!success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed flight id");
                    continue;
                }

                success = DateTime.TryParse(bookingSplit[3], out DateTime bookingDate);
                if (!success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed booking date");
                    continue;
                }

                success = decimal.TryParse(bookingSplit[4], out decimal bookingPrice);
                if (!success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed booking price");
                    continue;
                }

                success = Enum.TryParse(bookingSplit[5], out FlightClass bookingClass);
                if (!success)
                {
                    Console.WriteLine($"\nWarning: skipping line no. {i + 1} with malformed booking id");
                    continue;
                }

                var booking = new Booking(
                    passengerId,
                    flightId,
                    bookingPrice,
                    bookingClass,
                    bookingId,
                    bookingDate
                );

                bookingsToReturn.Add(booking);
            }

            return bookingsToReturn;
        }

        public static void SaveBooking(Booking booking)
        {
            if (! Directory.Exists(BOOKINGS_DIRECTORY))
            {
                Directory.CreateDirectory(BOOKINGS_DIRECTORY);
            }

            File.AppendAllText($"{FULL_PATH}", $"{booking.ToCSV()}\n");
        }

        public static bool RemoveBooking(Guid id)
        {
            try
            {
                var newFileContents = File.ReadAllLines(FULL_PATH).Where(line => !line.StartsWith(id.ToString()));
                File.WriteAllLines(FULL_PATH, newFileContents);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + ex.Message);
                return false;
            }
        }

        public static bool ModifyBooking(Guid id, decimal newPrice, FlightClass newClass)
        {
            try
            {
                var newFileContents = File.ReadAllLines(FULL_PATH)
                    .Select(
                        booking =>
                        {
                            if (booking.StartsWith(id.ToString()))
                            {
                                var bookingSplit = booking.Split(',');
                                
                                if (bookingSplit.Length == 6)
                                {
                                    bookingSplit[4] = newPrice.ToString();
                                    bookingSplit[5] = newClass.ToString();

                                    return string.Join(",", bookingSplit);
                                }
                            }

                            return booking;
                        }
                    );

                File.WriteAllLines(FULL_PATH, newFileContents);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + ex.Message);
                return false;
            }
        }
    }
}
