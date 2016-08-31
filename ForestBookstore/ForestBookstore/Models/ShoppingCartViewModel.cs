namespace ForestBookstore.Models
{
    using System.Collections.Generic;
    using DbContext;

    public class ShoppingCartBookViewModel
    {
        public ShoppingCartBookViewModel(ICollection<CartLine> collection)
        {
            this.Books = collection;
        }

        public ShoppingCartBookViewModel() : this(new List<CartLine>())
        {
        }

        public ICollection<CartLine> Books { get; set; }
    }

    public class CartLine
    {
        public string UserId { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public int Count { get; set; }
    }
}