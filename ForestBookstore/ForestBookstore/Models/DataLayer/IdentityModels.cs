using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ForestBookstore.Models.DbContext;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Data.Entity.Validation;

namespace ForestBookstore.Models
{
    using System.ComponentModel;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private ICollection<BooksInBasket> booksInBasket;
        private ICollection<OrdersMade> orderMade;

        public ApplicationUser() : base()
        {
            booksInBasket = new HashSet<BooksInBasket>();
            orderMade = new HashSet<OrdersMade>();
        }

        [DefaultValue("")]
        [StringLength(50)]
        public string PersonName { get; set; }

        [StringLength(50)]
        public string Town { get; set; }

        public string Address { get; set; }

        public int Phone { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<BooksInBasket> BooksInBasket
        {
            get
            {
                return this.booksInBasket;
            }
            set
            {
                this.booksInBasket = value;
            }
        }

        public virtual ICollection<OrdersMade> OrdersMades
        {
            get
            {
                return this.orderMade;
            }
            set
            {
                this.orderMade = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("BookstoreDbContext", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BooksForOrder> BooksForOrders { get; set; }
        public virtual DbSet<BooksInBasket> BooksInBaskets { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<OrdersMade> OrdersMades { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}