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
            var selectedRace = (Race)((Button)sender).CommandParameter;
            Participant newParticipant = new Participant(Context.CurrentUser, selectedRace,
                                                         Context.CurrentUser.Id, selectedRace.Id);
            string id = await FirestoreParticipant.Add(newParticipant);

            if (id != null)
            {
                await DisplayAlert("Success", "You successfully signed up for this race.", "Ok");
            }
            else
                await DisplayAlert("Failure", "We could not sign you up. Please try again", "Ok");

        }
    }
}