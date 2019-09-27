﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;

namespace Tech4Gaming_Deals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public ObservableCollection<Product> ProductList;
        private App _app;

        public ProductsPage()
        {
            InitializeComponent();

            _app = Application.Current as App;
            _app.ProductsPageInstance = (this);

            InitializeProducts();

            AdaptContentToScreenSize();

            //ITemplatedItemsView<Cell> productItemsView = lstProducts as ITemplatedItemsView<Cell>;

            //foreach (var viewCell in productItemsView.TemplatedItems)
            //{
            //    Console.WriteLine(viewCell);
            //}
            //ViewCell firstCell = productItemsView.TemplatedItems[0] as ViewCell;
        }

        private void ActivateSearch(object sender, EventArgs e)
        {
            entrySearchProduct.IsVisible = !entrySearchProduct.IsVisible;
        }

        private async void CreateNewProduct(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewProductPage());
        }

        private void DisableSelection(object sender, EventArgs e)
        {
            lstProducts.SelectedItem = null;
        }

        private void OpenUrl(object sender, EventArgs e)
        {
            var product = (sender as ImageButton).CommandParameter as Product;
            Device.OpenUri(new Uri(product.Url));
        }

        #region Product Popup

        private void OnShowPopup(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as Product;

            PopupNavigation.Instance.PushAsync((Rg.Plugins.Popup.Pages.PopupPage)new ProductPopupPage(product, _app), true);
            
        }

        #endregion

        #region DataTemplate Adaptation

        private void OnBindingContexChanged(object sender, EventArgs e)
        {
            var cell = sender as ViewCell;
            var product = cell.BindingContext as Product;

            if(product.SalePrice <= 0)
            {
                var lblPrice = cell.FindByName("lblProductPrice") as Label;
                var lblSalePrice = cell.FindByName("lblProductSalePrice") as Label;

                lblSalePrice.IsVisible = false;
                lblPrice.FontSize = lblSalePrice.FontSize;
                lblPrice.TextDecorations = TextDecorations.None;
                lblPrice.TextColor = (Color)Application.Current.Resources["colorRed"];
            }
            // TODO: Do the reverse
        }

        #endregion

        #region Platform Adaptation

        private void AdaptContentToScreenSize()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            var displayWidth = mainDisplayInfo.Width;
            int normalFontSize, smallFontSize;

            if (Device.RuntimePlatform == Device.Android)
            {
                normalFontSize = 12;
                smallFontSize = 8;

                AdaptAndroidImageSize(displayWidth);
            }
            else
            {
                normalFontSize = 14;
                smallFontSize = 10;
            }

            AdaptFontSize(displayWidth, normalFontSize, smallFontSize);
        }

        private void AdaptFontSize(double displayWidth, int normalFontSize, int smallFontSize)
        {
            if (displayWidth < 1000)
            {
                //Console.WriteLine($"Width: {displayWidth}");

                var normalStyle = new Style(typeof(Label))
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = normalFontSize
                        }
                    }
                };

                var smallStyle = new Style(typeof(Label))
                {
                    Setters =
                    {
                        new Setter
                        {
                            Property = Label.FontSizeProperty,
                            Value = smallFontSize
                        }
                    }
                };

                Application.Current.Resources["fntszNormal"] = normalStyle;
                Application.Current.Resources["fntszSmall"] = smallStyle;
            }
        }

        private void AdaptAndroidImageSize(double displayWidth)
        {
            var imageProductStyle = new Style(typeof(Image))
            {
                Setters = {
                            new Setter
                            {
                                Property = HeightRequestProperty,
                                Value = displayWidth / 12
                            }
                        }
            };
            Application.Current.Resources["imageProduct"] = imageProductStyle;
        }

        #endregion

        #region Tech4Gaming Api

        private void OnRefreshProducts(object sender, EventArgs e)
        {
            InitializeProducts();
            lstProducts.EndRefresh();
        }

        private async void InitializeProducts()
        {
            await GetProductsAsync();

        }

        private async Task GetProductsAsync()
        {
            await _app.UpdateProductsAsync();

            UpdateProductListItemSource();
        }

        public void UpdateProductListItemSource()
        {
            lstProducts.ItemsSource = _app.Products;
        }

        private void OnViewMoreProductsClicked(object sender, EventArgs e)
        {
            InitializeProducts();
        }

        #endregion
    }
}