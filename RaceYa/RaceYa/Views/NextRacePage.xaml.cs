﻿using System;
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
        /*
         * TODO: Check again for location availability at regular intervals
         * 
         */

        public static DataExchangeService Service = DataExchangeService.Instance();

        public static Participant CurrentParticipant = Service.CurrentRace.CurrentParticipant;

        public static LocationServiceManager LocationService = new LocationServiceManager();

        public NextRacePage()
        {
            InitializeComponent();

            nextRaceStackLayout.BindingContext = Service.CurrentRace;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            startButton.IsEnabled = true;
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
    }
}