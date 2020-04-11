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
        public string CurrencySymbol { get; set; }
        public DateTime ExpireAt { get; set; }

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
