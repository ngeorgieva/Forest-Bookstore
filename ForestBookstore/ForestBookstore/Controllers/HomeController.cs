using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForestBookstore.Controllers
{
    using System.Data.Entity;
    using Models;

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            this.ViewBag.NewReleases = this.db.Books.Include(b => b.Author).OrderByDescending(b => b.CreatedOn).Take(3);
            this.ViewBag.BestSellers = this.db.Books.Include(b => b.Author).OrderBy(b => b.CreatedOn).Take(3);
            return this.View();
        }
    }
}