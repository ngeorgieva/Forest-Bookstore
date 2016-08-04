using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForestBookstore.Models
{
    public class ShipmentDetailsViewModel
    {

        public ShipmentDetailsViewModel()
        {
        }

        public string PersonName { get; set; }

        public string Address { get; set; }

        public string Town { get; set; }

        public int Phone { get; set; }

    }
}