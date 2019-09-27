using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace Tech4Gaming_Deals.Models
{
    public class ProductPost
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public float SalePrice { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        //public Byte[] productImage { get; set; }
    }
}
