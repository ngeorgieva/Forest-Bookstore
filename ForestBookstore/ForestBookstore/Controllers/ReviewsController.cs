﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ForestBookstore.DataLayer.Models;
using ForestBookstore.Models;

namespace ForestBookstore.Controllers
{
    using System.Text;
    using Classes;
    using Models.DataLayer;
    using Models.DbContext;

    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reviews
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var reviews = db.Reviews.Include(r => r.Author).Include(r => r.Book.Author);
            return View(reviews.ToList());
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            return this.PartialView("Create");
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text,Date,Book")] Review review, int? id)
        {
            if (ModelState.IsValid)
            {
                review.Author = this.db.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Book book = this.db.Books.Find(id);
                review.Book = book;
                this.db.Reviews.Add(review);
                this.db.SaveChanges();
                return this.RedirectToAction("Details", "Books", new { id = id });
            }

            return this.PartialView(review);
        }

        // GET: Reviews/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "PersonName", review.AuthorId);

            var booksQuery =
                this.db.Books
                .Include(b => b.Author)
                    .Include(b => b.Categories)
                    .AsEnumerable().Select(b => new
                    {
                        BookId = b.Id,
                        Description = $"{b.Name} by {b.Author.Name}"
                    });
            this.ViewBag.BookId = new SelectList(booksQuery, "BookId", "Description", review.BookId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,Date,AuthorId,BookId")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "PersonName", review.AuthorId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "Name", review.BookId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Include(r => r.Book.Author).FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}