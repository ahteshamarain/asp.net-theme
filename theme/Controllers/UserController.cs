using System.Net;
using System.Net.Mail;

using Microsoft.AspNetCore.Mvc;

namespace theme.Controllers
{
    public class UserController : Controller
    {
      

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string email , string mess)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;

            //ptmaptech@gmail.com    "ylkflsuagqvtshad"
            client.Credentials = new NetworkCredential("ahteshamarain3@gmail.com", "vleemsjecrsslgaj");

            MailMessage msg = new MailMessage(email, "ahteshamarain3@gmail.com");
            msg.Subject = "Feedback";
            msg.Body = mess;

            // msg.Attachments.Add(new Attachment(PathToAttachment));
            client.Send(msg);

            ViewBag.message = "Mail sent successfully, Role assigned";

            return View();
        }
    }
}
