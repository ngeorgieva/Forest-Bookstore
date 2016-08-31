namespace ForestBookstore.Models.DbContext
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using DataLayer;

    [Table("BooksInBasket")]
    public partial class BooksInBasket
    {
        [Key]
        public int Id { get; set; }

        public int Count { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
