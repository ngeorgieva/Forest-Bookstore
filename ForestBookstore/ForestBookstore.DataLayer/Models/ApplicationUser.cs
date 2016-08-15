using ForestBookstore.Models.DbContext;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ForestBookstore.Models.DataLayer
{
    using ForestBookstore.DataLayer.Models;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private ICollection<BooksInBasket> booksInBasket;
        private ICollection<OrdersMade> orderMade;
        private ICollection<Review> reviews;

        public ApplicationUser() : base()
        {
            this.booksInBasket = new HashSet<BooksInBasket>();
            this.orderMade = new HashSet<OrdersMade>();
            this.reviews = new HashSet<Review>();
            this.CreatedOn = DateTime.Now;
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

        public virtual ICollection<Review> Reviews
        {
            get { return this.reviews; }
            set { this.reviews = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}