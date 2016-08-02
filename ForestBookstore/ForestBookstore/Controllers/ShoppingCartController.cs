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
            var booksInCart = db.BooksInBaskets.Where(b => b.UserId == currentUser.Id).ToList();

            return View(booksInCart);
        }

        // GET: ShoppingCart/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BooksInBasket booksInBasket = db.BooksInBaskets.Find(id);
            if (booksInBasket == null)
            {
                return HttpNotFound();
            }
            return View(booksInBasket);
        }

        // POST: ShoppingCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BooksInBasket booksInBasket = db.BooksInBaskets.Find(id);
            db.BooksInBaskets.Remove(booksInBasket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
