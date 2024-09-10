using Microsoft.AspNetCore.Mvc;

namespace login.Controllers
{
    public class CookieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
