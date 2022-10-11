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
    public partial class HomePage : ContentPage
    {
        public static bool UserIsAuthenticated = false;

        public static DataExchangeService Service = DataExchangeService.Instance();
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            if (UserIsAuthenticated)
            {
                base.OnAppearing();
            }
            else
            {
                Navigation.PushAsync(new LoginPage());
            }

        }

        private async void syncButton_Clicked(object sender, EventArgs e)
        {   
            await Task.Factory.StartNew(() => { Service.SyncData(); });
            
        }

        private async void nextRaceButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RaceResultPage");
        }
    }
}