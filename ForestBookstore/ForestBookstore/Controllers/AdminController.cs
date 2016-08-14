using ForestBookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForestBookstore.Models.DbContext;

namespace ForestBookstore.Controllers
{
    public class AdminController : Controller
    { 
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CheckNewOrders()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OrdersMade> allOrders;

            if(db.OrdersMades.Where(order => order.Status == false).Count() == 0)
            {
                allOrders = null;
            }
            else
            {
                allOrders = db.OrdersMades
                                .Where(order => order.Status == false)
                                .OrderByDescending(order => order.OrderedOn)
                                .ToList();

                foreach (var order in allOrders)
                {
                    foreach (var book in order.BooksForOrder)
                    {
                        book.Book.Author = db.Authors.Find(book.Book.AuthorId);
                    }
                }            
            }
     
            return View(allOrders);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult IsShipped(int orderId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var currOrder = db.OrdersMades.Find(orderId);
            currOrder.Status = true;
            db.SaveChanges();

            return RedirectToAction("CheckNewOrders");
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult IsNotShipped(int orderId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var currOrder = db.OrdersMades.Find(orderId);
            currOrder.Status = false;
            db.SaveChanges();

            return View();
        }
    }
}