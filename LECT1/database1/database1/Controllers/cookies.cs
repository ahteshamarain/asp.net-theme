using database1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

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


        // user or admin dono ka same ha /// 
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login lg)
        {
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            string fpass = HashPassword(lg.Password);

            var res = db.Logins.FirstOrDefault(x => x.Username == lg.Username || x.Email == lg.Username && x.Password == fpass);

            if (res != null)
            {
                if (res.Roleid==1)
                {

                    //Create the identity for the user
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Sid, res.Id.ToString()),
                    new Claim(ClaimTypes.Name, lg.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                }

                if (res.Roleid==2)
                {
                    //Create the identity for the user
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Sid, res.Id.ToString()),
                    new Claim(ClaimTypes.Name, lg.Username),
                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                }

              
                if (isAuthenticated && res.Roleid==1)
                {
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                if (isAuthenticated && res.Roleid == 2)
                {
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "User");
                }


            }

            else
            {
                return Content("Wrong email and password");

            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }



       
        // ye user ka register ha //////////////////

        [HttpGet]
        public IActionResult Adduser2()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Adduser2(Login lg)
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
                    return RedirectToAction("Index", "Home");

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





        //update password ka kaam ///


      
        [HttpGet]
        public ActionResult regEdit()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult regEdit(string newpass)
        {
            int Id = int.Parse(User.FindFirst(ClaimTypes.Sid)?.Value);
           
            var loginn = db.Logins.Find(Id);

            loginn.Password = HashPassword(newpass);

            db.Update(loginn);
            db.SaveChanges();


            return RedirectToAction("Index", "User");
        }


        [HttpGet]
        public IActionResult AddCategory()
        {
            //ViewBag.Roleid = new SelectList(db.Roles, "Id", "Rname");
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();

            }

            return View();
        }

        public IActionResult ShowCategory()
        {
            var cdata = db.Categories.ToList();
            return View(cdata);
        }


        [HttpGet]
        public IActionResult Deletecat(int Id)
        {

            var deletekadata = db.Categories.Find(Id);
            return View(deletekadata);
        }

        [HttpPost]
        public IActionResult Deletecat(Category cat)
        {


                db.Remove(cat);
                db.SaveChanges();
                return View();
            
           
        }

        [HttpGet]
        public IActionResult Updatecat(int Id)
        {
            //ViewBag.Roleid = new SelectList(db.Roles, "Id", "Rname");
            var deletekadata = db.Categories.Find(Id);
            return View(deletekadata);
        }
        [HttpPost]
        public IActionResult Updatecat(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Update(cat);
                db.SaveChanges();

            }

            return View();
        }

        [HttpGet]
        public IActionResult addpro()
        {
            ViewData["Catid"] = new SelectList(db.Categories, "Id", "Catname");

            return View();
        }

        [HttpPost]
        public IActionResult addpro(Product pr , IFormFile file)
        {
      var imageName = Path.GetFileName(file.FileName);
        string imagePath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Image/");
            string imagevalue = Path.Combine(imagePath, imageName);
            using (var stream = new FileStream(imagevalue, FileMode.create))
            {
                file.CopyTo(stream);
            }
            var dbimage = Path.Combine("/Image/", imageName);
            pr.Picture = dbimage;
            db.Products.Add(pr);
            db.SaveChanges();
            return View();
        }



    }
}
