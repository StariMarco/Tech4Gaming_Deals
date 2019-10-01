using System;
using System.Collections.Generic;

namespace Tech4Gaming_Deals.Models
{
    public class LocalData
    {
        public List<string> ShoppingCartProducts { get; set; }
        public List<ProductCategory> Categories { get; set; }
        public bool Notifications { get; set; }
        public List<Location> SelectedLocations { get; set; }
    }
}
