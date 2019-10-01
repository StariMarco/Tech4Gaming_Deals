using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tech4Gaming_Deals.Models;
using Refit;
using System.IO;

namespace Tech4Gaming_Deals.Interfaces
{
    public interface ITech4GamingApi
    {
        // GET

        [Get("/api/products")]
        Task<Product[]> GetProducts();

        [Get("/api/products/{id}")]
        Task<Product> GetProductById(string id);

        [Get("/api/products/category/{name}")]
        Task<Product[]> GetProductsByCategory(string name);

        [Get("/api/products/category/{name}/{skip}/{limit}")]
        Task<Product[]> GetProductsByCategoryLimit(string name, string skip, string limit);

        [Get("/api/products/category/{name}/{skip}/{limit}/{currency}")]
        Task<Product[]> GetProductsByCategoryLocationLimit(string name, string skip, string limit, string currency);

        // POST
        [Multipart]
        [Post("/api/products")]
        Task<Product> AddProduct([AliasAs("name")]string name, [AliasAs("price")]float price, [AliasAs("salePrice")]float salePrice, [AliasAs("url")]string url, [AliasAs("category")]string category, [AliasAs("description")]string descxription, [AliasAs("currencySymbol")]string location, [AliasAs("productImage")]ByteArrayPart bytes);
        //Task<Product> AddProduct(ProductPost product, [AliasAs("productImage")]IEnumerable<StreamPart> streams);

        // DELETE

        [Delete("/api/products/{id}")]
        Task DeleteProductById(string id);
    }
}
