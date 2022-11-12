using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultDataPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant = Service.CurrentRace.CurrentParticipant;
        public RaceResultDataPage()
        {
            InitializeComponent();

            raceDataLayout.BindingContext = Service.CurrentRace;

            personalResultLayout.BindingContext = CurrentParticipant.Result;
        }
    }
}