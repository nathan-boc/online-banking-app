using Microsoft.AspNetCore.Mvc;

using MvcBank.Data;
using MvcBank.Models;
using MvcBank.Filters;
using MvcBank.Utilities;

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

        public IActionResult Deposit() => View();
        
        [HttpPost]
        public async Task<IActionResult> Deposit(decimal amount, int accountNumber)
        {
            var account = await _context.Account.FindAsync(accountNumber);

            // Checking for valid deposit amount
            if(amount > 0 == false)
                ModelState.AddModelError("NegativeAmount", "The deposit amount must be greater than 0.");
            else if(amount.MoreThanNDecimalPlaces(2) == true)
                ModelState.AddModelError("TooManyDecimals", "The deposit amount cannot have more than 2 decimal places.");
            else if(account == null || account.CustomerID != CustomerID)
                ModelState.AddModelError("InvalidAccount", "Invalid account number. Please input one of your accounts.");

            // If an invalid input is given, pass the inputted amount to the new view call
            if (ModelState.IsValid == false)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            else
            {
                // On success, add to current balance and list of transactions
                account.Balance += amount;
                account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = 'D',
                        Amount = amount,
                        TransactionTimeUtc = DateTime.UtcNow
                    });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Deposit));
            }
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
