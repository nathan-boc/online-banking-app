using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace s3717205_a2.Controllers
{
    public class CustomerController : Controller
    {
        // GET: HomeController1
        public ActionResult Index() => View();
        

        // GET: HomeController1/Details/5
        public ActionResult Deposit()
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public ActionResult Withdraw()
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public ActionResult Transfer()
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public ActionResult MyStatements()
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public ActionResult Profile()
        {
            return View();
        }

    }
}
