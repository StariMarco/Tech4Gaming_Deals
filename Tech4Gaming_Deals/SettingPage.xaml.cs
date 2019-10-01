using System;
using System.Collections.Generic;
using Tech4Gaming_Deals.Managers;
using Xamarin.Forms;

namespace Tech4Gaming_Deals
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();

            swtNotifications.On = (Application.Current as App).Notifications;
        }

        private async void OpenEmailEditor(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ContactUsPage());
        }

        private void OnSetLocations(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SelectRegionPage());
        }

        private void OpenPrivacyPolicy(object sender, EventArgs e)
        {
            // TODO: Go to privacy policy page
        }

        private void OnNotificationSwitched(object sender, EventArgs e)
        {
            (Application.Current as App).Notifications = (sender as SwitchCell).On;
            Console.WriteLine((sender as SwitchCell).On);
            var data = (Application.Current as App).GetLocalDataToSave();
            LocalDataManager.Save(data);
        }
    }
}
