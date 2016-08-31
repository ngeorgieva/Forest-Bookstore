namespace ForestBookstore.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ShipmentDetailsViewModel
    {

        public ShipmentDetailsViewModel()
        {
        }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string PersonName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Address { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Town { get; set; }

        [Required]
        public string Phone { get; set; }

    }
}