using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveRaceLeaderboardPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant = Service.CurrentRace.CurrentParticipant;

        public ActiveRaceLeaderboardPage()
        {
            InitializeComponent();

            leaderBoardView.ItemsSource = Service.CurrentRace.ObservableLeaderBoard;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Service.CurrentRace.UpdateObservableLeaderboard();
        }

        private async void leaderBoardPageButton_Clicked(object sender, EventArgs e)
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