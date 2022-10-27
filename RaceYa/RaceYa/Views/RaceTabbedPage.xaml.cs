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
    public partial class RaceTabbedPage : TabbedPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant;

        CancellationTokenSource cts;

        public static StopWatch PageStopWatch = new StopWatch();

        public RaceTabbedPage()
        {
            InitializeComponent();
            CurrentParticipant = Service.CurrentRace.CurrentParticipant;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            PageStopWatch.SetTimer();

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

                CurrentParticipant.Result.CoveredDistance = CurrentParticipant.Result.CalculateCoveredDistance();
 
                if (CurrentParticipant.Result.CoveredDistance != 0)
                {
                    Service.CurrentRace.UpdateLeaderBoard();

                    CurrentParticipant.Result.AverageSpeed = CurrentParticipant.Result.CalculateAverageSpeed();
                }
                
                /*
                if (CurrentParticipant.Result.CoveredDistance > Service.CurrentRace.RouteLength)
                { 
                    CurrentParticipant.Result.ManageExceedingDistance();
                }
                */
                
            }
            CurrentParticipant.Result.RaceCompleted = true;
            Service.CurrentRace.CalculateFinalLeaderBoard();
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