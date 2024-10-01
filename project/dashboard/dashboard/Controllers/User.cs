using dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dashboard.Controllers
{
    public class User : Controller
    {


        mydbContext db = new mydbContext();
        public IActionResult Index()
        {
            
            return View(db.Products.ToList());
        }

        public IActionResult showproducts(int Id)
        {

            var resultt = db.Products.Where(x => x.CatId.Equals(Id));
            return View(resultt);

          
        }


        public IActionResult mypro()
        {

            return View(db.Products.ToList());
        }


        [Authorize]
        public IActionResult cart(int Id, Cart cc)
        {

            cc.Id = 0;
            String abc = User.FindFirst(ClaimTypes.Sid)?.Value;

            var upcart = db.Carts.FirstOrDefault(x => x.Userid == Convert.ToInt32(abc) && x.Proid == Id);


            var cartt = db.Products.Where(x => x.Id.Equals(Id));

            if (upcart == null)
            {
                foreach (var c in cartt)
                {


                    cc.Userid = Convert.ToInt32(abc);
                    cc.Proid = c.Id;
                    cc.Qty = 1;
                   

                    cc.Proprice = c.Price;


                }
                db.Carts.Add(cc);

            }

            else if (upcart.Qty < 5)
            {
                foreach (var c in cartt)
                {

                    upcart.Qty += 1;
                    upcart.Proprice = c.Price * upcart.Qty;

            

                }
                db.Carts.Update(upcart);
            }
            else
            {
                TempData["already"] = "out of range";

            }

            db.SaveChanges();




            return RedirectToAction("cartshow");
        }

        public IActionResult cartshow()
        {

            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;
            //var cartt = db.Carts.ToList();
            var cartt = db.Carts.Where(x => x.Userid.Equals(Convert.ToInt32(abc))).Include(x => x.Pro);
            ViewBag.Total = db.Carts.Where(x => x.Userid == Convert.ToInt32(abc)).Sum(x => x.Proprice);
            return View(cartt);


        }
        public ActionResult cartEdit(IFormCollection f)
        {
            var fid = f["ii"];
            var fqt = f["qq"];

            int ffid = Convert.ToInt32(fid);
            int ffqt = Convert.ToInt32(fqt);

            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;

            var upcart = db.Carts.FirstOrDefault(x => x.Userid == Convert.ToInt32(abc) && x.Id == ffid);


            var pro = db.Products.FirstOrDefault(x => x.Id == upcart.Proid);


            if (upcart != null)
            {

                upcart.Qty = ffqt;
                int ffprice = Convert.ToInt32(pro.Price);
                int fprice = ffprice * ffqt;
                upcart.Proprice = fprice;
                db.Carts.Update(upcart);
                db.SaveChanges();

            }
            else
            {
                TempData["sorry"] = "sorry no data";
            }

            return RedirectToAction("cartshow");
        }


        public IActionResult cartDelete(IFormCollection f)
        {
            var tid = f["ij"];

            int ffid = Convert.ToInt32(tid);
            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;

            var upcart = db.Carts.FirstOrDefault(x => x.Userid == Convert.ToInt32(abc) && x.Id == ffid);
            if (upcart != null)
            {
                db.Carts.Remove(upcart);
                db.SaveChanges();
            }

            return RedirectToAction("cartshow");
        }

        public IActionResult checkout()
        {
            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;

            var cartt = db.Carts.Where(x => x.Userid.Equals
(Convert.ToInt32(abc))).Include(x => x.Pro);


            ViewBag.Total = db.Carts.Where(x => x.Userid == Convert.ToInt32(abc)).Sum(x => x.Proprice);


            return View();

        }
       

        [HttpPost]
        public IActionResult checkout(IFormCollection f, Order o, OrderDetail ood)
        {

            var name = f["cname"];
            var totalprice = f["tp"];
            var phone = f["cphone"];
            var address = f["caddress"];
            var date = f["cdate"];

            string abc = User.FindFirst(ClaimTypes.Sid)?.Value;

            o.Userid = Convert.ToInt32(abc);
            o.Date = DateTime.Now.ToString("dd-mm-yyyy"); 
            o.CustomerName = name;
            o.TotalAmount = Convert.ToInt32(totalprice);
            o.PhoneNumber = phone;
            o.Address = address;
            db.Add(o);
            db.SaveChanges();
            List<Cart> carts = db.Carts.Where(cart => cart.Userid == Convert.ToInt32(abc)).Include(x => x.Pro).ToList();
            foreach (var aa in carts)
            {
                // Create a new instance of OrderDetail for each cart item
                OrderDetail od = new OrderDetail();

                od.Userid = Convert.ToInt32(abc);
                od.OrderId = o.Id;
                od.ProId = aa.Proid;
                od.ProName = aa.Pro.Namee;
                od.Qty = aa.Qty;
                od.Proprice = aa.Proprice;
                od.Date = DateTime.Now.ToString("dd-mm-yyyy"); ;

                db.Add(od);
                db.Carts.Remove(aa);
            }



                db.SaveChanges(); // Save all the changes after the loop
             


                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("aptech2110c1@gmail.com", "kyteusufuzfwraeu");

                MailMessage msg = new MailMessage("aptech2110c1@gmail.com", name);
                msg.Subject = "Login Activated";
                msg.Body = "Dear User,Your Order Details is " + "Order_ Id  "+ o.Id + " & Price  " +  totalprice + " be ready with this amount";

                // msg.Attachments.Add(new Attachment(PathToAttachment));
                client.Send(msg);

                ViewBag.message = "Order placed & Mail sent successfully,";

            

            return View();
        }

  
        //ye client panal p hoga show 
        public IActionResult profile()
        {

            // Retrieve OrderDetails from DB
            List<OrderDetail> orderDetail = db.OrderDetails.ToList();
            List<Order> order = db.Orders.ToList();
            List<Product> product = db.Products.ToList();


            string abc = User.FindFirst(ClaimTypes.Sid)?.Value; ;
            // Filter based on UserId. Order by date. Select and group by relevant columns.
            IEnumerable<PurchasesViewModel> purchases =
                from o in order
                join od in orderDetail on o.Id equals od.OrderId
                join p in product on od.ProId equals p.Id
                where o.Userid == Convert.ToInt32(abc)

                select new { o.Date, od.Id, p.Picture, p.Namee, p.Descp, od.ProId } into y
                group y by new { y.Picture, y.Namee, y.Descp, y.ProId } into grp
                select new PurchasesViewModel
                {

                    ImageLink = grp.Key.Picture,
                    Name = grp.Key.Namee,
                    Quantity = grp.Count(),
                    Description = grp.Key.Descp,

                    ProductId = (int)grp.Key.ProId
                };

            if (purchases.ToList().Count == 0) // If no purchases, send info to View to display "no past purchases"
            {
                ViewData["HavePastOrders"] = false;

                return View();
            }

            ViewData["HavePastOrders"] = true;

            return View(purchases.ToList());
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


        // profile wala kaam ha ye b knra ha order ka table use kr k



        //public IActionResult Index1()
        //{
        //    //db.Emps.ToList();
        //    string emaill = User.Identity.Name;
        //    var faculties = db.Emps.Where(f => f.Name == emaill);
        //    return View(faculties.ToList());



        //}




 