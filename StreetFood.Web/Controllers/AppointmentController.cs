using System.Linq;
using System.Web.Mvc;
using Raven.Client;

namespace StreetFood.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IDocumentSession _session;
        public AppointmentController(IDocumentSession session)
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