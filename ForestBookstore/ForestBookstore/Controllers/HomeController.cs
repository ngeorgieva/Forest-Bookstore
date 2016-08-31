namespace ForestBookstore.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Models;

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            this.ViewBag.NewReleases = this.db.Books.Include(b => b.Author).OrderByDescending(b => b.ReleaseDate).Take(3);

            this.ViewBag.BestSellers = this.db.Books.Include(b => b.Author).Where(b => b.Categories.Any(c => c.Name == "Bestsellers")).OrderByDescending(b => b.ReleaseDate).Take(3);
            return this.View();
        }

        public ActionResult Contact()
        {
            return this.View();
        }
    }
}