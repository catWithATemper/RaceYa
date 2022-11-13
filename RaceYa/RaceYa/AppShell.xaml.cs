using RaceYa.Views;
using System;
using Xamarin.Forms;

using RaceYa.Models;

namespace RaceYa
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static DataExchangeService Service = DataExchangeService.Instance();
        public AppShell()
        {
            InitializeComponent();
        }

        
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Service.UserIsAuthenticated = false;
            await Shell.Current.GoToAsync("//LoginPage");

            Service.UserIsAuthenticated = false;
        }
        
    }
}
