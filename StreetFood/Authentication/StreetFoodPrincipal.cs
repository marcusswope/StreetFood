using System.Security.Principal;

namespace StreetFood.Authentication
{
    public class StreetFoodPrincipal : GenericPrincipal
    {
        public StreetFoodPrincipal(Account account)
            : base(new StreetFoodIdentity(account), new[] { "" })
        {
        }
    }
}