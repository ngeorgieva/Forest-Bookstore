using ForestBookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForestBookstore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CheckNewOrders()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var allOrders = db.OrdersMades
                .OrderBy(order => order.Status == false)
                .ThenBy(order => order.OrderedOn)
                .ToList();

            return View(allOrders);
        }
        //TODO
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult IsShipped(int orderId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var currOrder = db.OrdersMades.Find(orderId);
            currOrder.Status = true;
            db.SaveChanges();

            return View();
        }
        //TODO
        [HttpGet]
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