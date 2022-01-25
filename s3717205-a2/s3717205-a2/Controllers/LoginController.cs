using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // Use eager loading to retrieve Login data from database
            var login = await _context.Login.FirstOrDefaultAsync(login => login.LoginID == loginID);

            // Checks if password field is empty
            if (string.IsNullOrEmpty(password) == true)
            {
                ModelState.AddModelError("EmptyPassword", "The password field is blank.");
                return View(new Login { LoginID = loginID });
            }
            // Checks if LoginID is empty or if password matches hashed value from database
            else if (login == null || PBKDF2.Verify(login.PasswordHash, password) == false)
            {
                ModelState.AddModelError("LoginFailed", "Invalid username or password, please try again.");
                return View(new Login { LoginID = loginID });
            }

            // Login customer by adding details into session data
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);

            // Successful login directs user to the index page of CustomerController
            return RedirectToAction("Dashboard", "Customer");
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
