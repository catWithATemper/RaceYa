﻿using System;
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
    public partial class MainPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (Service.UserIsAuthenticated)
            {
                base.OnAppearing();

                if (Service.CurrentRace.Participants.Count == 0)
                {
                    await Task.Factory.StartNew(() => { Service.SyncData(); });
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }



        private async void nextRaceButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//NextRacePage");
        }



        /*
        private async void syncButton_Clicked(object sender, EventArgs e)
        {   
            await Task.Factory.StartNew(() => { Service.SyncData(); });

        }
        */

    }
}