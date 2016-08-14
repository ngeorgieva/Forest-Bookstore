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
    using DataLayer;
    using System.ComponentModel;
    using ForestBookstore.DataLayer.Models;

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
        public virtual DbSet<Review> Reviews { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}