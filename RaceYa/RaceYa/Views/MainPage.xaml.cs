﻿using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using RaceYa.Helpers;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static GlobalParameters Parameters = GlobalParameters.Instance();

        public static Race CurrentRace = Service.CurrentRace;

        public static User CurrentUser = Service.CurrentUser;

        public static Participant CurrentParticipant = Service.CurrentParticipant;

        public static RaceResult CurrentParticipantResult = Service.CurrentParticipantResult;

        public Race NextRace;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (Service.UserIsAuthenticated)
            {
                base.OnAppearing();

                /*
                if (CurrentParticipant == null)
                {        
                    CurrentParticipant = new Participant(Parameters.CurrentUser, Service.CurrentRace, Parameters.CurrentUser.UserId, Service.CurrentRace.Id);
                    if (CurrentParticipant.Result == null)
                    {
                        CurrentParticipant.AssignRaceResult(CurrentParticipantResult);
                        CurrentParticipant.AddToParticipantsList(Service.CurrentRace);

                        Service.CurrentRace.CurrentParticipant = CurrentParticipant;
                        CurrentParticipant.IsCurrentParticipant = true;
                    }
                }
                */

                //Service.PopulateRaceResultsFromFiles();

                nextRaceStackLayout.BindingContext = null;
                NextRace = await FirestoreRace.ReadNextRace();
                nextRaceStackLayout.BindingContext = NextRace;
                Parameters.NextRace = NextRace;

                CurrentRace = Service.CurrentRace;
                CurrentUser = Service.CurrentUser;
                CurrentParticipant = Service.CurrentParticipant;
                CurrentParticipantResult = Service.CurrentParticipantResult;

                //Debug
                Parameters.NextRace = Service.CurrentRace;
                Parameters.CurrentUser = Service.CurrentUser;
                Parameters.CurrentParticipant = Service.CurrentParticipant;
                Parameters.CurrentParticipantResult = Service.CurrentParticipantResult;

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
            await Shell.Current.GoToAsync($"//NextRacePage");

            //string id = NextRace.Id;
            //await Shell.Current.GoToAsync($"//NextRacePage?raceId={id}");
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