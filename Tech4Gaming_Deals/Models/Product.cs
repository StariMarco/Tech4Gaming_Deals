using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Xamarin.Forms;

namespace Tech4Gaming_Deals.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float SalePrice { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
        public string productImage { get; set; }
        public string Description { get; set; }
        public string currencySymbol { get; set; }
    }
}
