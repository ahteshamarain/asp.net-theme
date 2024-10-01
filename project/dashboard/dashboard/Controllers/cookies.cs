using dashboard.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace dashboard.Controllers
{
    public class cookies : Controller
    {
        mydbContext db = new mydbContext();
        //logout ka view banye ga
        //theme client panal ki lgi ge
        //admin panal m hoga show or delete

        //registration ka page client panal p show hoga login b

        //user edit krega apny apko

        
        public IActionResult index()
        {

            return View(db.Logins.ToList());
        }

        
        public IActionResult registration()
        {

            return View();
        }


        [HttpPost]

       
        public IActionResult registration(Login lg)
        {
            if (ModelState.IsValid)
            {
                lg.RoleId = 2;
                db.Logins.Add(lg);
                db.SaveChanges();
            }


            ViewBag.mes = "User Registered Successfully";
            return View();
        }

        //change password krna ha //



        [HttpGet]
        public IActionResult changepassword()
        {
            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;

            var abcc = db.Logins.FirstOrDefault(x => x.Id == Convert.ToInt32(abc));


            ViewBag.ps = abcc.Password;

            return View();
        }

        [HttpPost]
        public IActionResult changepassword(IFormCollection f)
        {
            var newp = f["np"];

            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;

            var abcc = db.Logins.FirstOrDefault(x => x.Id == Convert.ToInt32(abc));

            abcc.Password = newp;
            db.Update(abcc);
            db.SaveChanges();

            ViewBag.mes = "Password has been changed";
            return View();
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

       
        public IActionResult Login(Login lg)
        {
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            var res = db.Logins.FirstOrDefault(x => x.Name == lg.Name && x.Password == lg.Password);
            if (res != null)
            {


                if (res.RoleId == 1)
                {

                    //Create the identity for the user
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, lg.Name),
                    new Claim(ClaimTypes.Sid, res.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                }

                if (res.RoleId == 2)
                {
                    //Create the identity for the user
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, lg.Name),
                     new Claim(ClaimTypes.Sid, res.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                }

                if (isAuthenticated&& res.RoleId==1)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Admin");
                }


               else if (isAuthenticated && res.RoleId == 2)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("mypro", "User");
                }


            }


            return View();
        }



        
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}