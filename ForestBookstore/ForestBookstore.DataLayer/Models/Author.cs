namespace ForestBookstore.Models.DbContext
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Author
    {
        private ICollection<Book> books;
       
        public Author()
        {
            this.books = new HashSet<Book>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Book> Books
        {
            get
            {
                return this.books;
            }
            set
            {
                this.books = value;
            }
        }
    }
}
