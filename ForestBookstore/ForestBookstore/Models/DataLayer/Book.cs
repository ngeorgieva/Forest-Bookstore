namespace ForestBookstore.Models.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Book
    {
        private ICollection<BooksForOrder> booksForOrder;
        private ICollection<BooksInBasket> booksInBasket;
        private ICollection<Author> authors;
        private ICollection<Category> categories;

        public Book()
        {
            this.booksForOrder = new HashSet<BooksForOrder>();
            this.booksInBasket = new HashSet<BooksInBasket>();
            this.authors = new HashSet<Author>();
            this.categories = new HashSet<Category>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int CurrentCount { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }
     
        public virtual ICollection<BooksForOrder> BooksForOrder
        {
            get
            {
                return this.booksForOrder;
            }
            set
            {
                this.booksForOrder = value;
            }
        }
     
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
    
        public virtual ICollection<Author> Authors
        {
            get
            {
                return this.authors;
            }
            set
            {
                this.authors = value;
            }
        }
      
        public virtual ICollection<Category> Categories
        {
            get
            {
                return this.categories;
            }
            set
            {
                this.categories = value;
            }
        }
    }
}
