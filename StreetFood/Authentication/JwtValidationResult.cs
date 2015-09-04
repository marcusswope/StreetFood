namespace StreetFood.Authentication
{
    public class JwtValidationResult
    {
        public bool IsValid { get; set; }
        public string EmailAddress { get; set; }
    }
}