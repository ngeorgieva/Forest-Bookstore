namespace ForestBookstore.Models.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Book
    {
        private ICollection<BooksForOrder> booksForOrder;
        private ICollection<BooksInBasket> booksInBasket;
        private ICollection<Category> categories;

        public Book()
        {
            this.booksForOrder = new HashSet<BooksForOrder>();
            this.booksInBasket = new HashSet<BooksInBasket>();
            this.categories = new HashSet<Category>();
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Title")]
        public string Name { get; set; }

        [Required]
        public Author Author { get; set; }

        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [DisplayName("In stock")]
        public int CurrentCount { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Added on")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
