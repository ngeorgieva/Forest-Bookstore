namespace ForestBookstore.Models
{
    public class ShipmentViewModel
    {
        public ShipmentViewModel()
        {
            this.Cart = new ShoppingCartBookViewModel();
            this.Details = new ShipmentDetailsViewModel();
        }

        public ShoppingCartBookViewModel Cart { get; set; }

        public ShipmentDetailsViewModel Details { get; set; }
    }
}