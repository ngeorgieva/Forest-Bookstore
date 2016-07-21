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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdersMade()
        {
            BooksForOrders = new HashSet<BooksForOrder>();
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime OrderedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BooksForOrder> BooksForOrders { get; set; }

        public virtual User User { get; set; }
    }
}
