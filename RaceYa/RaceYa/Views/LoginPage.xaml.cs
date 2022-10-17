using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

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
            if (!string.IsNullOrEmpty(userNameEntry.Text))
            {
                CurrentUser = new User(userNameEntry.Text);
                Service.UserIsAuthenticated = true;
                
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}