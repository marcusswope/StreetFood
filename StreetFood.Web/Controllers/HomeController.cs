using System.Linq;
using System.Web.Mvc;
using Raven.Client;

namespace StreetFood.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDocumentSession _session;
        public HomeController(IDocumentSession session)
        {
            _session = session;
        }

        public ActionResult Index()
        {
            var appointments = _session.Query<Appointment>().ToList();
            return View(appointments);
        }
    }
}