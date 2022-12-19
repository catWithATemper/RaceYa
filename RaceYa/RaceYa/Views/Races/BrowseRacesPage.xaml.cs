using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using RaceYa.Helpers;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowseRacesPage : ContentPage
    {
        public static GlobalContext Context = GlobalContext.Instance();

        //TODO: Create a race result for the participant when signing up

        public BrowseRacesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            racesListView.ItemsSource = null;
            var races = await FirestoreRace.ReadRacesForSigningUp(Context.CurrentUser.Id);
            racesListView.ItemsSource = races;
        }

        private async void signUpButton_Clicked(object sender, System.EventArgs e)
        {
            Race selectedRace = (Race)((Button)sender).CommandParameter;
            Participant newParticipant = new Participant(Context.CurrentUser, selectedRace,
                                                         Context.CurrentUser.Id, selectedRace.Id);
            string participantId = await FirestoreParticipant.Add(newParticipant);

            RaceResult newResult = new RaceResult(newParticipant);
            string resultId = await FirestoreRaceResult.Add(newResult, newParticipant.Id);

            RaceResultGPX newResultGPX = new RaceResultGPX();
            string newResultGPXId = await FirestoreRaceResultGPX.Add(newResultGPX, newParticipant.Id, newResult.Id);

            if (participantId != null && resultId != null && newResultGPXId != null)
            {
                await DisplayAlert("Success", "You successfully signed up for this race.", "Ok");

                var tabbedPage = this.Parent as TabbedPage;
                tabbedPage.CurrentPage = tabbedPage.Children[2];
            }
            else
                await DisplayAlert("Failure", "We could not sign you up. Please try again", "Ok");

        }
    }
}