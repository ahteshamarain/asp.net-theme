using database1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;

namespace database1.Controllers
{
    public class User : Controller
    {
        EmpContext db = new EmpContext();
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Adduser()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Adduser(Login lg)
        {
            if (ModelState.IsValid)
            {
                var checkemail = db.Logins.Where(x => x.Username == lg.Username || x.Email == lg.Email);
                if (checkemail.Count() == 0)
                {
                   lg.Roleid = 2;
                    lg.Password = HashPassword(lg.Password);
                    db.Logins.Add(lg);
                    db.SaveChanges();
                    return RedirectToAction("Index","Home");

                }
                else
                {
                    ViewBag.msg = "Email or Username already registered, please provide another email";
                }

                
            }
            return View();



        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }





    }
}