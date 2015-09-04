using System;
using System.Linq;
using System.Threading;
using System.Web;
using Raven.Client;

namespace StreetFood.Authentication
{
    public interface IAccountant
    {
        void Login(Account account, HttpContextBase httpContext);
        void Authenticate(HttpContextBase httpContext);
        void Logout(HttpContextBase httpContext);
        Account GetUser(HttpContextBase httpContext);
    }

    public class Accountant : IAccountant
    {
        private string _passphrase = "373289761542-q89spcr7cr3su1c3f6gcvvrs15jpd67q";
        private readonly IDocumentSession _session;

        public Accountant(IDocumentSession session)
        {
            _session = session;
        }

        public void Login(Account account, HttpContextBase httpContext)
        {
            var principal = new StreetFoodPrincipal(account);
            Thread.CurrentPrincipal = principal;
            httpContext.User = principal;
            
            var encryptedEmail = StringCipher.Encrypt(account.EmailAddress, _passphrase);
            var cookie = new HttpCookie("email");
            cookie.Values.Add("email", encryptedEmail);
            cookie.Expires = DateTime.Now.AddMonths(1);
            httpContext.Response.Cookies.Add(cookie);
        }

        public void Authenticate(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies["email"];
            if (cookie == null || string.IsNullOrEmpty(cookie.Values["email"])) return;

            var email = StringCipher.Decrypt(cookie.Values["email"], _passphrase);
            var account = _session.Query<Account>().FirstOrDefault(x => x.EmailAddress == email);

            if (account != null)
            {
                Login(account, httpContext);
            }
        }

        public void Logout(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies["email"] != null)
            {
                httpContext.Request.Cookies["email"].Expires = DateTime.Today.AddDays(-1);
            }
        }

        public Account GetUser(HttpContextBase httpContext)
        {
            return GetAccount(httpContext);
        }

        public static bool LoggedIn(HttpContextBase httpContext)
        {
            return httpContext.User is StreetFoodPrincipal || Thread.CurrentPrincipal is StreetFoodPrincipal;
        }

        public static Account GetAccount(HttpContextBase httpContext)
        {
            var principal = httpContext.User as StreetFoodPrincipal ??
                            Thread.CurrentPrincipal as StreetFoodPrincipal;

            var identity = principal.Identity as StreetFoodIdentity;
            return identity.Account;
        }
    }
}