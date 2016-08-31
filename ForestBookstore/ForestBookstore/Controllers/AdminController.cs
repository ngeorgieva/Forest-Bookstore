namespace ForestBookstore.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using Models.DbContext;

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    { 
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CheckNewOrders()
        {
            var getUnsentOrders = this.GetOrders(false);
     
            return this.View(getUnsentOrders);
        }

        [HttpGet]
        public ActionResult CheckSentOrders()
        {
            var getSentOrders = this.GetOrders(true);      

            return this.View(getSentOrders);
        }

        [HttpPost]
        public ActionResult IsShipped(int orderId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var currOrder = db.OrdersMades.Find(orderId);
            currOrder.Status = true;
            db.SaveChanges();

            return this.RedirectToAction("CheckNewOrders");
        }
        
        [HttpPost]
        public ActionResult IsNotShipped(int orderId)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var currOrder = db.OrdersMades.Find(orderId);
            currOrder.Status = false;
            db.SaveChanges();

            return this.View("CheckSentOrders");
        }

        private List<OrdersMade> GetOrders(bool getSentOrders)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            List<OrdersMade> allOrders;

            if (db.OrdersMades.Where(order => order.Status == getSentOrders).Count() == 0)
            {
                allOrders = null;
            }
            else
            {
                allOrders = db.OrdersMades
                                .Where(order => order.Status == getSentOrders)
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

            return allOrders;
        }
    }
}