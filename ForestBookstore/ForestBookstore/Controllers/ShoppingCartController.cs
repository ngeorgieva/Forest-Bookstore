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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ForestBookstore.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ShoppingCartController()
        {
        }

        public ShoppingCartController(ApplicationUserManager userManager)
        {
            this.UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); ;
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET ShoppingCart
        [Authorize]
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            ShoppingCartBookViewModel booksInCart = new ShoppingCartBookViewModel(new List<CartLine>());
            booksInCart.Books = db.BooksInBaskets.Where(b => b.UserId == currentUser.Id)
                .Select(b => new CartLine()
                {
                    UserId = b.UserId,
                    BookId = b.BookId,
                    Book = b.Book,
                    Count = b.Count
                })
                .ToList();
                  
            return View(booksInCart);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ShoppingCartBookViewModel cart)
        {
            if(cart == null)
            {
                return RedirectToAction("Index");
            }
        
            return this.RedirectToAction("ShippingDetails");
        }

        // POST: ShoppingCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            BooksInBasket booksInBasket = db.BooksInBaskets
                .Where(bi => bi.BookId == id && bi.User.Id == user.Id).Single();
            db.BooksInBaskets.Remove(booksInBasket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
