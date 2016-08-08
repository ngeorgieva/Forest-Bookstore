using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ForestBookstore.Models;
using ForestBookstore.Models.DbContext;

namespace ForestBookstore.Controllers
{
    using System.IO;
    using System.Text;
    using PagedList;

    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            var books = this.db.Books.Include(b => b.Author).OrderByDescending(b => b.CreatedOn);

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            string searchType = this.Request["searchBy"];
            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchType.Equals("Name"))
                {
                    books =
                        this.db.Books.Include(b => b.Author)
                            .Include(b => b.Categories)
                            .Where(b => b.Name.Contains(searchString))
                            .OrderByDescending(b => b.CreatedOn);
                }
                else
                {
                    books =
                        this.db.Books.Include(b => b.Author)
                            .Include(b => b.Categories)
                            .Where(b => b.Author.Name.Contains(searchString))
                            .OrderByDescending(b => b.CreatedOn);
                }
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(books.ToPagedList(pageNumber, pageSize));
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
            var categries = new StringBuilder();
            foreach (var c in book.Categories)
            {
                categries.Append(c.Name + ", ");
            }
            categries.Remove(categries.Length - 2, 2);
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
        public ActionResult Create([Bind(Include = "Id,Name,Author,Description,Price,CurrentCount,CreatedOn")] Book book, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var fileName = "";
                byte[] bytes;
                int bytesToRead;
                int bytesRead;

                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);
                    bytes = new byte[file.ContentLength];
                    bytesToRead = (int)file.ContentLength;
                    bytesRead = 0;
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
                    }
                }               

                book.Author = bookAuthor;
                book.AuthorId = bookAuthor.Id;

                int categoryId = int.Parse(this.Request.Form["CategoryId"]);
                book.Categories.Add(this.db.Categories.Single(c => c.Id == categoryId));
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
        public ActionResult Edit([Bind(Include = "Id,Name,Image,Author,Description,Price,CurrentCount,CreatedOn")] Book book)
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
                currentBook.Description = book.Description;
                currentBook.CurrentCount = book.CurrentCount;
                currentBook.CreatedOn = book.CreatedOn;

                int categoryId = int.Parse(this.Request.Form["CategoryId"]);
                currentBook.Categories.Add(this.db.Categories.Single(c => c.Id == categoryId));

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

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoryQuery = from c in db.Categories
                                orderby c.Name
                                select c;
            this.ViewBag.CategoryId = new SelectList(categoryQuery, "Id", "Name", selectedCategory);
        }
    }
}
