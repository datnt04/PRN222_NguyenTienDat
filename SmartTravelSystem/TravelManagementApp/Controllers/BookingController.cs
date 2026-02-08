using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly DBTravelCenterContext _context;

        public BookingController(DBTravelCenterContext context)
        {
            _context = context;
        }

        // Check if user is logged in
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("CustomerID") != null;
        }

        public IActionResult MyBookings(string statusFilter = "")
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            int customerId = HttpContext.Session.GetInt32("CustomerID") ?? 0;

            var bookings = _context.Bookings
                .Include(b => b.Trip)
                .Where(b => b.CustomerId == customerId)
                .AsQueryable();

            // Apply status filter if provided
            if (!string.IsNullOrEmpty(statusFilter))
            {
                bookings = bookings.Where(b => b.Status == statusFilter);
            }

            var result = bookings
                .OrderBy(b => b.BookingDate)
                .ToList();

            ViewBag.StatusFilter = statusFilter;
            ViewBag.AvailableTrips = _context.Trips.Where(t => t.Status == "Confirmed").ToList();

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int tripId)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var customerId = HttpContext.Session.GetInt32("CustomerID");
            if (!customerId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = new Booking
            {
                TripId = tripId,
                CustomerId = customerId.Value,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("MyBookings");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int bookingId, string status)
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = _context.Bookings.Find(bookingId);
            var sessionCustomerId = HttpContext.Session.GetInt32("CustomerID");
            
            if (booking != null && sessionCustomerId.HasValue && booking.CustomerId == sessionCustomerId.Value)
            {
                booking.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction("MyBookings");
        }
    }
}
