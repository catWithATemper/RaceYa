using RaceYa.Views;
using System;
using Xamarin.Forms;
using RaceYa.Helpers;

using RaceYa.Models;

namespace RaceYa
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public static GlobalContext Context = GlobalContext.Instance();
        public AppShell()
        {
            InitializeComponent();
        }

        
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Context.UserIsAuthenticated = false;
            Auth.LogOutUser();
            await Shell.Current.GoToAsync("//LoginPage");
        }
        
    }
}
