namespace ForestBookstore.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Models;
    using Models.DbContext;

    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return this.View(this.db.Categories.Include(c => c.Books).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = this.db.Categories.Include(c => c.Books).Single(c => c.Id == id);
            if (category == null)
            {
                return this.HttpNotFound();
            }
            if (category.Name == "New Releases")
            {
                DateTime sixMonthsAgo = DateTime.Today.AddMonths(-6);
                this.ViewBag.BookList = this.db.Books.Include(b => b.Author).Where(b => DateTime.Compare(b.ReleaseDate, sixMonthsAgo) >= 0).OrderByDescending(b => b.ReleaseDate).ToList();
            }
            else
            {
                this.ViewBag.BookList = this.db.Books.Include(b => b.Categories).Include(b => b.Author).Where(b => b.Categories.Any(c => c.Id == id)).ToList();
            }
            
            return this.View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Categories.Add(category);
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = this.db.Categories.Find(id);
            if (category == null)
            {
                return this.HttpNotFound();
            }
            
            var booksQuery =
                this.db.Books
                .Include(b => b.Author)
                    .Include(b => b.Categories)
                    .Where(b => b.Categories.Any(c => c.Id == id)).AsEnumerable().Select(b => new
                    {
                        BookId = b.Id,
                        Description = $"{b.Name} by {b.Author.Name}"
                    });

            this.ViewBag.BookId = new SelectList(booksQuery, "BookId", "Description");
            return this.View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (this.ModelState.IsValid)
            {
                int bookId = int.Parse(this.Request.Form["BookId"]);
                var book = this.db.Books.Include(b => b.Categories).Single(b => b.Id == bookId);
                if (book != null) book.Categories.Remove(this.db.Categories.Single(c => c.Id == category.Id));
                //this.db.Entry(category).State = EntityState.Modified;
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }
            return this.View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = this.db.Categories.Find(id);
            if (category == null)
            {
                return this.HttpNotFound();
            }
            return this.View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = this.db.Categories.Find(id);
            this.db.Categories.Remove(category);
            this.db.SaveChanges();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}