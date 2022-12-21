using RaceYa.Helpers;
using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultHistoryPage : ContentPage
    {
        public static GlobalContext Context = GlobalContext.Instance();
        public ResultHistoryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            resultsListView.ItemsSource = null;

            List<RaceResult> results = await FirestoreRaceResult.ReadAllResultsForUser(Context.CurrentUser.Id);

            resultsListView.ItemsSource = results;               
        }

        private async void detailsButton_Clicked(object sender, EventArgs e)
        {
            Context.LatestResult = (RaceResult)((Button)sender).CommandParameter;
            Context.LatestRace = await FirestoreRace.ReadRaceById(Context.LatestResult.RaceId);

            await Shell.Current.GoToAsync("//RaceResultTabbedPage");
        }
    }
}