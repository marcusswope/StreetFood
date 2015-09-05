using System;
using System.Collections.Generic;

namespace StreetFood
{
    public class Account
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<FoodTruckAccount> FoodTrucks { get; set; }
        
        public Account()
        {
            FoodTrucks = new List<FoodTruckAccount>();
        }
    }

    public class FoodTruckAccount
    {
        public Guid FoodTruckId { get; set; }
    }
}