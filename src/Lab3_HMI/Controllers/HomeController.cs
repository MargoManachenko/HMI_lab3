using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab3_HMI.Data;
using Microsoft.EntityFrameworkCore;
using Lab3_HMI.Models;

namespace Lab3_HMI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Flights.ToList());
        }

        public IActionResult PassengersOfFlight(int flightId)
        {
            var flight = _db.Flights.Single(f => f.Id == flightId);
            ViewBag.Route = flight.DepaturePoint + "(" + flight.DateOfStart + ")" + " - " + flight.ArrivalPoint + "(" + flight.DateOfFinish + ")";
            ViewBag.FlightId = flightId;
            var passengers = _db.Passengers.Include(p => p.Flight).Include(p => p.Baggage).Where(p => p.Flight.Id == flightId).ToList();
            return View(passengers);
        }

        [HttpGet]
        public IActionResult AddPassenger(int flightId)
        {
            ViewBag.FlightId = flightId;
            var model = new Passenger();
            return View(model);
        }

        public IActionResult AddPassengerToFlight(Passenger passenger, int flightId)
        {
            passenger.Flight = _db.Flights.Single(f => f.Id == flightId);

            if (passenger.Name != null && passenger.Surname != null
                && passenger.Phone != null && passenger.Flight != null)
            {

                foreach (var bag in passenger.Baggage)
                {
                    bag.Passenger = passenger;
                    _db.Baggages.Add(bag);
                }
                _db.Passengers.Add(passenger);

                _db.SaveChanges();

                return RedirectToAction(nameof(PassengersOfFlight), new { flightId = flightId });
            }
            return View();
        }


        [HttpGet]
        public IActionResult EditPassenger(int passengerId)
        {
            var passenger = _db.Passengers.Include(p => p.Flight).Include(n => n.Baggage).Single(p => p.Id == passengerId);
            ViewBag.FlightId = passenger.Flight.Id;
            return View(passenger);
        }

        [HttpPost]
        public IActionResult EditPassenger(Passenger passenger, int flightId)
        {
            if (ModelState.IsValid)
            {
                _db.Passengers.Update(passenger);
                _db.SaveChanges();
                return RedirectToAction(nameof(PassengersOfFlight), new { flightId = flightId });
            }
            return View(passenger);
        }

        //complite mess
        //[HttpPost]
        //public IActionResult EditPassenger(Passenger newPassenger, int oldPassengerId, int flightId)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var oldBaggage = _db.Baggages.Where(b => b.Passenger.Id == oldPassengerId).ToList();
        //        var oldPassanger = _db.Passengers.Single(p => p.Id == oldPassengerId);

        //        foreach (var oldBag in oldBaggage)
        //        {
        //            _db.Baggages.Remove(oldBag);
        //        }

        //        _db.Passengers.Remove(oldPassanger);

        //        _db.SaveChanges();


        //        foreach (var newBag in newPassenger.Baggage)
        //        {
        //            _db.Baggages.Add(newBag);
        //        }

        //        _db.Passengers.Add(newPassenger);
        //        _db.SaveChanges();

        //        return RedirectToAction(nameof(PassengersOfFlight), new { flightId });
        //    }
        //    return View(newPassenger);
        //}


        public IActionResult DeletePassenger(int passengerId, int flightId)
        {
            var passenger = _db.Passengers.Single(p => p.Id == passengerId);
            var listOfBaggages = _db.Baggages.Where(p => p.Passenger == passenger);
            foreach (var bag in listOfBaggages)
            {
                _db.Baggages.Remove(bag);
            }
            _db.Passengers.Remove(passenger);
            _db.SaveChanges();
            return RedirectToAction(nameof(PassengersOfFlight), new { flightId = flightId });
        }

        public IActionResult BaggageOfPassenger(int passengerId)
        {
            return View();
        }

        [HttpGet]
        public IActionResult FlightsMonitoring()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddFlight()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFlight(Flight flight)
        {
            if (ModelState.IsValid)
            {
                _db.Flights.Add(flight);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        [HttpGet]
        public IActionResult EditFlight(int flightId)
        {
            var flight = _db.Flights.Single(f => f.Id == flightId);
            return View(flight);
        }

        [HttpPost]
        public IActionResult EditFlight(Flight flight)
        {
            if (ModelState.IsValid)
            {
                _db.Flights.Update(flight);
                try
                {
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Flight)
                        {
                            // Using a NoTracking query means we get the entity but it is not tracked by the context
                            // and will not be merged with existing entities in the context.
                            var databaseEntity = _db.Flights.AsNoTracking().Single(f => f.Id == ((Flight)entry.Entity).Id);
                            var databaseEntry = _db.Entry(databaseEntity);

                            foreach (var property in entry.Metadata.GetProperties())
                            {
                                var proposedValue = entry.Property(property.Name).CurrentValue;
                                var originalValue = entry.Property(property.Name).OriginalValue;
                                var databaseValue = databaseEntry.Property(property.Name).CurrentValue;

                                entry.Property(property.Name).OriginalValue = databaseEntry.Property(property.Name).CurrentValue;
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                        }
                    }
                    _db.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }


        public IActionResult DeleteFlight(int flightId)
        {
            var flight = _db.Flights.Single(f => f.Id == flightId);
            _db.Flights.Remove(flight);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult FutureFlights()
        {
            var currentDateTime = DateTime.Now;

            var model = new SearchBySurnameViewModel { Flights = new List<Flight>() };

            foreach (var flight in _db.Flights)
            {
                var i = DateTime.Compare(flight.DateOfStart, currentDateTime);
                if (i > 0)
                {
                    model.Flights.Add(flight);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CurrentFlights()
        {
            var currentDateTime = DateTime.Now;

            var model = new SearchBySurnameViewModel { Flights = new List<Flight>() };

            foreach (var flight in _db.Flights)
            {
                var i = DateTime.Compare(flight.DateOfStart, currentDateTime);
                var j = DateTime.Compare(flight.DateOfFinish, currentDateTime);
                if (i <= 0  && j >= 0)
                {
                    model.Flights.Add(flight);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult PastFlights()
        {
            var currentDateTime = DateTime.Now;

            var model = new SearchBySurnameViewModel { Flights = new List<Flight>() };
            
            foreach (var flight in _db.Flights)
            {
                var i = DateTime.Compare(flight.DateOfFinish, currentDateTime);

                if (i < 0)
                {
                    model.Flights.Add(flight);
                }
            }
            return View(model);
        }

        public IActionResult SearchBySurname(string SurnameToSearch)
        {
            var result = new List<Passenger>();

            if (SurnameToSearch != null)
            {
                if (SurnameToSearch.Trim() != "")
                {
                    var searchStr = SurnameToSearch.Trim().ToLower();

                    var passengers = _db.Passengers.Include(p => p.Flight).Include(p => p.Baggage).ToList();

                    foreach (var passenger in passengers)
                    {
                        if (passenger.Surname.ToLower() == searchStr || passenger.Surname.Contains(searchStr))
                        {
                            result.Add(passenger);
                        }
                        
                    }
                }
                return View(result);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Temp()
        {

            return View();
        }


        public IActionResult About(string surname)
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
