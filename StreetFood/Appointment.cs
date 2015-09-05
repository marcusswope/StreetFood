using System;

namespace StreetFood
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid FoodTruckId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LocationName { get; set; }
        public string Vicinity { get; set; }
        public string PlaceId { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public string TimeDescription
        {
            get
            {
                if (StartDate.Date == EndDate.Date)
                {
                    return string.Format("{0} {1} - {2}", StartDate.Date.ToString("M/d/yy"),
                        StartDate.ToString("h:mmtt").ToLower(), EndDate.ToString("h:mmtt").ToLower());
                }

                return string.Format("{0} {1} - {2} {3}", StartDate.Date.ToString("M/d/yy"),
                        StartDate.ToString("h:mmtt").ToLower(), EndDate.Date.ToString("M/d/yy"), EndDate.ToString("h:mmtt").ToLower());
            }
        }
    }
}