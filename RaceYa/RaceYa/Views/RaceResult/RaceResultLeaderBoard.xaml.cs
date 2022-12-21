using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultLeaderBoard : ContentPage
    {
        public static GlobalContext Context = GlobalContext.Instance();

        public RaceResultLeaderBoard()
        {
            InitializeComponent();

            if (Context.LatestRace != null)
            {
                leaderBoardView.ItemsSource = Context.LatestRace.ObservableFinalLeaderBoard;
            }
        }
    }
}