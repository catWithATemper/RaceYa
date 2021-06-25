﻿using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using System.Threading;
using Xamarin.Essentials;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultPage : ContentPage
    {

        RaceResult result = new RaceResult();

        CancellationTokenSource cts;

        public RaceResultPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = result;
            distanceLabel.Text = "0";
            avgSpeedLabel.Text = "0";
            latitudeLabel.Text = "";
            longitudeLabel.Text = "";
        }

         async void OnButtonClicked(object sender, EventArgs e)
         {
            Location locationTest;

            StartButton.IsEnabled = false;
            StartButton.Text = "Searching for GPS... ";
            StartButton.TextColor = Color.Red;

            try
            {
                locationTest = await GetCurrentLocation();

                if (locationTest != null)
                {
                    bool answer = await DisplayAlert("Start race?", "Tap \"OK\" to start the countdown", "OK", "Cancel");

                    if (!answer)
                    {
                        StartButton.IsEnabled = true;
                        StartButton.Text = "START";
                        StartButton.TextColor = Color.White ;
                        return;
                    }
                    else
                    {
                        StartButton.TextColor = Color.Black;
                        for (int times = 5; times > 0; times--)
                        {
                            StartButton.Text = times.ToString();
                            await Task.Delay(1000);
                        }

                        StartButton.Text = "Run!";

                        //Debug
                        result.SetCurrentLocation(locationTest);
                        latitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Latitude.ToString("F8"));
                        longitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Longitude.ToString("F8"));

                        //The gist
                        await CalculateRaceResult();

                        /*
                        distanceLabel.SetValue(Label.TextProperty, result.Distance.ToString("F0"));
                        avgSpeedLabel.SetValue(Label.TextProperty, result.AverageSpeed.ToString("F2"));
                        latitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Latitude.ToString("F8"));
                        longitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Longitude.ToString("F8"));
                        */

               
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", "Unable to get location", "OK");
                StartButton.IsEnabled = true;
                StartButton.Text = "START";
            }
        }

        public async Task CalculateRaceResult()
        {
            Location currentLocation = await GetCurrentLocation();
            
            result.SetCurrentLocation(currentLocation);
            result.SetStartingPoint();
            result.SetStartTime();

            while (result.Distance <= result.RouteLength)
            {
                await Task.Delay(1000);
                currentLocation = await GetCurrentLocation();
                result.SetCurrentLocation(currentLocation);

                latitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Latitude.ToString("f8"));
                longitudeLabel.SetValue(Label.TextProperty, result.CurrentLocation.Longitude.ToString("F8"));

                result.Distance = result.CalculateDistance();

                distanceLabel.SetValue(Label.TextProperty, result.Distance.ToString("F0"));

                result.SetTimeSinceStart();

                if (result.Distance != 0)
                {
                    result.AverageSpeed = result.CalculateAverageSpeed();

                    //Debug
                    Console.WriteLine("Debug Speed: " + result.CalculateAverageSpeed());

                    avgSpeedLabel.SetValue(Label.TextProperty, result.AverageSpeed.ToString("F2"));
                }
            }
        }

        public async Task<Location> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();

            Location location = null;
            while (location == null)
            {
                location = await Geolocation.GetLocationAsync(request, cts.Token);
            }
            return location;
        }



    }

}