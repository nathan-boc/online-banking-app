using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult MyStatements() => View();

        [HttpPost]
        public async Task<IActionResult> MyStatements(int accountNumber, int page)
        {
            // Checks if account is owned by the customer
            var account = await _context.Account.FindAsync(accountNumber);

            if (account == null || account.CustomerID != CustomerID)
            {
                // Returns error message if account number isn't associated with logged in customer
                ModelState.AddModelError("InvalidAccount", "Invalid account number. Please input one of your accounts.");
                return View();
            }
            else
            {
                const int pageSize = 4;

                // Pass page number and account number to the view
                ViewBag.Page = page;
                ViewBag.AccountNumber = accountNumber;

                // Retrieves 4 transactions at a time matching account number
                var transactionList = await _context.Transaction.Where(x => x.AccountNumber == accountNumber).OrderBy(x => x.TransactionTimeUtc)
                    .Skip(pageSize * page).Take(pageSize).ToListAsync();

                // Throw error when going out of bounds
                if (transactionList == null || transactionList.Count == 0)
                {
                    // Reverses the effect of clicking 'Next Page'
                    page -= 1;
                    ViewBag.Page = page;

                    transactionList = await _context.Transaction.Where(x => x.AccountNumber == accountNumber).OrderBy(x => x.TransactionTimeUtc)
                        .Skip(pageSize * page).Take(pageSize).ToListAsync();

                    ModelState.AddModelError("OutOfBounds", "This is the final page of your statement.");
                    return View(transactionList); ;
                }

                // Returns 4 transactions as a list
                return View(transactionList);
            }
        }

        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
