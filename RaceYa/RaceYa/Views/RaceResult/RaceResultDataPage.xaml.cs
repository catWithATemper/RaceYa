using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultDataPage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static GlobalContext Context = GlobalContext.Instance();

        //public static Participant CurrentParticipant = Service.CurrentRace.CurrentParticipant;
        public RaceResultDataPage()
        {
            InitializeComponent();

            raceDataLayout.BindingContext = Context.CurrentRace;

            if (Context.CurrentParticipant != null && Context.CurrentParticipant.Result != null)
            {
                personalResultLayout.BindingContext = Context.CurrentParticipant.Result;
            }
        }
    }
}