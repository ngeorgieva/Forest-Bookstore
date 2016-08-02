using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForestBookstore.Models.DbContext;

namespace ForestBookstore.Models
{
    public class ShoppingCartBookViewModel
    {
        public ShoppingCartBookViewModel(ICollection<CartLine> collection)
        {
            this.Books = collection;
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