using RaceYa.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class DataExchangeService
    {
        public static DataExchangeService instance = null;

        public bool UserIsAuthenticated = false;

        public Race CurrentRace;

        /*
        public Race CurrentRace = new Race(0.2, 
                                           DateTime.Parse("2022-12-11T00:00:00"),
                                           DateTime.Parse("2022-12-17T00:00:00"),
                                           "Test run");
        */

        RaceResult Result1;
        RaceResult Result2;
        RaceResult Result3;

        public static DataExchangeService Instance()
        {
            if (instance == null)
                instance = new DataExchangeService();
            return instance;
        }

        public async void SyncData()
        {
            //string raceId = await FirestoreRace.Add(CurrentRace);
            CurrentRace = await FirestoreRace.ReadRaceById("raULrKmSdQMeLYeiLwjr");

            User user1 = await FirestoreUser.ReadUserById("1fg7XZGXXTdpzkOvxVVP");
            User user2 = await FirestoreUser.ReadUserById("GWSGg18LUi0TG5j8WlT0");
            User user3 = await FirestoreUser.ReadUserById("iCHsytUAsCgjkf935GkE");;

            Participant participant1 = await FirestoreParticipant.ReadParticipantById("AAUM0tuVFzywkArMoOsl");
            Participant participant2 = await FirestoreParticipant.ReadParticipantById("Gy3YGoVnqZr3ggCB0WzB");
            Participant participant3 = await FirestoreParticipant.ReadParticipantById("rgWjjYeq6jHmCz9vee9m");
  
            /*
            Participant participant1 = new Participant(user1, CurrentRace, user1.Id, CurrentRace.Id);
            await FirestoreParticipant.Add(participant1);
            Participant participant2 = new Participant(user1, CurrentRace, user2.Id, CurrentRace.Id);
            await FirestoreParticipant.Add(participant2);
            Participant participant3 = new Participant(user3, CurrentRace, user3.Id, CurrentRace.Id);
            await FirestoreParticipant .Add(participant3);
            */
                     
            participant1.AssignUser(user1);
            participant2.AssignUser(user2);
            participant3.AssignUser(user3);

            participant1.AssignRace(CurrentRace);
            participant2.AssignRace(CurrentRace);
            participant3.AssignRace(CurrentRace);

            //TODO
            //Result1 = await FirestoreRaceResult.ReadResultByParticipantId();


            Result1 = new RaceResult(participant1);
            Result2 = new RaceResult(participant2);
            Result3 = new RaceResult(participant3);

            participant1.AssignRaceResult(Result1);
            participant2.AssignRaceResult(Result2);
            participant3.AssignRaceResult(Result3);

            participant1.AddToParticipantsList(CurrentRace);
            participant2.AddToParticipantsList(CurrentRace);
            participant3.AddToParticipantsList(CurrentRace);

            participant1.AddToRaceLeaderboard(CurrentRace);
            participant2.AddToRaceLeaderboard(CurrentRace);
            participant3.AddToRaceLeaderboard(CurrentRace);
        }

        public async void PopulateRaceResultsFromFiles()
        {
            PopulateRaceResultFromFile(Result1, "RaceYa.DB.FASTactivity_8915103095.gpx");
            PopulateRaceResultFromFile(Result2, "RaceYa.DB.FASTactivity_8937870612.gpx");
            PopulateRaceResultFromFile(Result3, "RaceYa.DB.activity_9486210614.gpx");
            //PopulateRaceResultFromFile(participant4.Result, "RaceYa.DB.activity_9578996388.gpx");
            //PopulateRaceResultFromFile(participant5.Result, "RaceYa.DB.activity_9643381559.gpx");
            //PopulateRaceResultFromFile(participant6.Result, "RaceYa.DB.activity_9731960401.gpx");

            await FirestoreRaceResult.Add(Result1, "AAUM0tuVFzywkArMoOsl");
            await FirestoreRaceResult.Add(Result2, "Gy3YGoVnqZr3ggCB0WzB");
            await FirestoreRaceResult.Add(Result3, "rgWjjYeq6jHmCz9vee9m");
        }

        public void PopulateRaceResultFromFile(RaceResult result, string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            {
                XDocument gpxFile = XDocument.Load(stream);
                XNamespace ns = "http://www.topografix.com/GPX/1/1";
                XElement gpx = gpxFile.Element(ns + "gpx");
                XElement trk = gpx.Element(ns + "trk");

                //Set initial location
                XElement firstTrkseg = trk.Element(ns + "trkseg");
                XElement firstTrkpt = firstTrkseg.Element(ns + "trkpt");
                double initialLat = (double)firstTrkpt.Attribute("lat");
                double initialLon = (double)firstTrkpt.Attribute("lon");

                DateTime startTime = DateTime.Parse(firstTrkpt.Element(ns + "time").Value);
                Location initialLocation = new Location(initialLat, initialLon, startTime);

                result.SetCurrentLocation(initialLocation);
                result.SetStartingPoint();

                //Then read through the file and calculate distance and speed for each location reading
                //until completing the race route length
                foreach (XElement trkseg in trk.Elements())
                {
                    foreach (XElement trkpt in trkseg.Elements())
                    {
                        if (result.CoveredDistance <= CurrentRace.RouteLength)
                        {
                            double lat = (double)trkpt.Attribute("lat");
                            double lon = (double)trkpt.Attribute("lon");

                            DateTime Time = DateTime.Parse(trkpt.Element(ns + "time").Value);
                            Location currentLocation = new Location(lat, lon, Time);

                            result.SetCurrentLocation(currentLocation);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (result.CoveredDistance > CurrentRace.RouteLength)
                    {
                        result.RaceCompleted = true;
                        break;
                    }
                }
            }
        }
    }
}




