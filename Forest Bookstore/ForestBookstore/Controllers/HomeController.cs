using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ForestBookstore.Models.DbContext;

namespace ForestBookstore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var db = new BookstoreDbContext();
            var newTestBook = new Book()
            {
                Name = "The book5",
                Description = "Testvane na migraciqta",
                Price = 5,
                CurrentCount = 1,
                CreatedOn = DateTime.Now

            };

            db.Books.Add(newTestBook);
            db.SaveChanges();

            return View();
        }
    }
}
