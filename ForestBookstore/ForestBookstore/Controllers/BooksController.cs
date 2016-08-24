namespace ForestBookstore.Controllers
{
    using System;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Models;
    using Models.DbContext;
    using PagedList;

    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index(string searchString, string searchBy, int? categoryId, int page = 1, int pageSize = 3)
        {
            var books = this.db.Books.Include(b => b.Author).OrderByDescending(b => b.CreatedOn);

            if (categoryId != null)
            {
                var category = this.db.Categories.Find(categoryId);
                if (category.Name == "New Releases")
                {
                    DateTime sixMonthsAgo = DateTime.Today.AddMonths(-6);
                    books = this.db.Books.Include(b => b.Author).Where(b => DateTime.Compare(b.ReleaseDate, sixMonthsAgo) >= 0).OrderByDescending(b => b.ReleaseDate);
                }
                else
                {
                    books = this.db.Books.Include(b => b.Author)
                   .Include(b => b.Categories)
                   .Where(b => b.Categories.Any(c => c.Id == categoryId)).OrderByDescending(b => b.ReleaseDate);
                }
               
            }
            else
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (searchBy == "Name")
                    {
                        books =
                            this.db.Books.Include(b => b.Author)
                                .Include(b => b.Categories)
                                .Where(b => b.Name.Contains(searchString))
                                .OrderByDescending(b => b.ReleaseDate);
                    }
                    else
                    {
                        books =
                            this.db.Books.Include(b => b.Author)
                                .Include(b => b.Categories)
                                .Where(b => b.Author.Name.Contains(searchString))
                                .OrderByDescending(b => b.ReleaseDate);
                    }
                }
            }

            PagedList<Book> model = new PagedList<Book>(books, page, pageSize);

            ViewBag.Categories = this.db.Categories.Include(c => c.Books);

            return View(model);
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = this.db.Books.Include(b => b.Author).Include(b => b.Categories).Single(b => b.Id == id);

            if (book == null)
            {
                return HttpNotFound();
            }

            var categries = this.GetBookCategoriesAsAString(book);
            this.ViewBag.Categories = categries.ToString();
            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            //this.ViewBag.Categories = this.db.Categories.ToList();
            this.PopulateCategoriesDropDownList();
            return this.View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Author,Description,ReleaseDate,Price,CurrentCount,CreatedOn")] Book book, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    byte[]  bytes = new byte[file.ContentLength];
                    int bytesToRead = (int)file.ContentLength;
                    int bytesRead = 0;
                    while (bytesToRead > 0)
                    {
                        int n = file.InputStream.Read(bytes, bytesRead, bytesToRead);
                        if (n == 0) break;
                        bytesRead += n;
                        bytesToRead -= n;
                    }

                    book.Image = bytes;
                }

                Author bookAuthor = null;
                try
                {
                    bookAuthor = db.Authors.Where(a => a.Name == book.Author.Name).Single();
                }
                catch (InvalidOperationException)
                {
                    if (bookAuthor == null)
                    {
                        bookAuthor = db.Authors.Add(new Author
                        {
                            Name = book.Author.Name
                        });

                        db.SaveChanges();
                    }
                }

                book.Author = bookAuthor;
                book.AuthorId = bookAuthor.Id;

                try
                {
                    int categoryId = int.Parse(this.Request.Form["CategoryId"]);
                    book.Categories.Add(this.db.Categories.Single(c => c.Id == categoryId));
                }
                catch (FormatException)
                {
                    //No selected category to add;
                }
                
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Include(b => b.Author).Single(b => b.Id == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            this.PopulateCategoriesDropDownList();
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Image,Author,Description,ReleaseDate,Price,CurrentCount,CreatedOn")] Book book)
        {
            if (ModelState.IsValid)
            {
                Author bookAuthor = null;
                try
                {
                    bookAuthor = db.Authors.Where(a => a.Name == book.Author.Name).Single();
                }
                catch (InvalidOperationException)
                {
                    if (bookAuthor == null)
                    {
                        bookAuthor = db.Authors.Add(new Author
                        {
                            Name = book.Author.Name
                        });
                    }
                }

                var currentBook = db.Books.Where(b => b.Id == book.Id).Single();
                currentBook.Author = bookAuthor;
                currentBook.AuthorId = bookAuthor.Id;
                currentBook.Name = book.Name;
                currentBook.Price = book.Price;
                currentBook.ReleaseDate = book.ReleaseDate;
                currentBook.Description = book.Description;
                currentBook.CurrentCount = book.CurrentCount;
                currentBook.CreatedOn = book.CreatedOn;

                try
                {
                    int categoryId = int.Parse(this.Request.Form["CategoryId"]);
                    currentBook.Categories.Add(this.db.Categories.Single(c => c.Id == categoryId));
                }
                catch (FormatException)
                {
                    //No selected category to add
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Include(b => b.Author).Single(b => b.Id == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Books/BookImage/
        public ActionResult BookImage(int id)
        {
            //this.ViewBag.Image = img;
            Book book = this.db.Books.Find(id);
            return this.View(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string GetBookCategoriesAsAString(Book book)
        {
            var categries = new StringBuilder();
            if (book.ReleaseDate >= DateTime.Today.AddMonths(-6))
            {
                categries.Append("New Release" + ", ");
            }
            foreach (var c in book.Categories)
            {
                categries.Append(c.Name + ", ");
            }
            if (categries.Length > 0)
            {
                categries.Remove(categries.Length - 2, 2);
                categries.Append(" | ");
            }
            return categries.ToString();
        }

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoryQuery = from c in db.Categories
                                orderby c.Name
                                select c;
            this.ViewBag.CategoryId = new SelectList(categoryQuery, "Id", "Name", selectedCategory);
        }
    }
}