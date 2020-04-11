using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Rg.Plugins.Popup.Pages;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;

namespace Tech4Gaming_Deals
{
    public partial class ProductPreviewPopup : PopupPage
    {
        public ProductPost NewProduct { get; set; }
        public byte[] productImage { get; set; }
        public NewProductPage newProductPage { get; set; }

        public ProductPreviewPopup(ProductPost product, byte[] image, NewProductPage page)
        {
            InitializeComponent();

            this.NewProduct = product;
            this.productImage = image;
            this.newProductPage = page;

            SetupProductFrame();
            SetupProductDetailsFrame();
        }

        private void SetupProductFrame()
        {
            imgProduct.Source = ImageSource.FromStream(() => new MemoryStream(productImage));
            lblDate.Text = NewProduct.ExpireTime;
        }

        private void SetupProductDetailsFrame()
        {
            lblNameDetail.Text = NewProduct.Name;
            imgProductDetail.Source = ImageSource.FromStream(() => new MemoryStream(productImage));

            // Price
            lblPriceDetail.Text = $"{NewProduct.Price.ToString()}{NewProduct.CurrencySymbol}";
            lblSalePriceDetail.Text = $"{NewProduct.SalePrice.ToString()}{NewProduct.CurrencySymbol}";
            if (NewProduct.SalePrice > 0)
            {
                lblSalePriceDetail.IsVisible = true;
                lblPriceDetail.TextColor = (Color)Application.Current.Resources["colorSecondaryText"];
                lblPriceDetail.TextDecorations = TextDecorations.Strikethrough;
                lblPriceDetail.FontSize = lblSalePriceDetail.FontSize - 2;
            }
            else
            {
                lblSalePriceDetail.IsVisible = false;
                lblPriceDetail.TextColor = (Color)Application.Current.Resources["colorRed"];
                lblPriceDetail.TextDecorations = TextDecorations.None;
                lblPriceDetail.FontSize = lblSalePriceDetail.FontSize + 2;
            }

            // Description
            lblDescriptionDetail.Text = NewProduct?.Description;
            if (String.IsNullOrWhiteSpace(NewProduct.Description))
                lblDescTextDetail.IsVisible = false;
            else
                lblDescTextDetail.IsVisible = true;

        }

        private void OnModifyClicked(object sender, EventArgs e)
        {
            SetEntriesVisibility(true);
            SetEntriesValue();
        }

        private void OnSaveChangesClicked(object sender, EventArgs e)
        {
            SetEntriesVisibility(false);
            SaveEntriesValue();
            SetupProductDetailsFrame();
        }

        private void OnCancelChangesClicked(object sender, EventArgs e)
        {
            SetEntriesVisibility(false);
        }

        private void SaveEntriesValue()
        {
            NewProduct.Name = entryName.Text;
            NewProduct.Description = entryDescription.Text;

            NewProduct.SalePrice = 0;
            NewProduct.Price = 0;

            if(String.Equals(entrySalePrice.Text, "$", StringComparison.Ordinal) || String.Equals(entrySalePrice.Text, "€", StringComparison.Ordinal) || String.Equals(entrySalePrice.Text, "£", StringComparison.Ordinal))
            {
                entrySalePrice.Text = "0";
            }
            if (String.Equals(entryPrice.Text, "$", StringComparison.Ordinal) || String.Equals(entryPrice.Text, "€", StringComparison.Ordinal) || String.Equals(entryPrice.Text, "£", StringComparison.Ordinal))
            {
                entryPrice.Text = "0";
            }

            try
            {
                if(!String.IsNullOrEmpty(entrySalePrice.Text))
                    NewProduct.SalePrice = float.Parse(Regex.Replace(entrySalePrice.Text, "[€$£]", ""));
                if (!String.IsNullOrEmpty(entryPrice.Text))
                    NewProduct.Price = float.Parse(Regex.Replace(entryPrice.Text, "[€$£]", ""));
            }
            catch (Exception ex)
            {

            }
        }

        private void SetEntriesValue()
        {
            entryName.Text = lblNameDetail.Text;
            entryDescription.Text = lblDescriptionDetail.Text;
            entrySalePrice.Text = lblSalePriceDetail.Text;
            entryPrice.Text = lblPriceDetail.Text;
        }

        private void SetEntriesVisibility(bool value)
        {
            entryName.IsVisible = value;
            entryDescription.IsVisible = value;
            entryPrice.IsVisible = value;
            entrySalePrice.IsVisible = value;

            lblNameDetail.IsVisible = !value;
            lblDescriptionDetail.IsVisible = !value;
            lblPriceDetail.IsVisible = !value;
            lblSalePriceDetail.IsVisible = !value;

            btnModify.IsVisible = !value;
            gridModify.IsVisible = value;
        }

        protected override bool OnBackgroundClicked()
        {
            newProductPage.LoadChanges();
            return base.OnBackgroundClicked();
        }
    }
}
