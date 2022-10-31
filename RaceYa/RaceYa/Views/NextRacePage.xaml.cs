using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using Xamarin.Essentials;
using System.Threading;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NextRacePage : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant = new Participant(LoginPage.CurrentUser, Service.CurrentRace);

        public static LocationServiceManager LocationService = new LocationServiceManager();
        public NextRacePage()
        {
            InitializeComponent();
            Service.CurrentRace.CurrentParticipant = CurrentParticipant;

            CurrentParticipant.IsCurrentParticipant = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            distanceLabel.Text = (Service.CurrentRace.RouteLength / 1000) + " km";
            endDateLabel.Text = Service.CurrentRace.EndDate.ToString();
        }

        private async void startButton_Clicked(object sender, EventArgs e)
        {
            Location locationTest;

            startButton.IsEnabled = false;
            searchingForGPSLabel.Text = "Searching for GPS... ";

            try
            {
                locationTest = await LocationService.GetCurrentLocation();

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
                        searchingForGPSLabel.Text = "";
                        for (int times = 5; times > 0; times--)
                        {
                            countDownLabel.Text = times.ToString();
                            await Task.Delay(1000);
                        }
                        await Navigation.PushModalAsync(new RaceTabbedPage());
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
    }
}