using RaceYa.Views;
using System;
using System.Collections.Generic;
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

            Routing.RegisterRoute("CreateNewRacePage", typeof(CreateNewRacePage));
        }

        
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Service.UserIsAuthenticated = false;
            await Shell.Current.GoToAsync("//LoginPage");
        }
        
    }
}
