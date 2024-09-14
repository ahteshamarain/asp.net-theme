using Microsoft.AspNetCore.Mvc;

namespace database1.Controllers
{
    public class User : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
