using dashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        mydbContext db = new mydbContext();
        private string totalprice;
        private object o;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Product> limitedProducts = db.Products.Take(8).ToList();
            return View(limitedProducts);
     
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }





        public IActionResult contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult contact(IFormCollection f)
        {

            var name = f["email"];

            var mes = f["mes"];

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("aptech2110c1@gmail.com", "kyteusufuzfwraeu");

            MailMessage msg = new MailMessage("aptech2110c1@gmail.com", name);
            msg.Subject = "Contact";
            msg.Body = mes;

            // msg.Attachments.Add(new Attachment(PathToAttachment));
            client.Send(msg);

            ViewBag.message = "Mail sent successfully,";


            return View();
        }


        [HttpPost]
        public IActionResult Search(string xyz)
        {

            // var query = from x in db.Visitors.Where(x => x.Id == xyz) select x;
            var resultt = db.Products.Where(x => x.Namee.Contains(xyz));
            //var resultt = db.Visitors.Where(x => EF.Functions.Like(x.Namee, "a%"));
            return View("Index", resultt.ToList());

        }

    }
}
