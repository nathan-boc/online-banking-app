using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace s3717205_a2.Controllers
{
    public class CustomerController : Controller
    {
        // GET: HomeController1
        public ActionResult Index() => View();
        

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
