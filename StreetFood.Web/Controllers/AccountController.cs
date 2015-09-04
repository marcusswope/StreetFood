using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Raven.Client;
using StreetFood.Authentication;

namespace StreetFood.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDocumentSession _session;
        private readonly IJwtValidator _jwtValidator;
        private readonly IAccountant _accountant;

        public AccountController(IDocumentSession session, IJwtValidator jwtValidator, IAccountant accountant)
        {
            _session = session;
            _jwtValidator = jwtValidator;
            _accountant = accountant;
        }

        [HttpPost]
        public ActionResult GoogleSignIn(GoogleLoginInformation loginInformation)
        {
            var result = _jwtValidator.Validate(loginInformation.IdToken);
            if (!result.IsValid) return Json(false);

            var account = _session.Query<Account>().FirstOrDefault(x => x.EmailAddress == loginInformation.Email);
            if (account == null)
            {
                account = new Account
                {
                    EmailAddress = loginInformation.Email,
                    CreatedDate = DateTime.UtcNow,
                    ImageUrl = loginInformation.Image,
                    Name = loginInformation.Name
                };
                if (string.IsNullOrEmpty(account.ImageUrl))
                {
                    using (var md5 = MD5.Create())
                    {
                        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(account.EmailAddress.Trim().ToLower()));
                        var stringHash = new StringBuilder(hash.Length * 2);
                        foreach (var b in hash)
                            stringHash.Append(b.ToString("X2"));

                        account.ImageUrl = @"http://www.gravatar.com/avatar/" + stringHash;
                    }
                    
                }
                _session.Store(account);
                _session.SaveChanges();
            }

            _accountant.Login(account, HttpContext);

            return Json(true);
        }
    }

    public class GoogleLoginInformation
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string IdToken { get; set; }
    }
}