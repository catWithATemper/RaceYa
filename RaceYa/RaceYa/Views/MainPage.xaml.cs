using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using RaceYa.Helpers;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static GlobalParameters Parameters = GlobalParameters.Instance();

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
            if (Service.UserIsAuthenticated)
            {
                base.OnAppearing();

                //Service.PopulateRaceResultsFromFiles();

                nextRaceStackLayout.BindingContext = null;
                if (Parameters.CurrentRace != null)
                {
                    nextRaceStackLayout.BindingContext = Parameters.CurrentRace;
                }

                Parameters.CurrentParticipant = await FirestoreParticipant.ReadParticpantByUserAndRace(Parameters.CurrentUser.Id, Parameters.CurrentRace.Id);
                if (Parameters.CurrentParticipant != null )
                {
                    Parameters.CurrentParticipant.AssignRace(Parameters.CurrentRace);
                    Parameters.CurrentParticipant.AssignUser(Parameters.CurrentUser);
                    Parameters.CurrentParticipantResult = await FirestoreRaceResult.ReadRaceRaesultByParticipantId(Parameters.CurrentParticipant.Id);
                    Parameters.CurrentParticipantResult.RaceParticipant = Parameters.CurrentParticipant;
                    Parameters.CurrentParticipant.Result = Parameters.CurrentParticipantResult;
                    Parameters.CurrentParticipantResult.GPXRequired = true;

                    Parameters.CurrentRace.CurrentParticipant = Parameters.CurrentParticipant;
                    Parameters.CurrentParticipant.IsCurrentParticipant = true;
                }

                if (Parameters.CurrentParticipant.Result.RaceCompleted == true)
                {
                    latestRaceStackLayout.BindingContext = Parameters.CurrentParticipant.Result;
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
            SignUpForARaceTabbedPage.SelectedTab = 0;
            await Shell.Current.GoToAsync("//SignUpForARacePage");
        }

        private async void createNewRaceButton_Clicked(object sender, EventArgs e)
        {
            SignUpForARaceTabbedPage.SelectedTab = 1;
            await Shell.Current.GoToAsync("//SignUpForARacePage");
        }
    }
}