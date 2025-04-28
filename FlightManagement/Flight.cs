using FTS.AirportTicketBookingExercise.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FTS.AirportTicketBookingExercise.FlightManagement
{
    public class Flight
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [Range(typeof(decimal), "0", "100000")]
        public required decimal Price { get; set; }
        
        [Required]
        public required string DepartureCountry { get; set; }
        
        [Required]
        public required string DestinationCountry { get; set; }
        
        [Required]
        [FutureDate]
        public required DateTime DepartureDate { get; set; }
        
        [Required]
        public required string DepartureAirport { get; set; }
        
        [Required]
        public required string ArrivalAirport { get; set; }

        public override string ToString()
        {
            return $"Flight from: {DepartureCountry}, to: {DestinationCountry}, at the price of {Price}, departs on {DepartureDate}, from {DepartureAirport}, to {ArrivalAirport}";
        }
    }
}
