using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tech4Gaming_Deals.Managers;

namespace Tech4Gaming_Deals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public ObservableCollection<Product> ProductList;
        private App _app;
        private int normalFontSize, smallFontSize;

        public ProductsPage()
        {
            InitializeComponent();

            _app = Application.Current as App;
            _app.ProductPage = (this);

            if (_app.SelectedLocations.Count == 0)
                Navigation.PushModalAsync(new SelectRegionPage());

            if (IsInternetMissing())
            {
                //  Not Internet connection
                lblOffline.IsVisible = true;
                activityIndicator.IsVisible = false;
                btnViewMore.IsVisible = false;
                return;
            }

            InitializeProducts();

            AdaptContentToScreenSize();
        }

        private static bool IsInternetMissing()
        {
            return Connectivity.NetworkAccess == NetworkAccess.None || Connectivity.NetworkAccess == NetworkAccess.Local;
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
            // The classId is used to store the product url
            var url = (sender as Image).ClassId;
            Device.OpenUri(new Uri(url));
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

            if (cell == null || product == null)
                return;

            var lblPrice = cell.FindByName("lblProductPrice") as Label;
            var lblSalePrice = cell.FindByName("lblProductSalePrice") as Label;
            UpdateCell(product, lblPrice, lblSalePrice);

            lblPrice.TextDecorations = TextDecorations.None;

            if (lblSalePrice.IsVisible && Device.RuntimePlatform == Device.Android)
            {
                lblSalePrice.IsVisible = true;
                lblPrice.TextDecorations = TextDecorations.Strikethrough;
            }
        }

        private void UpdateCell(Product product, Label lblPrice, Label lblSalePrice)
        {
            if (product.SalePrice > 0)
            {
                // sale price
                lblSalePrice.IsVisible = true;
                lblSalePrice.TextColor = (Color)Application.Current.Resources["colorRed"];
                lblSalePrice.FontSize = normalFontSize;

                lblPrice.TextColor = (Color)Application.Current.Resources["colorSecondaryText"];
                lblPrice.FontSize = smallFontSize;

                if (String.IsNullOrWhiteSpace(lblSalePrice.Text))
                    lblSalePrice.Text = product.SalePriceText;
            }
            else
            {
                // no sale price
                lblSalePrice.IsVisible = false;
                lblPrice.TextColor = (Color)Application.Current.Resources["colorRed"];
                lblPrice.FontSize = normalFontSize;
            }
        }

        #endregion

        #region Platform Adaptation

        private void AdaptContentToScreenSize()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            var displayWidth = mainDisplayInfo.Width;

            if (Device.RuntimePlatform == Device.Android)
            {
                this.normalFontSize = 12;
                this.smallFontSize = 9;

                AdaptAndroidImageSize(displayWidth);
            }
            else
            {
                this.normalFontSize = 14;
                this.smallFontSize = 11;
            }
            Application.Current.Resources["smallFontSize"] = (double)smallFontSize;
            Application.Current.Resources["normalFontSize"] = (double)normalFontSize;
            
            AdaptFontSize(displayWidth);
        }

        private void AdaptFontSize(double displayWidth)
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
                            Value = this.normalFontSize
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
                            Value = this.smallFontSize
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

        private async void OnRefreshProducts(object sender, EventArgs e)
        {
            if (IsInternetMissing())
            {
                lstProducts.EndRefresh();
                lblOffline.IsVisible = true;
                btnViewMore.IsVisible = false;
                return;
            }
            lblOffline.IsVisible = false;
            btnViewMore.IsVisible = true;

            await _app.FilterProductsAsync(refreshProductsList: false);
            UpdateProductListItemSource();
            lstProducts.EndRefresh();
        }

        private async void InitializeProducts()
        {
            await GetProductsAsync();
            activityIndicator.IsVisible = false;
            btnViewMore.IsVisible = true;
        }

        private async Task GetProductsAsync()
        {
            await _app.UpdateProductsAsync();

            UpdateProductListItemSource();
        }

        public void UpdateProductListItemSource()
        {
            lstProducts.ItemsSource = _app.Products;

            if (_app.Products.Count == 0)
                lblProductNotFound.IsVisible = true;
            else
                lblProductNotFound.IsVisible = false;
        }

        private void OnViewMoreProductsClicked(object sender, EventArgs e)
        {
            if (IsInternetMissing())
                return;

            btnViewMore.IsVisible = false;
            activityIndicator.IsVisible = true;
            InitializeProducts();
        }

        private async void OnSearchByPartialName(object sender, EventArgs e)
        {
            await _app.FilterProductsByPartialNameAsync(entrySearchProduct.Text);

            UpdateProductListItemSource();
        }

        #endregion
    }
}
