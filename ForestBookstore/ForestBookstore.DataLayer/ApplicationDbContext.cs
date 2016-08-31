namespace ForestBookstore.Models
{
    using System.Data.Entity;
    using DataLayer;
    using DbContext;
    using ForestBookstore.DataLayer.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

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