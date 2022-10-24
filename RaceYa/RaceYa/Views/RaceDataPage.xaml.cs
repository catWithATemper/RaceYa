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

        //public StopWatch PageStopWatch = new StopWatch();

        public StopWatch PageStopWatch = RaceTabbedPage.PageStopWatch;

        public RaceDataPage()
        {
            InitializeComponent();

            CurrentParticipant = Service.CurrentRace.CurrentParticipant;

            BindingContext = CurrentParticipant.Result;
            timerLabel.BindingContext = PageStopWatch;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //PageStopWatch.SetTimer();
        }
    }
}