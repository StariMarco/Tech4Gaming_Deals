using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tech4Gaming_Deals.Managers;
using Tech4Gaming_Deals.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tech4Gaming_Deals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public List<MasterPageItem> MenuItemsList { get; set; }
        private App _app;

        #region Init

        public MasterPage()
        {
            InitializeComponent();

            _app = Application.Current as App;

            imgBanner.Source = ImageSource.FromResource("Tech4Gaming_Deals.Images.Tech4Gaming_Banner.png");

            InitializeMenuItemsList();

            lstCategories.ItemsSource = _app.CategoryList;

            // Setting home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProductsPage)));
        }

        private void InitializeMenuItemsList()
        {
            // Adding menu items
            MenuItemsList = new List<MasterPageItem>()
            {
                new MasterPageItem(){Title="Settings", TargetType=typeof(SettingPage), Icon="ic_settings.png"},
                new MasterPageItem(){Title="Shopping Cart", TargetType=typeof(ShoppingCartPage), Icon="ic_shopping_cart.png"}
            };

            lstNavigation.ItemsSource = MenuItemsList;
            // Fixing iOS listview
            if (Device.RuntimePlatform == Device.iOS)
            {
                iosBoxView.IsVisible = true;
                lstNavigation.HeightRequest = MenuItemsList.Count * 60;
            }

        }

        #endregion

        #region Categories filter

        private async void UpdateCategoryState(object sender, ItemTappedEventArgs e)
        {
            var category = e.Item as ProductCategory;
            int index = _app.CategoryList.IndexOf(_app.CategoryList.SingleOrDefault(c => c == category));

            SwitchCategoryState(category);

            await UpdateCategoryList(category, index);
        }

        private async void SelectAllCategories(object sender, EventArgs e)
        {
            int categoryIndex = 0;
            foreach(ProductCategory category in _app.CategoryList.ToList())
            {
                if (category.IsSelected)
                    continue;

                categoryIndex = _app.CategoryList.IndexOf(_app.CategoryList.SingleOrDefault(c => c == category));

                SwitchCategoryState(category);

                await UpdateCategoryList(category, categoryIndex);
            }
        }

        private async void DeselectAllCategories(object sender, EventArgs e)
        {
            int categoryIndex = 0;
            foreach (ProductCategory category in _app.CategoryList.ToList())
            {
                if (!category.IsSelected)
                    continue;

                categoryIndex = _app.CategoryList.IndexOf(_app.CategoryList.SingleOrDefault(c => c == category));

                SwitchCategoryState(category);

                await UpdateCategoryList(category, categoryIndex);
            }
        }

        private async Task UpdateCategoryList(ProductCategory category, int categoryIndex)
        {
            try
            {
                await _app.UpdateCategoryList(category, categoryIndex);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Hold up", "OK");
            }
        }

        private void SwitchCategoryState(ProductCategory category)
        {
            if (category.IsSelected)
                category.Image = "ic_label_outline.png";
            else
                category.Image = "ic_label.png";

            category.IsSelected = !category.IsSelected;

            // Change apply button aspect
            btnApply.BackgroundColor = (Color)Application.Current.Resources["colorPrimary"];
            btnApply.TextColor = Color.White;
        }

        #endregion

        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listView = sender as ListView;
            listView.SelectedItem = false;
        }

        private async void OnMenuItemTapped(object sender, ItemTappedEventArgs e)
        {
            var pageType = (e.Item as MasterPageItem).TargetType;

            // Navigate to the selected page
            await Detail.Navigation.PushAsync((Page)Activator.CreateInstance(pageType));
            IsPresented = false;
        }

        private void SaveCategories()
        {
            var data = _app.GetLocalDataToSave();
            LocalDataManager.Save(data);
        }

        private async void OnApplyCategories(object sender, EventArgs e)
        {
            btnApply.BackgroundColor = Color.Transparent;
            btnApply.TextColor = (Color)Application.Current.Resources["colorPrimary"];

            await _app.FilterProductsAsync(true);
            SaveCategories();
        }
    }
}
