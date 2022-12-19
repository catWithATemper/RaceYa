using RaceYa.Helpers;
using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RaceYa.Views.Races
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnteredRacesPage : ContentPage
    {
        public static GlobalContext Context = GlobalContext.Instance();
        public EnteredRacesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            racesListView.ItemsSource = null;
            var enteredRaces = await FirestoreRace.ReadRacesByUserId(Context.CurrentUser.Id);
            racesListView.ItemsSource = enteredRaces;

            foreach (Race race in enteredRaces)
            {
                Context.LoadRaceData(race);
            }
        }

        private async void runButton_Clicked(object sender, EventArgs e)
        {
            Context.CurrentRace = (Race)((Button)sender).CommandParameter;

            //Context.LoadRaceData(selectedRace);

            await Shell.Current.GoToAsync($"//NextRacePage");
        }
    }
}