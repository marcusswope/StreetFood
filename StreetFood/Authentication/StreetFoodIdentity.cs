using System.Security.Principal;

namespace StreetFood.Authentication
{
    public class StreetFoodIdentity : GenericIdentity
    {
        public Account Account { get; private set; }

        public StreetFoodIdentity(Account account)
            : base(account.EmailAddress)
        {
            Account = account;
        }
    }
}