namespace ForestBookstore.DataLayer.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using ForestBookstore.Models.DataLayer;
    using ForestBookstore.Models.DbContext;

    public class Review
    {
        public Review()
        {
            this.Date = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [DisplayName("Review Text")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string AuthorId { get; set; }
        
        public virtual ApplicationUser Author { get; set; }

        public int BookId { get; set; }

        //[Required]
        public virtual Book Book { get; set; }
    }
}
