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
    public partial class LeaderboardPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public string CurrentUser = Service.CurrentRace.CurrentParticipant.User.Name;

        public LeaderboardPage()
        {
            InitializeComponent();

            BindingContext = Service.CurrentRace;

            leaderBoardView.ItemsSource = Service.CurrentRace.ObservableLeaderBoard;

            Console.WriteLine(CurrentUser);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Service.CurrentRace.UpdateObservableLeaderboard();
        }

        public void HighlightCurrentParticipant()
        {
            

        }

    }
}