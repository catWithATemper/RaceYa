using RaceYa.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RaceYa
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            HomePage.UserIsAuthenticated = false;
            await Shell.Current.GoToAsync("//LoginPage");
        }
        
    }
}
