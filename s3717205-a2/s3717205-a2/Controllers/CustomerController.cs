using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;

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
            if (amount > 0 == false)
                ModelState.AddModelError("NegativeAmount", "The deposit amount must be greater than 0.");
            else if (amount.MoreThanNDecimalPlaces(2) == true)
                ModelState.AddModelError("TooManyDecimals", "The deposit amount cannot have more than 2 decimal places.");
            else if (account == null || account.CustomerID != CustomerID)
                ModelState.AddModelError("InvalidAccount", "Invalid account number. Please input one of your accounts.");

            // If an invalid input is given, pass the inputted amount to the new view call
            if (ModelState.IsValid == false)
            {
                ViewBag.Amount = amount;
                return View();
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
                return View();
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
            else if (amount > 0 == false)
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
                // Transaction for the sending account
                account.Balance -= amount;
                account.Transactions.Add(
                    new Transaction
                    {
                        DestinationAccountNumber = destinationAccountNumber,
                        TransactionType = 'T',
                        Amount = amount,
                        TransactionTimeUtc = DateTime.UtcNow
                    });

                // Transaction for the receiving account
                destAccount.Balance += amount;
                destAccount.Transactions.Add(
                    new Transaction
                    {
                        TransactionType = 'T',
                        Amount = amount,
                        TransactionTimeUtc = DateTime.UtcNow
                    });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Transfer));
            }
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

        public async Task<IActionResult> MyProfile()
        {
            // Loads current customers' data
            var customer = await _context.Customer.FindAsync(CustomerID);

            return View(customer);
        }

        public IActionResult ChangePassword() => View();

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var login = await _context.Login.Where(x => x.CustomerID == CustomerID).FirstAsync();

            // Check if any fields are empty
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                ModelState.AddModelError("EmptyPassword", "The password field is blank.");

            // Checks if LoginID is empty or if password matches hashed value from database
            else if (PBKDF2.Verify(login.PasswordHash, oldPassword) == false)
                ModelState.AddModelError("IncorrectPassword", "Incorrect password, please try again.");

            // Check if new password matched the confirm password field
            else if (newPassword != confirmPassword)
                ModelState.AddModelError("PasswordNotMatching", "This field doesn't match the above field.");

            if (ModelState.IsValid == false)
			{
                return View();
			}
            else
			{
                // TODO : Update hashed password field + save changes

                ViewBag.Success = "Password successfully changed!";
                return View();
            }
		}

        public async Task<IActionResult> EditProfile()
        {
            // Loads current customers' data
            var customer = await _context.Customer.FindAsync(CustomerID);

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(string name, string TFN, string address, string suburb, string state, string postcode, string mobile)
        {
            var customer = await _context.Customer.FindAsync(CustomerID);

            // Attempted to add validation for model attributes (TODO : Fix validation messages)
            if(ModelState.IsValid == true)
            {
                customer.Name = name;
                customer.TFN = TFN;
                customer.Address = address;
                customer.Suburb = suburb;
                customer.State = state;
                customer.Postcode = postcode;
                customer.Mobile = mobile;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyProfile));
            }

            return View(customer);
        }

        public async Task<IActionResult> BillPay()
        {
            // Obtain the customer's accounts
            var accounts = await _context.Account.Where(x => x.CustomerID == CustomerID).ToListAsync();

            List<BillPay> billPays = new();

            // Check for bill pays attached to the customer's accounts and append to a list
            foreach(var account in accounts)
            {
                billPays.AddRange(account.BillPays);
            }

            // Pass all bill pay objects to view
            return View(billPays);
        }

        public IActionResult AddBillPay() => View();

        [HttpPost]
        public async Task<IActionResult> AddBillPay(int accountNumber, int payeeID, decimal amount, DateTime scheduleTimeUtc, char period)
        {
            // Obtain the customer's account
            var account = await _context.Account.FindAsync(accountNumber);

            // Obtain the payee
            var payee = await _context.Payee.FindAsync(payeeID);

            // Checks for amount to be positive
            if (amount > 0 == false)
                ModelState.AddModelError("NegativeAmount", "The amount must be greater than 0.");
            // Checks for decimal places
            else if (amount.MoreThanNDecimalPlaces(2) == true)
                ModelState.AddModelError("TooManyDecimals", "The amount cannot have more than 2 decimal places.");
            // Checks if period is either O or M
            if (period.ToString() != "O" && period.ToString() != "M")
                ModelState.AddModelError("InvalidPeriod", "Period should either be O for one-off or M for monthly.");
            // Checks if specified account is owned by the customer
            if (account == null || account.CustomerID != CustomerID)
                ModelState.AddModelError("InvalidAccount", "Invalid account number. Please input one of your accounts.");
            // Checks if account has sufficient funds
            if (payee == null)
                ModelState.AddModelError("InvalidPayee", "Invalid Payee ID. Please input and existing ID.");
            if (scheduleTimeUtc <= DateTime.UtcNow)
                ModelState.AddModelError("InvalidTime", "Please enter a time in the future.");

            if (ModelState.IsValid == false)
            {
                return View();
            }
            else
            {
                // Add BillPay object associated with the given account
                account.BillPays.Add(
                    new BillPay
                    {
                        PayeeID = payeeID,
                        Amount = amount,
                        ScheduleTimeUtc = scheduleTimeUtc,
                        Period = period
                    });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(BillPay));
            }
        }
    }
}
