using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using Xamarin.Essentials;
using System.Threading;
using RaceYa.Helpers;
using System.Collections.Generic;
using System.Web;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    //[QueryProperty(nameof(NextRaceId), "raceId")]
    public partial class NextRacePage : ContentPage
    {
        /*
         * TODO: Check again for location availability at regular intervals
         */

        public static GlobalContext Context = GlobalContext.Instance();

        public static LocationServiceManager LocationService = new LocationServiceManager();

        public NextRacePage()
        {
            InitializeComponent();
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            startButton.IsEnabled = true;

            nextRaceStackLayout.BindingContext = Context.CurrentRace;

            if (Context.CurrentRace != null)
            {
                Context.SetUpNextParticipantContext();
            }

            Context.CurrentRace.FinalLeaderBoardSetCalculated = false;
            Context.CurrentRace.FinalLeaderBoardCalculated = false;
        }

        private async void startButton_Clicked(object sender, EventArgs e)
        {
            Location locationTest;

            startButton.IsEnabled = false;
            searchingForGPSLabel.Text = "Searching for GPS... ";

            try
            {
                locationTest = await LocationService.GetTestLocation();

                if (locationTest != null)
                {
                    bool answer = await DisplayAlert("Start race?", "Tap \"OK\" to start the countdown", "OK", "Cancel");
                    //TODO Check again for location availability at regular intervals here. 
                    if (!answer)
                    {
                        startButton.IsEnabled = true;
                        searchingForGPSLabel.Text = "";
                        return;
                    }
                    else
                    {
                        startButton.IsEnabled = false; //otherwise gets reenabled when the app asks for permissions
                        searchingForGPSLabel.Text = "";
                        for (int times = 5; times > 0; times--)
                        {
                            countDownLabel.Text = times.ToString();
                            await Task.Delay(1000);
                        }
                        await Navigation.PushModalAsync(new ActiveRaceTabbedPage());
                        countDownLabel.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + ex.Message + ex.StackTrace + ex.InnerException);
                await DisplayAlert("Exception", "Unable to get location", "OK");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            startButton.IsEnabled = true;
            searchingForGPSLabel.Text = "";
            Shell.Current.GoToAsync($"//MainPage");

            return false;
        }
    }
}