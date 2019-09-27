using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Tech4Gaming_Deals.Managers;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tech4Gaming_Deals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartPage : ContentPage
    {
        public ObservableCollection<Product> ShoppingCartProductList;
        private App _app;

        public ShoppingCartPage()
        {
            InitializeComponent();

            _app = Application.Current as App;
            _app.ShoppingcartPageInstance = (this);

            InitializeProducts();
        }

        private void DisableSelection(object sender, EventArgs e)
        {
            lstCartProducts.SelectedItem = null;
        }

        private void UpdateShoppingListItemSource()
        {
            lstCartProducts.ItemsSource = this.ShoppingCartProductList;
        }

        #region Popup

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

            if (product.SalePrice <= 0)
            {
                var lblPrice = cell.FindByName("lblProductPrice") as Label;
                var lblSalePrice = cell.FindByName("lblProductSalePrice") as Label;

                lblSalePrice.IsVisible = false;
                lblPrice.FontSize = lblSalePrice.FontSize;
                lblPrice.TextDecorations = TextDecorations.None;
                lblPrice.TextColor = (Color)Application.Current.Resources["colorRed"];
            }
        }

        #endregion

        #region Tech4Gaming Api

        private void OnRefreshProducts(object sender, EventArgs e)
        {
            InitializeProducts();
            lstCartProducts.EndRefresh();
        }

        private async void InitializeProducts()
        {
            await GetProductsAsync();
        }

        private async Task GetProductsAsync()
        {
            ShoppingCartProductList = new ObservableCollection<Product>();

            var restService = new Tech4GamingApi();
            List<string> toRemove = new List<string>();

            foreach (var productId in _app.ShoppingCartProducts)
            {
                Product result = await GetProductByIdAsync(restService, productId);
                Console.WriteLine(result);
                if (result == null)
                {
                    Console.WriteLine("Cannot find this product");
                    toRemove.Add(productId);
                    continue;
                }

                this.ShoppingCartProductList.Add(result);
            }

            _app.RemoveDeprecatedProductsFromShoppingList(toRemove);
            UpdateShoppingListItemSource();
            LocalDataManager.Save(_app.GetLocalDataToSave());
        }

        private async Task<Product> GetProductByIdAsync(Tech4GamingApi restService, string productId)
        {
            Product result = null;
            try
            {
                result = await restService.GetProductByIdAsync(productId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred\n" + ex.Message);
                return null;
            }

            return result;
        }

        #endregion
    }
}
