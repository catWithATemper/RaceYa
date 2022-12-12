using RaceYa.Helpers;
using RaceYa.Models;
using Xamarin.Forms;

namespace RaceYa
{
    public partial class App : Application
    {
        public static GlobalParameters Parameters = GlobalParameters.Instance();

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            Race NextRace = await FirestoreRace.ReadNextRace();
            Parameters.CurrentRace = NextRace;
            DataExchangeService.LoadRaceData(NextRace);

            //DataExchangeService.Instance().SyncData();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
