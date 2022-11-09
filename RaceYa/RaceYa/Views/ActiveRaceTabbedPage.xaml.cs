using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using System.Threading;
using System.Timers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveRaceTabbedPage : TabbedPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant;

        public static StopWatch PageStopWatch = new StopWatch();

        public static LocationServiceManager LocationService = new LocationServiceManager();

        public static TextToSpeechServiceManager TextToSpeechService = new TextToSpeechServiceManager();

        //For text to speech management

        public ActiveRaceTabbedPage()
        {
            InitializeComponent();
            CurrentParticipant = Service.CurrentRace.CurrentParticipant;

            MessagingCenter.Subscribe<ActiveRaceDataPage>(this, "Quit race", (sender) =>
            {
                //urrentParticipant.Result.RaceCompleted = true;
                Service.CurrentRace.CalculateFinalLeaderBoard();
            });
            MessagingCenter.Subscribe<ActiveRaceLeaderboardPage>(this, "Quit race", (sender) =>
            {
                //CurrentParticipant.Result.RaceCompleted = true;
                Service.CurrentRace.CalculateFinalLeaderBoard();
            });
        }

        //TODO Fix "Race complete" messages

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            PageStopWatch.SetTimer();

            TextToSpeechService.StartTextToSpeech();

            await CalculateRaceResult();
        }

        public async Task CalculateRaceResult()
        {
            Location currentLocation = await LocationService.GetCurrentLocation();

            CurrentParticipant.Result.SetCurrentLocation(currentLocation);
            CurrentParticipant.Result.SetStartingPoint();

            await TextToSpeech.SpeakAsync("Gps found");

            while (CurrentParticipant.Result.CoveredDistance <= Service.CurrentRace.RouteLength &&
                   CurrentParticipant.Result.RaceCompleted == false)
            { 
                await Task.Delay(1000);
                currentLocation = await LocationService.GetCurrentLocation();
                CurrentParticipant.Result.SetCurrentLocation(currentLocation);
                CurrentParticipant.Result.CalculateTimeSinceStart();

                CurrentParticipant.Result.CoveredDistance = CurrentParticipant.Result.CalculateCoveredDistance();
 
                if (CurrentParticipant.Result.CoveredDistance != 0)
                {
                    Service.CurrentRace.UpdateLeaderBoard();

                    CurrentParticipant.Result.AverageSpeed = CurrentParticipant.Result.CalculateAverageSpeed();
                }
            }
            await DisplayAlert("Race Complete!", "Tap \"OK\" to view your result.", "OK");

            CurrentParticipant.Result.RaceCompleted = true;
            Service.CurrentRace.CalculateFinalLeaderBoard();

            await Shell.Current.GoToAsync("//RaceResultTabbedPage");
            await Navigation.PopModalAsync();
        }
    }
}