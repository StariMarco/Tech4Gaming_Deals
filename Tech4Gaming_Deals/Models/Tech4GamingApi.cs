﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Refit;
using Tech4Gaming_Deals.Interfaces;
using Xamarin.Forms;

namespace Tech4Gaming_Deals.Models
{
    public class Tech4GamingApi
    {
        public readonly ITech4GamingApi _restClient;

        public Tech4GamingApi()
        {
            _restClient = RestService.For<ITech4GamingApi>("https://tech4gaming-deals.herokuapp.com");
        }

        // GET

        public async Task<Product[]> GetProductsAsync()
        {
            return await _restClient.GetProducts();
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            return await _restClient.GetProductById(productId);
        }

        public async Task<Product[]> GetProductsByCategoryAsync(string productCategory)
        {
            return await _restClient.GetProductsByCategory(productCategory);
        }

        public async Task<Product[]> GetProductsByPartialNameAsync(string productCategory, string partialName)
        {
            return await _restClient.GetProductsByPartialName(productCategory, partialName);
        }

        public async Task<Product[]> GetProductsByCategoryLimitAsync(string productCategory, string skip, string limit)
        {
            return await _restClient.GetProductsByCategoryLimit(productCategory, skip, limit);
        }

        public async Task<Product[]> GetProductsByCategoryLocationLimitAsync(string productCategory, string skip, string limit, string currency)
        {
            return await _restClient.GetProductsByCategoryLocationLimit(productCategory, skip, limit, currency);
        }

        // POST

        public async Task<Product> PostProductAsync(ProductPost product, ByteArrayPart bytes)
        {
            string date = product.ExpireAt.ToString("dd MMM yyyy hh:mm tt", CultureInfo.InvariantCulture);
            return await _restClient.AddProduct(product.Name, product.Price, product.SalePrice, product.Url, product.Category, product.Description, product.CurrencySymbol, date, bytes);
        }

        // DELETE

        public async Task<Product> DeleteProductByIdAsync(string productId)
        {
            return await _restClient.DeleteProductById(productId);
        }
    }
}
