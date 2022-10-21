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
    public partial class RaceDataPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant;

        public RaceDataPage()
        {
            InitializeComponent();

            CurrentParticipant = Service.CurrentRace.CurrentParticipant;

            BindingContext = CurrentParticipant.Result;

            //UpdateRaceDataLabels();
        }


        /*
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await UpdateRaceDataLabels();
        }
        */
        
        


        public async Task UpdateRaceDataLabels()
        {
            await Task.Run(() => {

                while (CurrentParticipant.Result.CoveredDistance <= Service.CurrentRace.RouteLength)
                {
                    distanceLabel.Text = CurrentParticipant.Result.CoveredDistance.ToString("F0");
                    avgSpeedLabel.Text = CurrentParticipant.Result.AverageSpeed.ToString("F2");

                    if (CurrentParticipant.Result.CurrentLocation != null)
                    {
                        latitudeLabel.Text = CurrentParticipant.Result.CurrentLocation.Latitude.ToString("F8");
                        longitudeLabel.Text = CurrentParticipant.Result.CurrentLocation.Longitude.ToString("F8");
                    }
                }
            }); 
        }
    }
}