using Microsoft.AspNetCore.Mvc;
using MvcBank.Data;
using MvcBank.Models;
using SimpleHashing;

namespace s3717205_a2.Controllers
{
    [Route("/Mcba/SecureLogin")]
    public class LoginController : Controller
    {
        private readonly MvcBankContext _context;

        public LoginController(MvcBankContext context) => _context = context;

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string loginID, string password)
        {
            // Retrieve Login object from database using the given LoginID
            var login = await _context.Login.FindAsync(loginID);

            // Checks if LoginID or password fields are empty or if password matches hashed value from database
            if(login == null || string.IsNullOrEmpty(password) == true || PBKDF2.Verify(login.PasswordHash, password) == false)
            {
                ModelState.AddModelError("LoginFailed", "Invalid username or password, please try again.");
                return View(new Login { LoginID = loginID });
            }

            // Login customer by adding details into session data
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);

            // Successful login directs user to the index page of CustomerController
            return RedirectToAction("Index", "Customer");
        }

        [Route("LogoutUser")]
        public IActionResult Logout()
        {
            // Logout user by clearing session data
            HttpContext.Session.Clear();

            // Logout directs user to home page
            return RedirectToAction("Index", "Home");
        }
    }
}
