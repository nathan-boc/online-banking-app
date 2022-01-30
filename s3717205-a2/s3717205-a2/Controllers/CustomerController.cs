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

        public IActionResult Withdraw() => View();

        [HttpPost]
        public async Task<IActionResult> Withdraw(decimal amount, int accountNumber)
        {
            var account = await _context.Account.FindAsync(accountNumber);

            // Checks for amount to be positive
            if (amount > 0 == false)
                ModelState.AddModelError("NegativeAmount", "The withdraw amount must be greater than 0.");
            // Checks for decimal places
            else if (amount.MoreThanNDecimalPlaces(2) == true)
                ModelState.AddModelError("TooManyDecimals", "The withdraw amount cannot have more than 2 decimal places.");
            // Checks if specified account is owned by the customer
            else if (account == null || account.CustomerID != CustomerID)
                ModelState.AddModelError("InvalidAccount", "Invalid account number. Please input one of your accounts.");
            // Checks if account has sufficient funds
            else if ((account.AccountType == 'C' && account.Balance - amount < 300) 
                || (account.AccountType == 'S' && account.Balance - amount < 0))
                ModelState.AddModelError("InsufficientFunds", "Insufficient funds.");

            // If an invalid input is given, pass the inputted amount to the new view call
            if (ModelState.IsValid == false)
            {
                ViewBag.Amount = amount;
                return View(account);
            }
            else
            {
                // Subtract from current balance and add transaction
                account.Balance -= amount;
                account.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = 'W',
                        Amount = amount,
                        TransactionTimeUtc = DateTime.UtcNow
                    });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Withdraw));
            }
        }

        public IActionResult Transfer() => View();

        [HttpPost]
        public async Task<IActionResult> Transfer(decimal amount, int accountNumber, int destinationAccountNumber)
        {
            var account = await _context.Account.FindAsync(accountNumber);
            var destAccount = await _context.Account.FindAsync(destinationAccountNumber);

            // Checks if destination and account number match
            if (accountNumber == destinationAccountNumber)
                ModelState.AddModelError("SameAccount", "You cannot transfer funds to the same account.");
            // Checks for amount to be positive
            if (amount > 0 == false)
                ModelState.AddModelError("NegativeAmount", "The withdraw amount must be greater than 0.");
            // Checks for decimal places
            else if (amount.MoreThanNDecimalPlaces(2) == true)
                ModelState.AddModelError("TooManyDecimals", "The withdraw amount cannot have more than 2 decimal places.");
            // Checks if specified account is owned by the customer
            else if (account == null || account.CustomerID != CustomerID)
                ModelState.AddModelError("InvalidAccount", "Invalid account number. Please input one of your accounts.");
            // Checks if specified destination account exists
            else if (destAccount == null)
                ModelState.AddModelError("AccountNotFound", "That account could not be found.");
            // Checks if account has sufficient funds
            else if ((account.AccountType == 'C' && account.Balance - amount < 300)
                || (account.AccountType == 'S' && account.Balance - amount < 0))
                ModelState.AddModelError("InsufficientFunds", "Insufficient funds.");

            // If an invalid input is given, pass the inputted amount to the new view call
            if (ModelState.IsValid == false)
            {
                ViewBag.Amount = amount;
                return View();
            }
            else
            {

            }

            // TODO : There should be 2 transaction rows added in the end - one for each customer involved
            // Sender should have a row with senders accountNumber + the destinationAccountNumber
            // Receiver should have a row with their accountNumber + null
            // Both rows should be transactionType 'T'

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
                ViewBag.Account = account;

                // Retrieves 4 transactions at a time matching account number
                var transactionList = await _context.Transaction.Where(x => x.AccountNumber == accountNumber).OrderByDescending(x => x.TransactionTimeUtc)
                    .Skip(pageSize * page).Take(pageSize).ToListAsync();

                // Throw error when going out of bounds
                if (transactionList == null || transactionList.Count == 0)
                {
                    // Reverses the effect of clicking 'Next Page'
                    page -= 1;
                    ViewBag.Page = page;

                    transactionList = await _context.Transaction.Where(x => x.AccountNumber == accountNumber).OrderByDescending(x => x.TransactionTimeUtc)
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
