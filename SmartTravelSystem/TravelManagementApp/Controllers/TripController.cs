using Microsoft.AspNetCore.Mvc;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers
{
    public class TripController : Controller
    {
        private readonly DBTravelCenterContext _context;

        public TripController(DBTravelCenterContext context)
        {
            _context = context;
        }

        // Check if user is logged in
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("CustomerID") != null;
        }

        public IActionResult Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var trips = _context.Trips.ToList();
            return View(trips);
        }

        // GET: Trip/Create
        public IActionResult Create()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: Trip/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Trip trip)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                _context.Trips.Add(trip);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trip);
        }

        // GET: Trip/Edit/5
        public IActionResult Edit(int id)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var trip = _context.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trip/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Trip trip)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id != trip.TripId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Trips.Update(trip);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trip);
        }
    }
}
