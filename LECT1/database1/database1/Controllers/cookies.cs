using database1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace database1.Controllers
{
    public class cookies : Controller
    {
        EmpContext db = new EmpContext();

        public IActionResult Index()
        {
            return View();
        }
        // ADMIN KA KAAM HA ////
        [HttpGet]
        public IActionResult Adduser()
        {
            ViewBag.Roleid = new SelectList(db.Roles, "Id", "Rname");
            return View();
        }
        [HttpPost]
        public IActionResult Adduser(Login lg)
        {
            if(ModelState.IsValid)
            {
                db.Logins.Add(lg);
                db.SaveChanges();

            }

            return View();
        }

        public IActionResult Showuser()
        {
            var userkadata=db.Logins.Include(p => p.Role);
            return View(userkadata.ToList());
        }


        [HttpGet]
        public IActionResult Deleteuser(int Id)
        {

            var deletekadata = db.Logins.Find(Id);
            return View(deletekadata);
        }

        [HttpPost]
        public IActionResult Deleteuser(Login lg)
        {
          
            if(ModelState.IsValid)
            {

                db.Logins.Remove(lg);
                db.SaveChanges();

            }
            return View("Showuser");
        }




        [HttpGet]
        public IActionResult Updateuser(int Id)
        {
            ViewBag.Roleid = new SelectList(db.Roles, "Id", "Rname");
            var deletekadata = db.Logins.Find(Id);
            return View(deletekadata);
        }
        [HttpPost]
        public IActionResult Updateuser(Login lg)
        {
            if (ModelState.IsValid)
            {
                db.Logins.Update(lg);
                db.SaveChanges();

            }

            return View();
        }













    }
}
