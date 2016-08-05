﻿using System;
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

    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index()
        {
            var books = this.db.Books.Include(b => b.Author).OrderByDescending(b => b.CreatedOn);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
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
        public ActionResult Create([Bind(Include = "Id,Name,Author,Description,Id,Price,CurrentCount,CreatedOn")] Book book, HttpPostedFileBase file)
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
                db.Entry(book).State = EntityState.Modified;
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
            this.ViewBag.Id = new SelectList(categoryQuery, "Id", "Name", selectedCategory);
        }
    }
}
