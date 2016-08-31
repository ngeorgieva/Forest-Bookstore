namespace ForestBookstore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Models;
    using Models.DbContext;

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
                return this._userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); ;
            }
            private set
            {
                this._userManager = value;
            }
        }

        // GET ShoppingCart
        [Authorize]
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var currentUser = this.UserManager.FindById(this.User.Identity.GetUserId());
            ShoppingCartBookViewModel booksInCart = new ShoppingCartBookViewModel(new List<CartLine>());
            var cartCount = db.BooksInBaskets.Count();

            if (cartCount > 0)
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

            return this.View(booksInCart);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddToCart(int bookId)
        {
            //if (!Request.IsAjaxRequest())
            //{
            //    return HttpNotFound();
            //}

            ApplicationDbContext db = new ApplicationDbContext();
            var currentUser = this.UserManager.FindById(this.User.Identity.GetUserId());
            string bookName = "";

            try
            {
                bookName = db.Books.Where(book => book.Id == bookId).Single().Name;
            }
            catch (Exception)
            {
                bookName = "Book does not exist";
            }

            var lineExist = db.BooksInBaskets
                    .Where(b => b.UserId == currentUser.Id && b.BookId == bookId).Count();
            if (lineExist == 1)
            {
                var cartLine = db.BooksInBaskets
                    .Where(b => b.UserId == currentUser.Id && b.BookId == bookId).Single();
                cartLine.Count++;
            }
            else
            {
                var book = db.Books.Find(bookId);
                var userId = currentUser.Id;
                var cartLine = new BooksInBasket
                {
                    Book = book,
                    BookId = bookId,
                    UserId = userId,
                    Count = 1          
                };

                db.BooksInBaskets.Add(cartLine);
            }
                          
            db.SaveChanges();

            return this.Content(bookName);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            var user = this.UserManager.FindById(this.User.Identity.GetUserId());

            ShipmentDetailsViewModel currentShipment = new ShipmentDetailsViewModel
            {
                PersonName = user.PersonName,
                Address = user.Address,
                Town = user.Town,
                Phone = user.Phone.ToString()
            };

            if(this.db.BooksInBaskets.Where(b => b.UserId == user.Id).Count() == 0)
            {
                return this.RedirectToAction("Index");
            }

            this.Session["PlacingOrder"] = true;

            return this.View(currentShipment);
        }

        [Authorize]
        public ActionResult Summary(ShipmentDetailsViewModel shipmentDetails)
        {
            if (shipmentDetails == null || this.Session["PlacingOrder"] == null)
            {
                return this.RedirectToAction("Index");
            }

            ApplicationDbContext db = new ApplicationDbContext();
            var currentUser = this.UserManager.FindById(this.User.Identity.GetUserId());
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

            foreach (var cartLine in booksInCart.Books)
            {
                cartLine.Book.Author = db.Authors.Find(cartLine.Book.AuthorId);
            }

            this.Session["PersonName"] = shipmentDetails.PersonName;
            this.Session["Address"] = shipmentDetails.Address;
            this.Session["Town"] = shipmentDetails.Town;
            this.Session["Phone"] = shipmentDetails.Phone;

            ShipmentViewModel shipment = new ShipmentViewModel
            {
                Cart = booksInCart,
                Details = shipmentDetails
            };

            return this.View(shipment);
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
                if (this.Session["PlacingOrder"] == null)
                {
                    return this.RedirectToAction("Index");
                }
                else
                {
                    return this.RedirectToAction("Checkout");
                }
            }

            bool orderSuccessful = true;

            try
            {
                var user = this.UserManager.FindById(this.User.Identity.GetUserId());
                ApplicationDbContext db = new ApplicationDbContext();
                OrdersMade order = new OrdersMade
                {
                    Status = false,
                    UserId = this.User.Identity.GetUserId(),
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
                }

                var booksInTheCart = db.BooksInBaskets.Where(b => b.UserId == user.Id);
                db.BooksInBaskets.RemoveRange(booksInTheCart);

                if(this.Session["PlacingOrder"] == null)
                {
                    return this.RedirectToAction("Index");
                }

                db.SaveChanges();

                this.Session["PlacingOrder"] = null;
            }
            catch (Exception e)
            {
                orderSuccessful = false;
            }
            
            return this.View(orderSuccessful);
        }

        // POST: ShoppingCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            this.Session["PlacingOrder"] = null;

            var user = this.UserManager.FindById(this.User.Identity.GetUserId());
            BooksInBasket booksInBasket = this.db.BooksInBaskets
                .Where(bi => bi.BookId == id && bi.User.Id == user.Id).Single();
            this.db.BooksInBaskets.Remove(booksInBasket);
            this.db.SaveChanges();
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpBookCount(int bookId)
        {
            var user = this.UserManager.FindById(this.User.Identity.GetUserId());

            var bookCountInBasket = this.db.BooksInBaskets.Where(b => b.UserId == user.Id && b.BookId == bookId).Single();
            bookCountInBasket.Count += 1;

            this.db.SaveChanges();

            return this.Content(bookCountInBasket.Count.ToString());
        }

        [HttpPost]
        public ActionResult DownBookCount(int bookId)
        {
            var user = this.UserManager.FindById(this.User.Identity.GetUserId());

            var bookCountInBasket = this.db.BooksInBaskets.Where(b => b.UserId == user.Id && b.BookId == bookId).Single();
            if(bookCountInBasket.Count > 1)
            {
                bookCountInBasket.Count -= 1;
            }

            this.db.SaveChanges();

            return this.Content(bookCountInBasket.Count.ToString());
        }

    }
}
