namespace ForestBookstore.Models.DbContext
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class BookstoreDbContext : DbContext
    {
        // Your context has been configured to use a 'BookstoreDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ForestBookstore.Models.DbContext.BookstoreDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'BookstoreDbContext' 
        // connection string in the application configuration file.
        public BookstoreDbContext()
            : base("BookstoreDbContext")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BooksForOrder> BooksForOrders { get; set; }
        public virtual DbSet<BooksInBasket> BooksInBaskets { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<OrdersMade> OrdersMades { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}