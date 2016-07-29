namespace ForestBookstore.Models.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrdersMade")]
    public partial class OrdersMade
    {

        private ICollection<BooksForOrder> booksForOrder;
          
        public OrdersMade()
        {
            this.booksForOrder = new HashSet<BooksForOrder>();
        }

        public int Id { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime OrderedOn { get; set; }

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

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
