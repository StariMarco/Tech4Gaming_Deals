using System;
using System.Collections.Generic;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Linq;
using Tech4Gaming_Deals.Interfaces;
using Tech4Gaming_Deals.Managers;
using System.IO;
using System.Threading.Tasks;
using Refit;
using Rg.Plugins.Popup.Services;

namespace Tech4Gaming_Deals
{
    public partial class NewProductPage : ContentPage
    {
        private ProductPost _newProduct;
        private byte[] _imageByteArray;

        public NewProductPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            _newProduct = new ProductPost();
        }

        private bool IsNewProductReady()
        {
            return !(String.IsNullOrWhiteSpace(txtProductLink.Text) ||
                     String.IsNullOrWhiteSpace(txtProductName.Text) ||
                     String.IsNullOrWhiteSpace(txtProductPrice.Text) ||
                     _newProduct.Category == null ||
                     _newProduct.CurrencySymbol == null ||
                     productImage.Source == null);
        }

        private void CheckProductCompletition()
        {
            if (IsNewProductReady())
            {
                btnAdd.IsEnabled = true;
                btnAdd.TextColor = Color.White;
                btnAdd.BackgroundColor = (Color)Application.Current.Resources["colorPrimary"];
            }
            else
            {
                btnAdd.IsEnabled = false;
                btnAdd.BackgroundColor = (Color)Application.Current.Resources["colorPrimaryLight"];
            }
        }

        private async void OnBtnBack_Pressed(object sender, EventArgs e)
        {
            bool userConfirmation = await DisplayAlert("Alert", "By going back you will lose your work!", "OK", "CANCEL");
            if (!userConfirmation)
                return;

            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void OnCategorySelectionChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;

            _newProduct.Category = picker.Items[picker.SelectedIndex];

            CheckProductCompletition();
        }

        private void OnLocationSelectionChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;

            string symbol;
            string item = picker.Items[picker.SelectedIndex];
            if (String.Equals(item, "Italy", StringComparison.Ordinal))
                symbol = "€";
            else if (String.Equals(item, "America", StringComparison.Ordinal))
                symbol = "$";
            else
                symbol = "£";

            _newProduct.CurrencySymbol = symbol;

            lblPriceTag.Text = $"Price ({symbol})";
            lblSalePriceTag.Text = $"Sale Price ({symbol})";

            CheckProductCompletition();
        }

        private void TextChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CheckProductCompletition();
        }

        #region Submit Product

        private async void OnButtonDoneClicked(object sender, EventArgs e)
        {
            // Check if all entries are filled
            if (!IsNewProductReady())
            {
                await DisplayAlert("Error", "Please fill all entries", "OK");
                return;
            }

            // Product Assembly
            try
            {
                CreateProduct();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred \n {ex.Message}", "OK");
            }

            //User confirmation
            bool userConfirmation = await DisplayAlert("Confirmation", "Do you want to add the product to the database?", "ADD", "CANCEL");

            if (!userConfirmation)
                return;

            Product result = null;

            try
            {
                result = await OnlineDataManager.PostProductAsync(_newProduct, new ByteArrayPart(_imageByteArray, "name"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Posting image:\n" + ex.Message);
            }

            // Check if the upload failed
            if (result == null)
            {
                await DisplayAlert("Error", "Something failed", "OK");
                return;
            }

            await DisplayAlert("Done", "The product has been added successfully", "OK");

            await Navigation.PopAsync();

        }

        private void CreateProduct()
        {
            _newProduct.Url = txtProductLink.Text;
            _newProduct.Name = txtProductName.Text;
            _newProduct.Price = float.Parse(txtProductPrice.Text);
            _newProduct.SalePrice = 0;
            if (!String.IsNullOrEmpty(txtProductSalePrice.Text))
                _newProduct.SalePrice = float.Parse(txtProductSalePrice.Text);
            _newProduct.Description = txtDescription.Text;
        }

        #endregion

        #region Photo Picker

        private async void OnPickPhotoClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            button.IsEnabled = false;

            System.IO.Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                _imageByteArray = GetImageStreamAsBytes(stream);

                productImage.Source = ImageSource.FromStream(() => new MemoryStream(_imageByteArray));
                btnPhotoPicker.Opacity = 0.1f;
            }

            button.IsEnabled = true;
        }

        private byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        #endregion

        #region Product Preview

        private async void GetProductPreview(object sender, EventArgs e)
        {
            CreateProduct();

            await PopupNavigation.Instance.PushAsync((Rg.Plugins.Popup.Pages.PopupPage)new ProductPreviewPopup(_newProduct, _imageByteArray, this), true);
        }

        public void LoadChanges()
        {
            txtProductName.Text = _newProduct.Name;
            txtProductPrice.Text = _newProduct.Price.ToString();
            txtProductSalePrice.Text = _newProduct.SalePrice.ToString();
            txtDescription.Text = _newProduct.Description;
        }

        #endregion
    }
}
