using Microsoft.AspNetCore.Mvc;

using MvcBank.Data;
using MvcBank.Models;
using MvcBank.Filters;

namespace s3717205_a2.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly MvcBankContext _context;

        // Retrieved from session data
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public CustomerController(MvcBankContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            // Loads current customers' data using
            var customer = await _context.Customer.FindAsync(CustomerID);

            return View(customer);
        }

        public IActionResult Deposit()
        {
            return View();
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        public IActionResult Transfer()
        {
            return View();
        }

        public IActionResult MyStatements()
        {
            return View();
        }

        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
