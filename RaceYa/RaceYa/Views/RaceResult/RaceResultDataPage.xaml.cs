using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultDataPage : ContentPage
    {
        public static GlobalContext Context = GlobalContext.Instance();

        //TODO: pace not shown when content is loaded from db

        public RaceResultDataPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Context.LatestRace != null)
            {
                raceDataLayout.BindingContext = Context.LatestRace;
            }

            if (Context.LatestResult != null)
            {
                personalResultLayout.BindingContext = Context.LatestResult;
            }
        }


    }
}