using System;
using System.Collections.Generic;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace Tech4Gaming_Deals
{
    public partial class ContactUsPage : ContentPage
    {
        private Color _primaryColor;
        private Color _primaryColorLight;

        public ContactUsPage()
        {
            InitializeComponent();
            _primaryColor = (Color)Application.Current.Resources["colorPrimary"];
            _primaryColorLight = (Color)Application.Current.Resources["colorPrimaryLight"];
        }

        #region Send Email

        private async void OnSendEmailAsync(object sender, EventArgs e)
        {
            if(btnSendEmail.BackgroundColor != _primaryColor)
            {
                await DisplayAlert("Error", "Please fill all the entries", "OK");
                return;
            }

            await SendEmailAsync(txtSubject.Text, txtBody.Text);

            await Navigation.PopModalAsync();
        }

        private async Task SendEmailAsync(string subject, string body)
        {
            try
            {
                SendEmail(subject, body);
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Error", "Email is not supported on this device", "OK");
                return;
            }
            catch(Exception e)
            {
                await DisplayAlert("Error", $"An error occurred \n {e.Message}", "OK");
                return;
            }

            await DisplayAlert("Done", $"Your email was set successfully", "OK");
        }

        private static void SendEmail(string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            // TODO: Change both emails
            mail.From = new MailAddress("marco6411footballer@gmail.com");
            mail.To.Add("stari.marco11@gmail.com");
            mail.Subject = subject;
            mail.Body = body;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("marco6411footballer@gmail.com", "Milan-ur");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        #endregion

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void BackToSettingsPage(object sender, EventArgs e)
        {
            bool discardConfermation = await DisplayAlert("Alert", "By going back you will lose your work!", "OK", "CANCEL");
            if (!discardConfermation)
                return;

            await Navigation.PopModalAsync();
        }

        private bool IsEmailReady()
        {
            return !(String.IsNullOrWhiteSpace(txtSubject.Text) ||
                String.IsNullOrWhiteSpace(txtBody.Text));
        }

        private void CheckEmailCompletition(object sender, TextChangedEventArgs e)
        {
            if (IsEmailReady())
                btnSendEmail.BackgroundColor = _primaryColor;
            else
                btnSendEmail.BackgroundColor = _primaryColorLight;
        }
    }
}
