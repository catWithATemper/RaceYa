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
        public static DBQuickStartService DBQuickStart = DBQuickStartService.Instance();

        public static GlobalContext Parameters = GlobalContext.Instance();

        public static User CurrentUser;

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(emailEntry.Text) && !string.IsNullOrEmpty(passwordEntry.Text))
            {
                //Console.WriteLine("Authenticated? " + Auth.IsAuthenticated()); //Returns true, was expecting false

                bool result = await Auth.LogInUser(emailEntry.Text, passwordEntry.Text);

                if (result)
                {
                    Parameters.CurrentUser = await FirestoreUser.ReadUserByUserId(Auth.GetCurrentUserId());
                    DBQuickStart.UserIsAuthenticated = true;
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