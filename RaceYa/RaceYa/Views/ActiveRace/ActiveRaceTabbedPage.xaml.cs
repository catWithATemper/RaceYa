﻿using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;
using System;
using System.Globalization;
using System.Xml;
using RaceYa.Helpers;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveRaceTabbedPage : TabbedPage
    {
        //public static DBQuickStartService Service = DBQuickStartService.Instance();

        public static GlobalContext Context = GlobalContext.Instance();

        public static Participant CurrentParticipant;

        public static StopWatch PageStopWatch = new StopWatch();

        public static LocationServiceManager LocationService = new LocationServiceManager();

        public static TextToSpeechServiceManager TextToSpeechService;

        public ActiveRaceTabbedPage()
        {
            InitializeComponent();

            //CurrentParticipant = Service.CurrentRace.CurrentParticipant;
            TextToSpeechService = new TextToSpeechServiceManager(Context.CurrentParticipant.Result);

            MessagingCenter.Subscribe<ActiveRaceDataPage>(this, "Quit race", (sender) =>
            {
                Context.CurrentRace.CalculateFinalLeaderBoard();
                Context.CurrentRace.CalculateIdFinalLeaderBoard();
            });
            MessagingCenter.Subscribe<ActiveRaceLeaderboardPage>(this, "Quit race", (sender) =>
            {
                Context.CurrentRace.CalculateFinalLeaderBoard();
                Context.CurrentRace.CalculateIdFinalLeaderBoard();
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            PageStopWatch.SetTimer();

            TextToSpeechService.StartTextToSpeech();

            await CalculateRaceResult();
        }

        public async Task CalculateRaceResult()
        {
            Location currentLocation = await LocationService.GetCurrentLocation();

            Context.CurrentParticipant.Result.SetCurrentLocation(currentLocation);
            Context.CurrentParticipant.Result.SetStartingPoint();

            //debug
            Console.WriteLine("Start: Latitude " + currentLocation.Latitude + ", Longitude " + currentLocation.Longitude + 
                              ", Time " + currentLocation.Timestamp +
                              ", Accuracy " + currentLocation.Accuracy); 

            int counter = 0;
            while (Context.CurrentParticipant.Result.CoveredDistance <= Context.CurrentRace.RouteLength &&
                   Context.CurrentParticipant.Result.RaceCompleted == false)
            {
                await Task.Delay(100); //0.1 seconds
                counter++;
                
                //Check if the 1 second counter can be removed, since GetCurrentLocation() has its own
                //internal 4 seconds timeout.
                if (counter == 10)
                {
                    currentLocation = await LocationService.GetCurrentLocation();
                   Context.CurrentParticipant.Result.SetCurrentLocation(currentLocation);

                    //debug
                    Context.CurrentParticipant.Result.Accuracy = (double)currentLocation.Accuracy;
                    Context.CurrentParticipant.Result.GPSSpeed = (double)currentLocation.Speed;

                    Console.WriteLine("Current location: Latitude " + currentLocation.Latitude + ", Longitude " + currentLocation.Longitude +
                                      ", Time " + currentLocation.Timestamp + 
                                      ", Formatted time: " + DateTime.SpecifyKind(currentLocation.Timestamp.DateTime, DateTimeKind.Utc).ToString("o", CultureInfo.InvariantCulture));
                    Console.WriteLine("Distance " + Context.CurrentParticipant.Result.CoveredDistance +
                                      ", Avg speed " + Context.CurrentParticipant.Result.AverageSpeed + 
                                      ", GPS speed " + currentLocation.Speed +
                                      ", Accuracy " + currentLocation.Accuracy);

                    //Check whether the if condition is necessary
                    if (Context.CurrentParticipant.Result.CoveredDistance != 0)
                    {
                        Context.CurrentRace.UpdateLeaderBoard();
                    }
                    counter = 0;
                }
            }

            if (PageStopWatch != null)
            {
                PageStopWatch.StopTimer();
            }

            if (TextToSpeechService != null)
            {
                TextToSpeechService.StopTextToSpeech();
            }

            if (Context.CurrentParticipant.Result.CoveredDistance >= Context.CurrentRace.RouteLength)
            {
                await DisplayAlert("Race Complete!", "Tap \"OK\" to view your result.", "OK");
            }

            Context.CurrentParticipant.Result.RaceCompleted = true;

            Context.CurrentRace.CalculateFinalLeaderBoard();
            Context.CurrentRace.CalculateIdFinalLeaderBoard();

            await SaveUpdatedData();

            //reset binding context for landing page
            await Shell.Current.GoToAsync("//RaceResultTabbedPage");
            await Navigation.PopModalAsync();
        }
        
        public async Task SaveUpdatedData()
        {
            await FirestoreRace.Update(Context.CurrentRace);

            foreach (Participant participant in Context.CurrentRace.Participants)
            {
                if (participant.Id == Context.CurrentParticipant.Id)
                {
                    //Context.CurrentParticipant.Result.Id = "TTJIHhDGxNa8NUbZYr9W";

                    RaceResult existingRecord = await FirestoreRaceResult.ReadRaceRaesultByParticipantId(Context.CurrentParticipant.Id);
                    Context.CurrentParticipant.Result.Id = existingRecord.Id;

                    await FirestoreRaceResult.Update(Context.CurrentParticipant.Result, Context.CurrentParticipant.Id);
                    
                    RaceResultGPX resultGPX = new RaceResultGPX();
                    resultGPX.Track.TrackSegment = participant.Result.TrackSegment;

                    await FirestoreRaceResultGPX.Add(resultGPX, participant.Id, participant.Result.Id);
                }
                else
                {
                    await FirestoreRaceResult.Update(participant.Result, participant.Id);

                    RaceResultGPX resultGPX = new RaceResultGPX();
                    resultGPX.Track.TrackSegment = participant.Result.TrackSegment;

                    await FirestoreRaceResultGPX.Add(resultGPX, participant.Id, participant.Result.Id);
                }
            }
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (TextToSpeechService != null)
            {
                TextToSpeechService.StopTextToSpeech();
            }
        }
    }
}