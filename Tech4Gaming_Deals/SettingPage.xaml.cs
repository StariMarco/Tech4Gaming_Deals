using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Tech4Gaming_Deals
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        private async void OpenEmailEditor(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ContactUsPage());
        }
        // TODO: Add more settings
    }
}
