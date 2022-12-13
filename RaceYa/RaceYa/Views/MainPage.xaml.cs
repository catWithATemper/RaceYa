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
            if (Service.UserIsAuthenticated)
            {
                base.OnAppearing();

                if (Service.dataCreated == false)
                {
                    //Service.CreateData();
                }

                nextRaceStackLayout.BindingContext = null;
                if (Context.CurrentRace != null)
                {
                    nextRaceStackLayout.BindingContext = Context.CurrentRace;
                }

                Context.CurrentParticipant = await FirestoreParticipant.ReadParticpantByUserAndRace(Context.CurrentUser.Id, Context.CurrentRace.Id);
                if (Context.CurrentParticipant != null )
                {
                    Context.CurrentParticipant.AssignRace(Context.CurrentRace);
                    Context.CurrentParticipant.AssignUser(Context.CurrentUser);
                    Context.CurrentParticipantResult = await FirestoreRaceResult.ReadRaceRaesultByParticipantId(Context.CurrentParticipant.Id);
                    Context.CurrentParticipantResult.RaceParticipant = Context.CurrentParticipant;
                    Context.CurrentParticipant.Result = Context.CurrentParticipantResult;
                    Context.CurrentParticipantResult.GPXRequired = true;

                    Context.CurrentRace.CurrentParticipant = Context.CurrentParticipant;
                    Context.CurrentParticipant.IsCurrentParticipant = true;
                }

                if (Context.CurrentParticipant.Result.RaceCompleted == true)
                {
                    latestRaceStackLayout.BindingContext = Context.CurrentParticipant.Result;
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