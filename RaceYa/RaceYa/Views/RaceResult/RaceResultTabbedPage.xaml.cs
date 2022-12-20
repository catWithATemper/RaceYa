using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using RaceYa.Helpers;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultTabbedPage : TabbedPage
    {
        public static GlobalContext Context = GlobalContext.Instance();

        public RaceResultTabbedPage()
        {
            InitializeComponent();
        }
        
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            Context.LatestResult = await FirestoreRaceResult.ReadLatestRaceResult(Context.CurrentUser.Id);
        }
    }
}