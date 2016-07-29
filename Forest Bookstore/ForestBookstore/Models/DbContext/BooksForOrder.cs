namespace ForestBookstore.Models.DbContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BooksForOrder")]
    public partial class BooksForOrder
    {
        [Key, Column(Order = 0)]
        public int OrderId { get; set; }

        [Key, Column(Order = 1)]
        public int BookId { get; set; }

        public int Count { get; set; }


        public virtual Book Book { get; set; }

        public virtual OrdersMade OrdersMade { get; set; }
    }
}
