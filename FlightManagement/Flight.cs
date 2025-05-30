﻿namespace FTS.AirportTicketBookingExercise.FlightManagement
{
    public class Flight
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public required decimal Price { get; set; }
        public required string DepartureCountry { get; set; }
        public required string DestinationCountry { get; set; }
        public required DateTime DepartureDate { get; set; }
        public required string DepartureAirport { get; set; }
        public required string ArrivalAirport { get; set; }


        public override string ToString()
        {
            return $"Flight from: {DepartureCountry}, to: {DestinationCountry}, at the price of {Price}, Departs on {DepartureDate}, from airport: {DepartureAirport}, to airport: {ArrivalAirport}";
        }
    }
}
