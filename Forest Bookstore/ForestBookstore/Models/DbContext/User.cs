namespace ForestBookstore.Models.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {

        private ICollection<BooksInBasket> booksInBasket;
        private ICollection<OrdersMade> orderMade;

        public User()
        {
            booksInBasket = new HashSet<BooksInBasket>();
            orderMade = new HashSet<OrdersMade>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string PersonName { get; set; }

        [Required]
        [StringLength(50)]
        public string Town { get; set; }

        [Required]
        public string Address { get; set; }

        public int Phone { get; set; }

        [Required]
        [StringLength(65)]
        public string Email { get; set; }

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
    }
}
