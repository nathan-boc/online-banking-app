using MvcBank.Data;
using MvcBank.Models;
using Microsoft.AspNetCore.Mvc;

namespace s3717205_a2.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MvcBankContext _context;

        // Retrieved from session data
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public CustomerController(MvcBankContext context) => _context = context;

        // GET: HomeController1
        public async Task<IActionResult> Index()
        {
            // Loads current customers' data using
            var customer = await _context.Customer.FindAsync(CustomerID);

            return View(customer);
        }

        // GET: HomeController1/Details/5
        public IActionResult Deposit()
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public IActionResult Withdraw()
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public IActionResult Transfer()
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public IActionResult MyStatements()
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public IActionResult Profile()
        {
            return View();
        }

    }
}
