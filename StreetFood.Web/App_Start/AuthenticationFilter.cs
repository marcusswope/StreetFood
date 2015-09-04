using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;
using StreetFood.Authentication;

namespace StreetFood.Web
{
    public class AuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var accountant = new Accountant(ServiceLocator.Current.GetInstance<IDocumentSession>());
            accountant.Authenticate(filterContext.HttpContext);
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            
        }
    }
}