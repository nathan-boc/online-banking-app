using Microsoft.AspNetCore.Mvc;
using MvcBank.Data;

namespace s3717205_a2.Controllers
{
    public class LoginController : Controller
    {
        private readonly MvcBankContext _context;

        public LoginController(MvcBankContext context) => _context = context;

        public IActionResult Login() => View();
    }
}
