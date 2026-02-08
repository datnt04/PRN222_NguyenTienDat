using Microsoft.AspNetCore.Mvc;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBTravelCenterContext _context;

        public AccountController(DBTravelCenterContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string code, string password)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Code == code && c.Password == password);

            if (customer == null)
            {
                ViewBag.Error = "Invalid Code or Password";
                return View();
            }

            HttpContext.Session.SetInt32("CustomerID", customer.CustomerId);
            HttpContext.Session.SetString("CustomerName", customer.FullName);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
