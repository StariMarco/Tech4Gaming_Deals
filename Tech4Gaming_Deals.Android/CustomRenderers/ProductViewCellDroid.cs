using System;
using System.ComponentModel;
using Android.Content;
using Android.Views;
using Tech4Gaming_Deals.CustomControls;
using Tech4Gaming_Deals.Droid.CustomRenderers;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ProductViewCell), typeof(ProductViewCellDroid))]
namespace Tech4Gaming_Deals.Droid.CustomRenderers
{
    public class ProductViewCellDroid : ViewCellRenderer
    {
        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnCellPropertyChanged(sender, e);

            var cell = sender as ViewCell;
            var product = cell.BindingContext as Product;

            var lblPrice = cell.FindByName("lblProductPrice") as Label;
            var lblSalePrice = cell.FindByName("lblProductSalePrice") as Label;

            if (product.SalePrice > 0)
            {
                // sale price
                lblSalePrice.IsVisible = true;
                lblSalePrice.TextColor = (Color)Application.Current.Resources["colorRed"];
                lblSalePrice.FontSize = (double)Application.Current.Resources["normalFontSize"];

                lblPrice.TextColor = (Color)Application.Current.Resources["colorSecondaryText"];
                lblPrice.FontSize = (double)Application.Current.Resources["smallFontSize"];

                if (String.IsNullOrWhiteSpace(lblSalePrice.Text))
                    lblSalePrice.Text = product.SalePriceText;
            }
            else
            {
                // no sale price
                lblSalePrice.IsVisible = false;
                lblPrice.TextColor = (Color)Application.Current.Resources["colorRed"];
                lblPrice.FontSize = (double)Application.Current.Resources["normalFontSize"];
            }
        }
    }
}
