using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Tech4Gaming_Deals.Managers;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;

namespace Tech4Gaming_Deals
{
    public partial class ProductPopupPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public Product SelectedProduct { get; set; }
        private App _app;

        public ProductPopupPage(Product product, App app)
        {
            InitializeComponent();

            this.SelectedProduct = product;
            this._app = app;

            InitShoppingCartButtonStyle();

            InitialisePopupComponents();
        }

        private void InitialisePopupComponents()
        {
            lblName.Text = SelectedProduct.Name;
            imgProduct.Source = SelectedProduct.productImage;

            // Price
            lblPrice.Text = $"{SelectedProduct.Price.ToString()}{SelectedProduct.currencySymbol}";
            if(SelectedProduct.SalePrice > 0)
            {
                lblSalePrice.Text = $"{SelectedProduct.SalePrice.ToString()}{SelectedProduct.currencySymbol}";
                lblSalePrice.IsVisible = true;
                lblPrice.TextColor = (Color)Application.Current.Resources["colorSecondaryText"];
                lblPrice.TextDecorations = TextDecorations.Strikethrough;
                lblPrice.FontSize = lblSalePrice.FontSize - 2;
            }

            // Description
            lblDescription.Text = SelectedProduct?.Description;
            if (String.IsNullOrWhiteSpace(SelectedProduct.Description))
                lblDescText.IsVisible = false;
        }

        public ProductPopupPage()
        {
            InitializeComponent();
        }


        private void OnOpenProductLink(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(SelectedProduct.Url));
        }

        private void OnAddToShoppingCart(object sender, EventArgs e)
        {
            if (IsProductInShoppingCart())
            {
                btnAddToCart.BackgroundColor = Color.White;
                btnAddToCart.TextColor = (Color)Application.Current.Resources["colorPrimary"];
                _app.ShoppingCartProducts.Remove(SelectedProduct?._id);
            }
            else
            {
                btnAddToCart.BackgroundColor = (Color)Application.Current.Resources["colorPrimary"];
                btnAddToCart.TextColor = Color.White;
                _app.ShoppingCartProducts.Add(SelectedProduct?._id);
            }

            SaveShoppingCart();
        }

        private void InitShoppingCartButtonStyle()
        {
            if (!IsProductInShoppingCart())
            {
                btnAddToCart.BackgroundColor = Color.White;
                btnAddToCart.TextColor = (Color)Application.Current.Resources["colorPrimary"];
            }
            else
            {
                btnAddToCart.BackgroundColor = (Color)Application.Current.Resources["colorPrimary"];
                btnAddToCart.TextColor = Color.White;
            }
        }

        private bool IsProductInShoppingCart()
        {
            return (_app.ShoppingCartProducts.Find(p => String.Equals(p, SelectedProduct._id, StringComparison.Ordinal)) != null);
        }

        private void SaveShoppingCart()
        {
            var data = _app.GetLocalDataToSave();
            LocalDataManager.Save(data);
        }
    }
}
