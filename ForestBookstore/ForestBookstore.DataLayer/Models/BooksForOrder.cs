namespace ForestBookstore.Models.DbContext
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BooksForOrder")]
    public partial class BooksForOrder
    {
        [Key]
        public int OrderId { get; set; }
              
        public int Count { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        [ForeignKey("OrdersMade")]
        public int OrdersMadeId { get; set; }

        public virtual OrdersMade OrdersMade { get; set; }
    }
}
