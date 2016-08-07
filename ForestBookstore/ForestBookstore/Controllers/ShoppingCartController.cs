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
            var cartCount = db.BooksInBaskets.Count();

            if(cartCount > 0)
            {
                booksInCart.Books = db.BooksInBaskets.Where(b => b.UserId == currentUser.Id)
                    .Select(b => new CartLine()
                    {
                        UserId = b.UserId,
                        BookId = b.BookId,
                        Book = b.Book,                 
                        Count = b.Count
                    })
                    .ToList();
            }

            foreach (var book in booksInCart.Books)
            {
                book.Book.Author = db.Authors.Find(book.Book.AuthorId);
            }

            return View(booksInCart);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            //Validation TODO
            //if (this.Session["cartCount"] == null || (int)this.Session["cartCount"] == 0)
            //{
            //    return RedirectToAction("Index");
            //}
        
            var user = UserManager.FindById(User.Identity.GetUserId());

            ShipmentDetailsViewModel currentShipment = new ShipmentDetailsViewModel
            {
                PersonName = user.PersonName,
                Address = user.Address,
                Town = user.Town,
                Phone = user.Phone
            };

            this.Session["PlacingOrder"] = true;

            return View(currentShipment);
        }

        [Authorize]
        public ActionResult Summary(ShipmentDetailsViewModel shipmentDetails)
        {
            if (shipmentDetails == null || this.Session["PlacingOrder"] == null)
            {
                return RedirectToAction("Index");
            }

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

            this.Session["PersonName"] = shipmentDetails.PersonName;
            this.Session["Address"] = shipmentDetails.Address;
            this.Session["Town"] = shipmentDetails.Town;
            this.Session["Phone"] = shipmentDetails.Phone;

            ShipmentViewModel shipment = new ShipmentViewModel
            {
                Cart = booksInCart,
                Details = shipmentDetails
            };

            return View(shipment);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Completed()
        {
            if (this.Session["PersonName"] == null 
                && this.Session["Address"] == null
                && this.Session["Town"] == null
                && this.Session["Phone"] == null)
            {
                if(this.Session["PlacingOrder"] == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Checkout");
                }
            }

            bool orderSuccessful = true;

            try
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                ApplicationDbContext db = new ApplicationDbContext();
                OrdersMade order = new OrdersMade
                {
                    Status = false,
                    UserId = User.Identity.GetUserId(),
                    PersonName = this.Session["PersonName"] as string,
                    Town = this.Session["Town"] as string,
                    Address = this.Session["Address"] as string,
                    Phone = this.Session["Phone"] as string
                };

                db.OrdersMades.Add(order);

                ShoppingCartBookViewModel booksInCart = new ShoppingCartBookViewModel(new List<CartLine>());
                booksInCart.Books = db.BooksInBaskets.Where(b => b.UserId == user.Id)
                    .Select(b => new CartLine()
                    {
                        UserId = b.UserId,
                        BookId = b.BookId,
                        Book = b.Book,
                        Count = b.Count
                    })
                    .ToList();

                foreach (var book in booksInCart.Books)
                {
                    db.BooksForOrders.Add(new BooksForOrder
                    {
                        BookId = book.BookId,
                        Count = book.Count,
                        OrdersMade = order
                    });

                    var currentBookId = book.BookId;
                    var currentBook = db.Books.Find(currentBookId);
                    currentBook.CurrentCount -= 1;
                    currentBook.Author = db.Authors.Find(currentBook.AuthorId);
                    //Check if book is out of order
                }

                var booksInTheCart = db.BooksInBaskets.Where(b => b.UserId == user.Id);
                db.BooksInBaskets.RemoveRange(booksInTheCart);
            
                db.SaveChanges();
            }
            catch(Exception e)
            {
                orderSuccessful = false;
            }


            return View(orderSuccessful);
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
