using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcBank.Data;
using MvcBank.Models;
using SimpleHashing;

namespace s3717205_a2.Controllers
{
    public class LoginController : Controller
    {
        public LoginController(MvcBankContext context) { }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Checks if password field is empty
            if (username == "admin" && password == "admin")
            {
                // Login admin by adding details into session data
                HttpContext.Session.SetInt32("Admin", 1);

                // Successful login directs user to the index page of CustomerController
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError("IncorrectInput", "Incorrect username or password.");
                return View();
            }
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
