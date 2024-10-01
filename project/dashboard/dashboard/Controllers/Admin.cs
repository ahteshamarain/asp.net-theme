using dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace dashboard.Controllers
{
    public class Admin : Controller
    {
        //category add update delete krwani ha//

        mydbContext db = new mydbContext();


        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            return View(db.Logins.ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ShowUser()
        {
            return View(db.Logins.ToList());
        }
        //[HttpGet]
        //public IActionResult addvisitors()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public IActionResult addvisitors(Visitor srk)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Visitors.Add(srk);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var data = db.Visitors.Find(id);
        //    return View(data);
        //}

        //[HttpPost]
        //public IActionResult Edit(int id, Visitor vs)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Update(vs);
        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("Index");
        //}

        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    var data = db.Visitors.Find(id);
        //    return View(data);
        //}

        //[HttpPost]
        //public IActionResult Delete(int id, Visitor vs)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Remove(vs);
        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("Index");
        //}
        //[HttpGet]
        //public IActionResult Detail(int id)
        //{
        //    // var data = db.Visitors.FirstOrDefault(m => m.Id == id);

        //    var t = (from data in db.Visitors where data.Id == id select data).FirstOrDefault();


        //    return View(t);
        //}



        //visitor ki jaga product search krwengy//
        [HttpPost]
        public IActionResult Search(string xyz)
        {

            // var query = from x in db.Visitors.Where(x => x.Id == xyz) select x;
            var resultt = db.Visitors.Where(x => x.Namee.Contains(xyz));
            //var resultt = db.Visitors.Where(x => EF.Functions.Like(x.Namee, "a%"));
            return View("Index", resultt.ToList());

        }







        //catg insert update delete hogi show b sb admin panal p


        //order k dono table show hongy admin paanl p

        // ABOUT Product DATA //

        [Authorize(Roles = "Admin")]
        public IActionResult product()
        {
            var mydbContext =db.Products.Include(p => p.Cat);
            return View(mydbContext.ToList());
        }


        [HttpGet]

        //[Authorize(Roles = "Admin")]
        public IActionResult addpro()
        {
            ViewData["CatId"]= new SelectList(db.Catgs, "Id", "Namee");

            return View();
        }
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult addpro(Product pr, IFormFile file)
        {
            var imageName = Path.GetFileName(file.FileName);
            string imagePath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Image/");
            string imagevalue = Path.Combine(imagePath, imageName);
            using (var stream = new FileStream(imagevalue, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            var dbimage = Path.Combine("/Image/", imageName);
            pr.Picture = dbimage;
            db.Products.Add(pr);
            db.SaveChanges();

            ViewData["CatId"]= new SelectList(db.Catgs, "Id", "Namee");
            return RedirectToAction("product");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult proDelete(int id)
        {
            var data = db.Products.Find(id);
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult proDelete(int id, Product vs)
        {
            if (ModelState.IsValid)
            {
                db.Remove(vs);
                db.SaveChanges();
            }

            return RedirectToAction("product");
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult proupdate(int id)
        {


            ViewData["CatId"] = new SelectList(db.Catgs, "Id", "Namee");
            var data = db.Products.Find(id);
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult proupdate(int id, Product vs,IFormFile file , string hid)
        {
            if (ModelState.IsValid)
            {
                var dbimage="";
                if (file != null && file.Length > 0)
                {
                    var imageName = Path.GetFileName(file.FileName);
                    string imagePath = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/Image/");
                    string imagevalue = Path.Combine(imagePath, imageName);
                    using (var stream = new FileStream(imagevalue, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                     dbimage = Path.Combine("/Image/", imageName);
                    vs.Picture = dbimage;
                    db.Update(vs);
                    db.SaveChanges();

                }
               else 
                {
                    vs.Picture = hid;
                    db.Update(vs);
                    db.SaveChanges();

                }     
                
            }
            ViewData["CatId"] = new SelectList(db.Catgs, "Id", "Namee");
            return RedirectToAction("product");
        }

        //product show index client m hogi mypro ka page client panale ka ha

        // product show admin panal m hoga

        //order or order detail ka show dono admin panal m hoga




      
        public IActionResult Detailuser(int Id)
        {
            var mydbContext = db.Products.Where(x => x.Id == Id).Include(p => p.Cat);
            return View(mydbContext.FirstOrDefault());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult order()
        {
       
            return View(db.Orders.Include(p => p.User).ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult orderDetail()
        {

            return View(db.OrderDetails.Include(x => x.Pro).Include(x => x.User).Include(x => x.Order).ToList());
        }

        public IActionResult semail(int Id)
        {


            var mydbContext = db.Orders.FirstOrDefault(x => x.Id == Id);

            string em = mydbContext.CustomerName;


            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("aptech2110c1@gmail.com", "kyteusufuzfwraeu");

            MailMessage msg = new MailMessage("aptech2110c1@gmail.com", em);
            msg.Subject = "Login Activated";
            msg.Body = "Dear User,Your Order has been shipped ";

            // msg.Attachments.Add(new Attachment(PathToAttachment));
            client.Send(msg);

            ViewBag.message = "mail sent";

            return Content("Email sent");
        }



        public IActionResult aemail(int Id)
        {


            var mydbContext = db.Orders.FirstOrDefault(x => x.Id == Id);

            string em = mydbContext.CustomerName;


            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("aptech2110c1@gmail.com", "kyteusufuzfwraeu");

            MailMessage msg = new MailMessage("aptech2110c1@gmail.com", em);
            msg.Subject = "Login Activated";
            msg.Body = "Dear User,Your Order is on Process ";

            // msg.Attachments.Add(new Attachment(PathToAttachment));
            client.Send(msg);

            ViewBag.message = "mail sent";

            return Content("Email sent");
        }


     

    }
}
