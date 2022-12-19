﻿using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using RaceYa.Helpers;
using System.Collections.Generic;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static DBQuickStartService DBQuickStart = DBQuickStartService.Instance();

        public static GlobalContext Context = GlobalContext.Instance();

        public static Race CurrentRace;

        public static User CurrentUser;

        public static Participant CurrentParticipant;

        public static RaceResult CurrentParticipantResult;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (Context.UserIsAuthenticated)
            {
                base.OnAppearing();

                if (DBQuickStart.dataCreated == false)
                {
                    //DBQuickStart.CreateData();
                }

                nextRaceStackLayout.BindingContext = null;
                if (Context.CurrentRace != null)
                {
                    nextRaceStackLayout.BindingContext = Context.CurrentRace;

                    //Context.SetUpNextParticipantContext();
                }

                latestRaceStackLayout.BindingContext = null;
                if (Context.CurrentParticipant != null && Context.CurrentParticipant.Result != null &&
                    Context.CurrentParticipant.Result.RaceCompleted == true)
                {
                    latestRaceStackLayout.BindingContext = Context.CurrentParticipant.Result;
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }

        private async void nextRaceButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//NextRacePage");
        }

        private async void latestRaceButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RaceResultTabbedPage");
        }

        private async void browseRacesButton_Clicked(object sender, EventArgs e)
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