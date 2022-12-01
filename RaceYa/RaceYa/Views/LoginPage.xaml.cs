using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using RaceYa.Helpers;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static User CurrentUser;

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(emailEntry.Text) && !string.IsNullOrEmpty(passwordEntry.Text))
            {
                bool result = await Auth.LogInUser(emailEntry.Text, passwordEntry.Text);

                if (result)
                {
                    CurrentUser = new User("CurrentUser", Auth.GetCurrentUserId());
                    Service.UserIsAuthenticated = true;
                    await Shell.Current.GoToAsync("//MainPage");
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}