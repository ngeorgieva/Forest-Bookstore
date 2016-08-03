using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForestBookstore.Controllers
{
    using Models;

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Books.Where(b => b.Categories.Any(c => c.Name == "New releases")));
        }
    }
}