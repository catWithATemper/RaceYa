using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using RaceYa.Helpers;
using System.Collections.Generic;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //TODO: Remove dependency servic and try directly calling firestore from the helper classes.
   
        public static DBQuickStartService DBQuickStart = DBQuickStartService.Instance();

        public static GlobalContext Context = GlobalContext.Instance();

        public static Race CurrentRace;

        public static User CurrentUser;

        public static Participant CurrentParticipant;

        public static RaceResult CurrentParticipantResult;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (Context.UserIsAuthenticated)
            {
                base.OnAppearing();

                if (DBQuickStart.dataCreated == false)
                {
                    //DBQuickStart.CreateData();
                }

                nextRaceStackLayout.BindingContext = null;
                if (Context.CurrentRace != null)
                {
                    nextRaceStackLayout.BindingContext = Context.CurrentRace;
                }

                latestRaceStackLayout.BindingContext = null;
                if (Context.LatestResult != null && Context.LatestResult.CoveredDistance > 0)
                {
                    latestRaceStackLayout.BindingContext = Context.LatestResult;
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }

        private async void nextRaceButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//NextRacePage");
        }

        private async void latestRaceButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RaceResultTabbedPage");
        }

        private async void browseRacesButton_Clicked(object sender, EventArgs e)
        {
            RacesTabbedPage.SelectedTab = 0;
            await Shell.Current.GoToAsync("//SignUpForARacePage");
        }

        private async void createNewRaceButton_Clicked(object sender, EventArgs e)
        {
            RacesTabbedPage.SelectedTab = 1;
            await Shell.Current.GoToAsync("//SignUpForARacePage");
        }
    }
}