using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using System;
using System.Globalization;
using System.Xml;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveRaceTabbedPage : TabbedPage
    {
        //TODO: At the end of the race set IsCurrentParticipant = false in RaceResult

        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant;

        public static StopWatch PageStopWatch = new StopWatch();

        public static LocationServiceManager LocationService = new LocationServiceManager();

        public static TextToSpeechServiceManager TextToSpeechService;

        public ActiveRaceTabbedPage()
        {
            InitializeComponent();

            CurrentParticipant = Service.CurrentRace.CurrentParticipant;
            TextToSpeechService = new TextToSpeechServiceManager(CurrentParticipant.Result);

            MessagingCenter.Subscribe<ActiveRaceDataPage>(this, "Quit race", (sender) =>
            {
                Service.CurrentRace.CalculateFinalLeaderBoard();
            });
            MessagingCenter.Subscribe<ActiveRaceLeaderboardPage>(this, "Quit race", (sender) =>
            {
                Service.CurrentRace.CalculateFinalLeaderBoard();
            });
        }

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

            //debug
            Console.WriteLine("Start: Latitude " + currentLocation.Latitude + ", Longitude " + currentLocation.Longitude + 
                              ", Time " + currentLocation.Timestamp +
                              ", Accuracy " + currentLocation.Accuracy); 

            int counter = 0;
            while (CurrentParticipant.Result.CoveredDistance <= Service.CurrentRace.RouteLength &&
                   CurrentParticipant.Result.RaceCompleted == false)
            {
                await Task.Delay(100); //0.1 seconds
                counter++;
                
                //Check if the 1 second counter can be removed, since GetCurrentLocation() has its own
                //internal 4 seconds timeout.
                if (counter == 10)
                {
                    currentLocation = await LocationService.GetCurrentLocation();
                    CurrentParticipant.Result.SetCurrentLocation(currentLocation);

                    //debug
                    CurrentParticipant.Result.Accuracy = (double)currentLocation.Accuracy;
                    CurrentParticipant.Result.GPSSpeed = (double)currentLocation.Speed;

                    Console.WriteLine("Current location: Latitude " + currentLocation.Latitude + ", Longitude " + currentLocation.Longitude +
                                      ", Time " + currentLocation.Timestamp + 
                                      ", Formatted time: " + DateTime.SpecifyKind(currentLocation.Timestamp.DateTime, DateTimeKind.Utc).ToString("o", CultureInfo.InvariantCulture));
                    Console.WriteLine("Distance " + CurrentParticipant.Result.CoveredDistance +
                                      ", Avg speed " + CurrentParticipant.Result.AverageSpeed + 
                                      ", GPS speed " + currentLocation.Speed +
                                      ", Accuracy " + currentLocation.Accuracy);

                    //Check whether the if condition is necessary
                    if (CurrentParticipant.Result.CoveredDistance != 0)
                    {
                        Service.CurrentRace.UpdateLeaderBoard();
                    }
                    counter = 0;
                }
            }

            if (PageStopWatch != null)
            {
                PageStopWatch.StopTimer();
            }

            if (TextToSpeechService != null)
            {
                TextToSpeechService.StopTextToSpeech();
            }

            if (CurrentParticipant.Result.CoveredDistance >= Service.CurrentRace.RouteLength)
            {
                await DisplayAlert("Race Complete!", "Tap \"OK\" to view your result.", "OK");
            }

            CurrentParticipant.Result.RaceCompleted = true;

            Service.CurrentRace.CalculateFinalLeaderBoard();

            await Shell.Current.GoToAsync("//RaceResultTabbedPage");
            await Navigation.PopModalAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (TextToSpeechService != null)
            {
                TextToSpeechService.StopTextToSpeech();
            }
        }
    }
}