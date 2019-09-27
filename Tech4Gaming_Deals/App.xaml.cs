﻿using System;
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
        public ProductsPage ProductsPageInstance { get; set; }

        public List<string> ShoppingCartProducts { get; set; }
        public ShoppingCartPage ShoppingcartPageInstance { get; set; }

        public App()
        {
            InitializeComponent();

            this.Products = new ObservableCollection<Product>();

            LoadLocalData();

            //MainPage = new NavigationPage(new ProductsPage());
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

            await FilterProductsAsync();
        }

        #region Tech4Gaming Api

        // Append new products to Products list

        public async Task UpdateProductsAsync()
        {
            List<Product> products = await OnlineDataManager.GetProductsByCategoryAsync(this.CategoryList);

            // Order by Date
            products = products.OrderByDescending(p => p.Date).ToList();

            this.Products = new ObservableCollection<Product>(this.Products.Union(products));

            ProductsPageInstance.UpdateProductListItemSource();
        }

        // Filter out the already existing products inside Products list

        public async Task FilterProductsAsync()
        {
            List<Product> products = await OnlineDataManager.FilterProductsByCategoryAsync(this.CategoryList);

            // Order by Date
            products = products.OrderByDescending(p => p.Date).ToList();

            this.Products = new ObservableCollection<Product>(products);

            ProductsPageInstance.UpdateProductListItemSource();
        }

        #endregion

        #region LocalData Management

        public LocalData GetLocalDataToSave()
        {
            LocalData data = new LocalData()
            {
                Categories = this.CategoryList.ToList(),
                ShoppingCartProducts = this.ShoppingCartProducts
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
                return;
            }

            foreach (var category in data.Categories)
            {
                category.Skip = 0;
            }
            this.CategoryList = new ObservableCollection<ProductCategory>(data.Categories);
            this.ShoppingCartProducts = data.ShoppingCartProducts;
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