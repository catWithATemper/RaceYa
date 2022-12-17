using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using System.Threading.Tasks;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveRaceDataPage : ContentPage
    {
        public static GlobalContext Context = GlobalContext.Instance();

        public static Participant CurrentParticipant;

        public StopWatch PageStopWatch = ActiveRaceTabbedPage.PageStopWatch;

        public static TextToSpeechServiceManager TextToSpeechService = ActiveRaceTabbedPage.TextToSpeechService;

        public ActiveRaceDataPage()
        {
            InitializeComponent();

            //CurrentParticipant = Service.CurrentRace.CurrentParticipant;

            BindingContext = Context.CurrentParticipant.Result;
            timerLabel.BindingContext = PageStopWatch;
        }

        private async void RaceDataPageButton_Clicked(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Warning!", "Quit race before the finish line?", "Yes", "No");
            if (response)
            {
                MessagingCenter.Send(this, "Quit race");
                Context.CurrentParticipant.Result.RaceCompleted = true;

                if (PageStopWatch != null)
                {

                    PageStopWatch.StopTimer();
                }

                if (TextToSpeechService != null)
                {
                    TextToSpeechService.StopTextToSpeech();
                }

                await Shell.Current.GoToAsync("//RaceResultTabbedPage");
                await Navigation.PopModalAsync();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

    }
}