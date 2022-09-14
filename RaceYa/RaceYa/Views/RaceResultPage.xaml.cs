using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using System.Threading;
using Xamarin.Essentials;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultPage : ContentPage
    {
        public DataExchangeService Service = new DataExchangeService();
        RaceResult Result = new RaceResult();

        CancellationTokenSource cts;

        public RaceResultPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = Result;
            distanceLabel.Text = "0";
            avgSpeedLabel.Text = "0";
            latitudeLabel.Text = "";
            longitudeLabel.Text = "";
        }

         async void OnButtonClicked(object sender, EventArgs e)
         {
            Location LocationTest;

            StartButton.IsEnabled = false;
            StartButton.Text = "Searching for GPS... ";
            StartButton.TextColor = Color.Red;

            try
            {
                LocationTest = await GetCurrentLocation();

                if (LocationTest != null)
                {
                    bool Answer = await DisplayAlert("Start race?", "Tap \"OK\" to start the countdown", "OK", "Cancel");
                    //Check again for location availability at regular intervals here. 
                    if (!Answer)
                    {
                        StartButton.IsEnabled = true;
                        StartButton.Text = "START";
                        StartButton.TextColor = Color.White ;
                        return;
                    }
                    else
                    {
                        StartButton.TextColor = Color.Black;
                        for (int times = 5; times > 0; times--)
                        {
                            StartButton.Text = times.ToString();
                            await Task.Delay(1000);
                        }

                        StartButton.Text = "Run!";

                        //Debug
                        Result.SetCurrentLocation(LocationTest);
                        latitudeLabel.SetValue(Label.TextProperty, Result.CurrentLocation.Latitude.ToString("F8"));
                        longitudeLabel.SetValue(Label.TextProperty, Result.CurrentLocation.Longitude.ToString("F8"));

                        await CalculateRaceResult();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", "Unable to get location", "OK");
                StartButton.IsEnabled = true;
                StartButton.Text = "START";
                //Textcolor?
            }
        }

        public async Task CalculateRaceResult()
        {
            Location currentLocation = await GetCurrentLocation();
            
            Result.SetCurrentLocation(currentLocation);
            Result.SetStartingPoint();
            Result.SetStartTime();

            while (Result.CoveredDistance <= Service.CurrentRace.RouteLength)
            {
                await Task.Delay(1000);
                currentLocation = await GetCurrentLocation();
                Result.SetCurrentLocation(currentLocation);

                latitudeLabel.SetValue(Label.TextProperty, Result.CurrentLocation.Latitude.ToString("f8"));
                longitudeLabel.SetValue(Label.TextProperty, Result.CurrentLocation.Longitude.ToString("F8"));

                Result.CoveredDistance = Result.CalculateCoveredDistance();

                distanceLabel.SetValue(Label.TextProperty, Result.CoveredDistance.ToString("F0"));
 
                Result.CalculateTimeSinceStart();

                //Should check timeSinceStart > 0 
                if (Result.CoveredDistance != 0)
                {
                    Result.AverageSpeed = Result.CalculateAverageSpeed();

                    avgSpeedLabel.SetValue(Label.TextProperty, Result.AverageSpeed.ToString("F2"));
                }
            }
        }

        public async Task<Location> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();

            Location location = null;
            while (location == null)
            {
                location = await Geolocation.GetLocationAsync(request, cts.Token);
            }
            return location;
        }
    }
}