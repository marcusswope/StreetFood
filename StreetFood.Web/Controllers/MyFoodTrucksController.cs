using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Linq;
using StreetFood.Authentication;

namespace StreetFood.Web.Controllers
{
    public class MyFoodTrucksController : Controller
    {
        private readonly IDocumentSession _session;
        private readonly IAccountant _accountant;

        public MyFoodTrucksController(IDocumentSession session, IAccountant accountant)
        {
            _session = session;
            _accountant = accountant;
        }

        public ActionResult Index()
        {
            var foodTruckIds = _accountant.GetUser(HttpContext).FoodTrucks.Select(x => x.FoodTruckId);
            var foodTrucks = _session.Query<FoodTruck>().Where(x => x.Id.In(foodTruckIds)).ToList();
            return View(foodTrucks);
        }

        [HttpPost]
        public ActionResult AddAppointment(Appointment appointment)
        {
            _session.Store(appointment);
            _session.SaveChanges();
            return Json(true);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FoodTruck truck)
        {
            _session.Store(truck);

            var user = _accountant.GetUser(HttpContext);
            user.FoodTrucks.Add(new FoodTruckAccount { FoodTruckId = truck.Id });
            
            _session.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult View(Guid id)
        {
            var truck = _session.Load<FoodTruck>(FoodTruck.Load(id));
            var appointments = _session.Query<Appointment>().Where(x => x.FoodTruckId == id).ToList();
            var model = new ViewFoodTruckModel {Truck = truck, Appointments = appointments};
            return View(model);
        }
    }

    public class ViewFoodTruckModel
    {
        public FoodTruck Truck { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}