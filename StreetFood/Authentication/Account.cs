using System;

namespace StreetFood.Authentication
{
    public class Account
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}