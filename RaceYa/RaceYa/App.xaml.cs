using RaceYa.Helpers;
using RaceYa.Models;
using Xamarin.Forms;

namespace RaceYa
{
    public partial class App : Application
    {
        public static GlobalContext Context = GlobalContext.Instance();
        public static DataExchangeService Service = DataExchangeService.Instance();

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            
            Race NextRace = await FirestoreRace.ReadNextRace();
            Context.CurrentRace = NextRace;
            Service.LoadRaceData(NextRace);
               

            //Service.SyncData();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
