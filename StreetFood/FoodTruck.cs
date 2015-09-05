using System;

namespace StreetFood
{
    public class FoodTruck
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public static string Load(Guid id)
        {
            return @"FoodTrucks/" + id;
        }
    }
}