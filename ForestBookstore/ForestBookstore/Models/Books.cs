using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForestBookstore.Models.DbContext;

namespace ForestBookstore.Models
{
    public class Books
    {
        public ICollection<Book> GetNewest(int count = 6)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var newestBooks = db.Books.OrderBy(book => book.CreatedOn).Take(count).ToList();

            return newestBooks;
        }
        
    }
}