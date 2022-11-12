﻿using System;
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

        public static Participant CurrentParticipant;

        

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

                    CurrentParticipant = new Participant(LoginPage.CurrentUser, Service.CurrentRace);
                    Service.CurrentRace.CurrentParticipant = CurrentParticipant;
                    CurrentParticipant.IsCurrentParticipant = true;
                }

                nextRaceStackLayout.BindingContext = Service.CurrentRace;

                if (CurrentParticipant.Result.RaceCompleted == true)
                {
                    latestRaceStackLayout.BindingContext = CurrentParticipant.Result;
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

        private async void latestRaceButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RaceResultTabbedPage");
        }

        private async void browseRaceButton_Clicked(object sender, EventArgs e)
        {
            SignUpForARaceTabbedPage.SelectedTab = 0;
            await Shell.Current.GoToAsync("//SignUpForARacePage");
        }

        private async void createNewRaceButton_Clicked(object sender, EventArgs e)
        {
            SignUpForARaceTabbedPage.SelectedTab = 1;
            await Shell.Current.GoToAsync("//SignUpForARacePage");
        }
    }
}