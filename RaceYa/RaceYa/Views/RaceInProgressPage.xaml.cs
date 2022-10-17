using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using System.Threading;
using Xamarin.Essentials;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceInProgressPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant;

        CancellationTokenSource cts;

        public RaceInProgressPage()
        {
            InitializeComponent();
            CurrentParticipant = Service.CurrentRace.CurrentParticipant;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = Service.CurrentRace.CurrentParticipant.Result;
            distanceLabel.Text = "0";
            avgSpeedLabel.Text = "0";
            latitudeLabel.Text = "";
            longitudeLabel.Text = "";
        }

         async void OnStartButtonClicked(object sender, EventArgs e)
         {
            Location locationTest;

            startButton.IsEnabled = false;
            startButton.Text = "Searching for GPS... ";
            startButton.TextColor = Color.Red;

            try
            {
                locationTest = await GetCurrentLocation();

                if (locationTest != null)
                {
                    bool answer = await DisplayAlert("Start race?", "Tap \"OK\" to start the countdown", "OK", "Cancel");
                    //TODO Check again for location availability at regular intervals here. 
                    if (!answer)
                    {
                        ResetStartButton();
                        return;
                    }
                    else
                    {
                        startButton.TextColor = Color.Black;
                        for (int times = 5; times > 0; times--)
                        {
                            startButton.Text = times.ToString();
                            await Task.Delay(1000);
                        }

                        startButton.Text = "Run!";

                        CurrentParticipant.Result.SetCurrentLocation(locationTest);
                        latitudeLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.CurrentLocation.Latitude.ToString("F8"));
                        longitudeLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.CurrentLocation.Longitude.ToString("F8"));

                        await CalculateRaceResult();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ex.Message + ex.StackTrace + ex.InnerException);
                await DisplayAlert("Exception", "Unable to get location", "OK");
                ResetStartButton();
            }
        }

        public async Task CalculateRaceResult()
        {
            Location currentLocation = await GetCurrentLocation();

            CurrentParticipant.Result.SetCurrentLocation(currentLocation);
            CurrentParticipant.Result.SetStartingPoint();

            while (CurrentParticipant.Result.CoveredDistance <= Service.CurrentRace.RouteLength)
            {
                await Task.Delay(1000);
                currentLocation = await GetCurrentLocation();
                CurrentParticipant.Result.SetCurrentLocation(currentLocation);
                CurrentParticipant.Result.CalculateTimeSinceStart();

                latitudeLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.CurrentLocation.Latitude.ToString("f8"));
                longitudeLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.CurrentLocation.Longitude.ToString("F8"));

                CurrentParticipant.Result.CoveredDistance = CurrentParticipant.Result.CalculateCoveredDistance();

                distanceLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.CoveredDistance.ToString("F0"));

                //CurrentParticipant.Result.CalculateTimeSinceStart();

                if (CurrentParticipant.Result.CoveredDistance != 0)
                {
                    Service.CurrentRace.UpdateLeaderBoard();

                    CurrentParticipant.Result.AverageSpeed = CurrentParticipant.Result.CalculateAverageSpeed();

                    avgSpeedLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.AverageSpeed.ToString("F2"));
                }
            }
        }

        public async Task<Location> GetCurrentLocation()
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();

            Location location = null;
            while (location == null)
            {
                location = await Geolocation.GetLocationAsync(request, cts.Token);
            }
            return location;
        }

        private void ResetStartButton()
        {
            startButton.IsEnabled = true;
            startButton.Text = "START";
            startButton.TextColor = Color.White;
        }
    }
}