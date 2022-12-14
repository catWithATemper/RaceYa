using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveRaceLeaderboardPage : ContentPage
    {
        public static GlobalContext Context = GlobalContext.Instance();

        //public static Participant CurrentParticipant = Service.CurrentRace.CurrentParticipant;

        public static TextToSpeechServiceManager TextToSpeechService = ActiveRaceTabbedPage.TextToSpeechService;

        public ActiveRaceLeaderboardPage()
        {
            InitializeComponent();

            leaderBoardView.ItemsSource = Context.CurrentRace.ObservableLeaderBoard;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Context.CurrentRace.UpdateObservableLeaderboard();
        }

        private async void leaderBoardPageButton_Clicked(object sender, EventArgs e)
        {
            bool response = await DisplayAlert("Warning!", "Quit race before the finish line?", "Yes", "No");
            if (response)
            {
                MessagingCenter.Send(this, "Quit race");
                Context.CurrentParticipant.Result.RaceCompleted = true;

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