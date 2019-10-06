using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Refit;
using Tech4Gaming_Deals.Interfaces;
using Tech4Gaming_Deals.Managers;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tech4Gaming_Deals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public ObservableCollection<ProductCategory> CategoryList { get; set; }

        public ObservableCollection<Product> Products { get; set; }

        public List<string> ShoppingCartProducts { get; set; }
        public ShoppingCartPage ShoppingcartPageInstance { get; set; }

        public ProductsPage ProductPage { get; set; }

        public List<Location> SelectedLocations { get; set; }

        public bool Notifications { get; set; }

        public App()
        {
            InitializeComponent();

            this.Products = new ObservableCollection<Product>();
            this.SelectedLocations = new List<Location>();


            LoadLocalData();

            //MainPage = new NavigationPage(new SelectRegionPage());
            MainPage = new MasterPage();
        }

        private void InitializeCategoryList()
        {
            CategoryList = new ObservableCollection<ProductCategory>()
            {
                new ProductCategory(){Name = "Games", IsSelected = true, Image = "ic_label.png", Skip = 0},
                new ProductCategory(){Name = "Hardware", IsSelected = true, Image = "ic_label.png", Skip = 0},
                new ProductCategory(){Name = "Accessories", IsSelected = true, Image = "ic_label.png", Skip = 0}
            };
        }

        public async Task UpdateCategoryList(ProductCategory category, int index)
        {
            CategoryList.RemoveAt(index);
            CategoryList.Insert(index, category);

            await FilterProductsAsync(refreshProductsList: true);
        }

        #region Tech4Gaming Api

        // Append new products to Products list

        public async Task UpdateProductsAsync()
        {
            List<Product> products = await OnlineDataManager.GetProductsByCategoryLocationAsync(this.CategoryList, GetCurrencyString());

            // Order by Date
            products = this.Products.ToList().Union(products).ToList();

            this.Products = new ObservableCollection<Product>(products.OrderByDescending(p => p.Date));
        }

        private string GetCurrencyString()
        {
            string result = "";
            result += (SelectedLocations.Find(l => String.Equals(l.Name, "England", StringComparison.Ordinal)) != null) ? "£" : "n";
            result += (SelectedLocations.Find(l => String.Equals(l.Name, "America", StringComparison.Ordinal)) != null) ? "$" : "n";
            result += (SelectedLocations.Find(l => String.Equals(l.Name, "Italy", StringComparison.Ordinal)) != null) ? "e" : "n";
            return result;
        }

        // Filter out the already existing products inside Products list

        public async Task FilterProductsAsync(bool refreshProductsList)
        {
            List<Product> products = await OnlineDataManager.FilterProductsByCategoryLocationAsync(this.CategoryList, GetCurrencyString());

            // Order by Date
            products = products.OrderByDescending(p => p.Date).ToList();

            this.Products = new ObservableCollection<Product>(products);

            if (refreshProductsList)
                ProductPage.UpdateProductListItemSource();
        }

        // Search by name

        public async Task FilterProductsByPartialNameAsync(string name)
        {
            List<Product> products = await OnlineDataManager.FilterProductsByPartialNameAsync(this.CategoryList, name);

            // Order by Date
            products = products.OrderByDescending(p => p.Date).ToList();

            this.Products = new ObservableCollection<Product>(products);
        }

        #endregion

        #region LocalData Management

        public LocalData GetLocalDataToSave()
        {
            LocalData data = new LocalData()
            {
                Categories = this.CategoryList.ToList(),
                ShoppingCartProducts = this.ShoppingCartProducts,
                Notifications = this.Notifications,
                SelectedLocations = this.SelectedLocations
            };
            return data;
        }

        private void LoadLocalData()
        {
            var data = LocalDataManager.Load();
            if (data == null)
            {
                InitializeCategoryList();
                this.ShoppingCartProducts = new List<string>();
                this.Notifications = true;
                this.SelectedLocations = new List<Location>();
                return;
            }

            foreach (var category in data.Categories)
            {
                category.Skip = 0;
            }
            this.CategoryList = new ObservableCollection<ProductCategory>(data.Categories);
            this.ShoppingCartProducts = data.ShoppingCartProducts;
            this.Notifications = data.Notifications;
            if(data.SelectedLocations != null)
                this.SelectedLocations = data.SelectedLocations;
        }

        public void RemoveDeprecatedProductsFromShoppingList(List<string> products)
        {
            foreach (var product in products)
            {
                this.ShoppingCartProducts.Remove(product);
            }
        }

        #endregion
    }
}
