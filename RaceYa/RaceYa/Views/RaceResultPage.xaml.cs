using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using System.Threading;
using Xamarin.Essentials;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultPage : ContentPage
    {

        RaceResult result = new RaceResult();

        CancellationTokenSource cts;
        public RaceResultPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = result;
            distanceLabel.Text = result.Distance.ToString();
            avgSpeedLabel.Text = result.AverageSpeed.ToString();
            latitudeLabel.Text = "";
            longitudeLabel.Text = "";
        }

         async void OnButtonClicked(object sender, EventArgs e)
         {
            (sender as Button).BackgroundColor = Color.FromRgb(211, 211, 211);

            await CalculateRaceResult();

            distanceLabel.SetValue(Label.TextProperty, String.Format("{0:#}", result.Distance));
            avgSpeedLabel.SetValue(Label.TextProperty, String.Format("{0:#.##}", result.AverageSpeed));
            latitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Latitude);
            longitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Longitude);
         }

        public async Task CalculateRaceResult()
        {
            Location currentLocation = await GetCurrentLocation();
            result.SetCurrentLocation(currentLocation);
            result.SetStartingPoint();
            result.SetStartTime();

            while (result.Distance <= result.RouteLength)
            {
                await Task.Delay(1000);
                currentLocation = await GetCurrentLocation();
                result.SetCurrentLocation(currentLocation);

                latitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Latitude);
                longitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Longitude);

                result.Distance = result.CalculateDistance();

                distanceLabel.SetValue(Label.TextProperty, String.Format("{0:#}", result.Distance));

                result.SetTimeSinceStart();
                result.AverageSpeed = result.CalculateAverageSpeed();

                avgSpeedLabel.SetValue(Label.TextProperty, String.Format("{0:#.##}", result.AverageSpeed));
            }
        }

        public async Task<Location> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Geolocation.GetLocationAsync(request, cts.Token);

            return location;
        }



    }

}