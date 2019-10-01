using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Refit;
using Tech4Gaming_Deals.Models;

namespace Tech4Gaming_Deals.Managers
{
    public static class OnlineDataManager
    {
        #region GET

        public static async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var restService = new Tech4GamingApi();

            var products = await restService.GetProductsAsync();
            return products;
        }

        public static async Task<List<Product>> GetProductsByCategoryAsync(ObservableCollection<ProductCategory> categoryList)
        {
            var restService = new Tech4GamingApi();
            List<Product> products = new List<Product>();

            // Get products
            foreach (var category in categoryList)
            {
                Product[] tmpProducts;
                if (category.IsSelected)
                {
                    tmpProducts = await restService.GetProductsByCategoryLimitAsync(category.Name, category.Skip.ToString(), "5");
                    category.Skip += tmpProducts.Length;
                    if(tmpProducts != null)
                        products.AddRange(tmpProducts);
                }

            }

            return products;
        }

        public static async Task<List<Product>> GetProductsByCategoryLocationAsync(ObservableCollection<ProductCategory> categoryList, string currencyString)
        {
            var restService = new Tech4GamingApi();
            List<Product> products = new List<Product>();

            // Get products
            foreach (var category in categoryList)
            {
                Product[] tmpProducts;
                if (category.IsSelected)
                {
                    tmpProducts = await restService.GetProductsByCategoryLocationLimitAsync(category.Name, category.Skip.ToString(), "5", currencyString);
                    category.Skip += tmpProducts.Length;
                    if (tmpProducts != null)
                        products.AddRange(tmpProducts);
                }

            }

            return products;
        }

        public static async Task<List<Product>> FilterProductsByCategoryAsync(ObservableCollection<ProductCategory> categoryList)
        {
            var restService = new Tech4GamingApi();
            List<Product> products = new List<Product>();

            // Get products
            foreach (var category in categoryList)
            {
                Product[] tmpProducts;
                if (category.IsSelected)
                {
                    tmpProducts = await restService.GetProductsByCategoryLimitAsync(category.Name, "0", category.Skip.ToString());
                    category.Skip = tmpProducts.Length;
                    products.AddRange(tmpProducts);
                }

            }

            return products;
        }

        public static async Task<List<Product>> FilterProductsByCategoryLocationAsync(ObservableCollection<ProductCategory> categoryList, string currencyStr)
        {
            var restService = new Tech4GamingApi();
            List<Product> products = new List<Product>();

            // Get products
            foreach (var category in categoryList)
            {
                Product[] tmpProducts;
                if (category.IsSelected)
                {
                    tmpProducts = await restService.GetProductsByCategoryLocationLimitAsync(category.Name, "0", category.Skip.ToString(), currencyStr);
                    category.Skip = tmpProducts.Length;
                    products.AddRange(tmpProducts);
                }

            }

            return products;
        }

        #endregion

        #region POST

        public static async Task<Product> PostProductAsync(ProductPost product, ByteArrayPart bytes)
        {
            var restService = new Tech4GamingApi();
            return await restService.PostProductAsync(product, bytes);
        }

        #endregion
    }
}
