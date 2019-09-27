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

namespace Tech4Gaming_Deals
{
    public partial class NewProductPage : ContentPage
    {
        private ProductPost _newProduct;
        private byte[] _androidByteArray;
        private MemoryStream _stream;

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

            // User confirmation
            bool userConfirmation = await DisplayAlert("Confirmation", GetUserConfirmationString(), "ADD", "CANCEL");

            if (!userConfirmation)
                return;

            Product result = null;

            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                    result = await OnlineDataManager.PostProductAsync(_newProduct, new ByteArrayPart(_stream.ToArray(), "name"));
                else
                    result = await OnlineDataManager.PostProductAsync(_newProduct, new ByteArrayPart(_androidByteArray, "name"));
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

            string productStr = "The product has been added successfully\n";
            productStr += GetProductOutString();
            await DisplayAlert("Done", productStr, "OK");

            await Navigation.PopAsync();

        }

        private void CreateProduct()
        {
            _newProduct.Url = txtProductLink.Text;
            _newProduct.Name = txtProductName.Text;
            _newProduct.Price = float.Parse(txtProductPrice.Text);
            _newProduct.SalePrice = float.Parse(txtProductSalePrice?.Text);
            _newProduct.Description = txtDescription.Text;
        }


        private string GetProductOutString()
        {
            string tmp = "";
            tmp += $"Name: {_newProduct.Name} \n";
            tmp += $"Url: {_newProduct.Url} \n";
            tmp += $"Price: {_newProduct.Price} \n";
            tmp += $"SalePrice: {_newProduct.SalePrice} \n";
            tmp += $"Category: {_newProduct.Category}";
            tmp += $"Description: {_newProduct.Description}";
            return tmp;
        }

        private string GetUserConfirmationString()
        {
            string confirmationString = "Do you want to add the product to the database? \n\n";
            confirmationString += GetProductOutString();
            return confirmationString;
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
                if(Device.RuntimePlatform == Device.iOS)
                {
                    // Save the stream
                    _stream = new MemoryStream();
                    await stream.CopyToAsync(_stream);
                    stream.Position = 0;
                }
                else
                {
                    _androidByteArray = GetImageStreamAsBytes(stream);
                }
                productImage.Source = ImageSource.FromStream(() => stream);
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
    }
}
