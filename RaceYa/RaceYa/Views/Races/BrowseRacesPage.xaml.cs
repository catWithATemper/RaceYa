using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using RaceYa.Helpers;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowseRacesPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public BrowseRacesPage()
        {
            InitializeComponent();

            racesListView.ItemsSource = Service.Races;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            racesListView.ItemsSource = null;
            var races = await FirestoreRace.Read();
            racesListView.ItemsSource = races;

        }
    }
}