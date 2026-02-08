using Microsoft.AspNetCore.Mvc;
using TravelDataAccess.Models;

namespace TravelManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBTravelCenterContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(DBTravelCenterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string code, string password)
        {
            // Check Admin Login from appsettings.json
            var adminCode = _configuration["AdminAccount:Code"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (code == adminCode && password == adminPassword)
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("CustomerName", "Administrator");
                return RedirectToAction("Index", "Home");
            }

            // Check Customer Login from DB
            var customer = _context.Customers
                .FirstOrDefault(c => c.Code == code && c.Password == password);

            if (customer == null)
            {
                ViewBag.Error = "Invalid Code or Password";
                return View();
            }

            HttpContext.Session.SetString("Role", "Customer");
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
