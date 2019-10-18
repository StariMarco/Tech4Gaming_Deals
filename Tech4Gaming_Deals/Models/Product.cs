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
        public DateTime ExpireAt { get; set; }

        public string PriceText
        {
            get
            {
                return string.Format("{0:0.00}{1}", Price, currencySymbol);
            }
        }
        public string SalePriceText
        {
            get
            {
                return (SalePrice <= 0) ? "" : string.Format("{0:0.00}{1}", SalePrice, currencySymbol);
            }
        }

        public string ExpireTime
        {
            get
            {
                var time = (ExpireAt.ToLocalTime().Subtract(DateTime.Now.ToUniversalTime().ToLocalTime()));
                //var time = (ExpireAt.Subtract(DateTime.Now));
                if (time.Hours == 0 && time.Minutes == 0)
                    return String.Format("Expires in {0}m", time.Minutes);
                if (time.Days == 0 && time.Hours != 0)
                    return String.Format("Expires in {0}h {1}m", time.Hours, time.Minutes);
                else
                    return String.Format("Expires in {0}d {1}h", time.Days, time.Minutes);

            }
        }
    }
}
