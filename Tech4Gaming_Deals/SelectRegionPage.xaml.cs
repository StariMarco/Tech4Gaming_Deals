using System;
using System.Collections.Generic;
using System.Reflection;
using FFImageLoading.Svg.Forms;
using SVG.Forms.Plugin.Abstractions;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using System.Xml.Linq;
using Tech4Gaming_Deals.Managers;

namespace Tech4Gaming_Deals
{
    public partial class SelectRegionPage : ContentPage
    {
        public SelectRegionPage()
        {
            InitializeComponent();
            SetupLocationButtonsBindingContext();
            LoadLocations();
        }

        private void LoadLocations()
        {
            var selectedLocations = (Application.Current as App).SelectedLocations;
            if (selectedLocations.Count == 0)
                return;

            btnSelect.IsVisible = true;

            if (selectedLocations.Find(l => String.Equals(l.Name, "Italy", StringComparison.OrdinalIgnoreCase)) != null)
                svgItaly.BackgroundColor = (Color)Application.Current.Resources["colorPrimary"];
            if (selectedLocations.Find(l => String.Equals(l.Name, "America", StringComparison.OrdinalIgnoreCase)) != null)
                svgAmerica.BackgroundColor = (Color)Application.Current.Resources["colorPrimary"];
            if (selectedLocations.Find(l => String.Equals(l.Name, "England", StringComparison.OrdinalIgnoreCase)) != null)
                svgEngland.BackgroundColor = (Color)Application.Current.Resources["colorPrimary"];
        }

        private void SetupLocationButtonsBindingContext()
        {
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
                app.SelectedLocations.RemoveAt(app.SelectedLocations.IndexOf(app.SelectedLocations.Find(l => l.Name == (button.BindingContext as Location).Name)));
                if (app.SelectedLocations.Count == 0)
                    btnSelect.IsVisible = false;
            }
            else
            {
                // Select
                sourceImage.BackgroundColor = (Color) Application.Current.Resources["colorPrimary"];
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
            LocalDataManager.Save((Application.Current as App).GetLocalDataToSave());
            Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
