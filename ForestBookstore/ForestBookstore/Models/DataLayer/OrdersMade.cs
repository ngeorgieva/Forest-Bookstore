namespace ForestBookstore.Models.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using ForestBookstore.Models.DbContext;

    [Table("OrdersMade")]
    public partial class OrdersMade
    {
        private ICollection<BooksForOrder> booksForOrder;
          
        public OrdersMade()
        {
            this.booksForOrder = new HashSet<BooksForOrder>();
            this.OrderedOn = DateTime.Now;
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

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string PersonName { get; set; }

        public string Town { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }
    }
}
