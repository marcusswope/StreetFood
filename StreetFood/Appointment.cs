using System;

namespace StreetFood
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LocationName { get; set; }
        public string Vicinity { get; set; }
        public string PlaceId { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}