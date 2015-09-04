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
            return View();
        }

        [HttpPost]
        public ActionResult AddAppointment(Appointment appointment)
        {
            _session.Store(appointment);
            _session.SaveChanges();
            return View("Index");
        }
    }
}