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
    public partial class ActiveRaceDataPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant;

        public StopWatch PageStopWatch = ActiveRaceTabbedPage.PageStopWatch;

        public ActiveRaceDataPage()
        {
            InitializeComponent();

            CurrentParticipant = Service.CurrentRace.CurrentParticipant;

            BindingContext = CurrentParticipant.Result;
            timerLabel.BindingContext = PageStopWatch;
        }

        private async void RaceDataPageButton_Clicked(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Warning!", "Quit race before the finish line?", "Yes", "No");
            if (response)
            {
                MessagingCenter.Send(this, "Quit race");

                await Shell.Current.GoToAsync("//MainPage");
                await Navigation.PopModalAsync();
            }
        }
    }
}