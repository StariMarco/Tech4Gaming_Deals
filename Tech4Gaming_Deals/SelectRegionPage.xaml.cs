using System;
using System.Collections.Generic;
using System.Reflection;
using FFImageLoading.Svg.Forms;
using SVG.Forms.Plugin.Abstractions;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using System.Xml.Linq;

namespace Tech4Gaming_Deals
{
    public partial class SelectRegionPage : ContentPage
    {
        public SelectRegionPage()
        {
            InitializeComponent();

            //NavigationPage.SetHasNavigationBar(this, false);

            btnItaly.BindingContext = new Location()
            {
                Name = "Italy",
                CurrencySymbol = "€"
            };
            btnAmerica.BindingContext = new Location()
            {
                Name = "America",
                CurrencySymbol = "$"
            };
            btnEngland.BindingContext = new Location()
            {
                Name = "England",
                CurrencySymbol = "£"
            };
        }

        private void OnSelectLocation(object sender, EventArgs e)
        {
            var button = sender as Button;
            var sourceImage = (button.CommandParameter as SvgCachedImage);
            var app = (Application.Current as App);

            // TODO: Change button image source
            if (IsSelected(sourceImage))
            {
                // Deselect
                sourceImage.BackgroundColor = Color.Transparent;
                sourceImage.InputTransparent = true;
                app.SelectedLocations.Remove(button.BindingContext as Location);
                if (app.SelectedLocations.Count == 0)
                    btnSelect.IsVisible = false;
            }
            else
            {
                // Select
                sourceImage.BackgroundColor = (Color) Application.Current.Resources["colorPrimary"];
                sourceImage.InputTransparent = true;
                app.SelectedLocations.Add(button.BindingContext as Location);
                btnSelect.IsVisible = true;
            }


        }

        private static bool IsSelected(SvgCachedImage image)
        {
            return image.BackgroundColor == (Color)Application.Current.Resources["colorPrimary"];
        }

        private void OnConfirm(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
