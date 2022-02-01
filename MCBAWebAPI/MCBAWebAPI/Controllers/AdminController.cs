using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;

using MvcBank.Data;
using MvcBank.Models;
using MvcBank.Filters;
using MvcBank.Utilities;

namespace s3717205_a2.Controllers
{
    [AuthorizeAdmin]
    public class AdminController : Controller
    {
        private readonly MvcBankContext _context;

        // Retrieved from session data
        private int adminKey => HttpContext.Session.GetInt32("Admin").Value;

        public AdminController(MvcBankContext context) => _context = context;

        public IActionResult Index()
        {
            return View();
        }

    }
}
