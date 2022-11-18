using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

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
    }
}