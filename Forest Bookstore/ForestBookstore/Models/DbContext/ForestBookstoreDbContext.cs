namespace ForestBookstore.Models.DbContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ForestBookstoreDbContext : DbContext
    {
        public ForestBookstoreDbContext()
            : base("name=ForestBookstoreDbContext")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<TestBooks> TestBooks { get; set; }
        public virtual DbSet<BooksForOrder> BooksForOrders { get; set; }
        public virtual DbSet<BooksInBasket> BooksInBaskets { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<OrdersMade> OrdersMades { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(e => e.Books)
                .WithMany(e => e.Authors)
                .Map(m => m.ToTable("Books_Authors").MapLeftKey("AuthorId").MapRightKey("BookId"));

            modelBuilder.Entity<Book>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BooksForOrders)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BooksInBaskets)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Categories)
                .WithMany(e => e.Books)
                .Map(m => m.ToTable("Books_Categories").MapLeftKey("BookId").MapRightKey("CategoryId"));

            modelBuilder.Entity<OrdersMade>()
                .HasMany(e => e.BooksForOrders)
                .WithRequired(e => e.OrdersMade)
                .HasForeignKey(e => e.OrderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.BooksInBaskets)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OrdersMades)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
