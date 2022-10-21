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

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceTabbedPage : TabbedPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant;

        CancellationTokenSource cts;

        public RaceTabbedPage()
        {
            InitializeComponent();
            CurrentParticipant = Service.CurrentRace.CurrentParticipant;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CalculateRaceResult();
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

                //latitudeLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.CurrentLocation.Latitude.ToString("f8"));
                //longitudeLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.CurrentLocation.Longitude.ToString("F8"));

                CurrentParticipant.Result.CoveredDistance = CurrentParticipant.Result.CalculateCoveredDistance();
                
                //distanceLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.CoveredDistance.ToString("F0"));

                if (CurrentParticipant.Result.CoveredDistance != 0)
                {
                    Service.CurrentRace.UpdateLeaderBoard();

                    CurrentParticipant.Result.AverageSpeed = CurrentParticipant.Result.CalculateAverageSpeed();

                    //avgSpeedLabel.SetValue(Label.TextProperty, CurrentParticipant.Result.AverageSpeed.ToString("F2"));
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
    }
}