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

        public Race CurrentRace = new Race(0.2, 
                                           DateTime.Parse("2022-12-11T00:00:00"),
                                           DateTime.Parse("2022-12-31T00:00:00"),
                                           "Test run");

        public static DataExchangeService Instance()
        {
            if (instance == null)
                instance = new DataExchangeService();
            return instance;
        }

        public async void SyncData()
        {
            //string raceId = await FirestoreRace.Add(CurrentRace);

            User user1 = await FirestoreUser.ReadUserById("1fg7XZGXXTdpzkOvxVVP");
            User user2 = await FirestoreUser.ReadUserById("GWSGg18LUi0TG5j8WlT0");
            User user3 = await FirestoreUser.ReadUserById("iCHsytUAsCgjkf935GkE");
            //User user1 = new User("Alice", "0CjDbthpFmSsIzQfUPnMtGWdZSm1");
            //User User2 = new User("Bob", "QKYX5PVw7LWo3RWBMMglhKlNttX2");
            //User User3 = new User("Runner101", "cKNoka7HtXSS5973vEyW2QrQbnD3");
            //User User4 = new User("Lin");
            //User User5 = new User("Greyhound");
            //User User6 = new User("Tom");

            Participant participant1 = await FirestoreParticipant.ReadParticipantById("2CPqfFJS2Rh2XyYoNxAr");
            Participant participant2= await FirestoreParticipant.ReadParticipantById("W2NuVhfqOV3Ao8kFxBYk");
            Participant participant3 = await FirestoreParticipant.ReadParticipantById("dB6FoyzDPMhW6cULUZT4");

            RaceResult result1 = new RaceResult(participant1);
            RaceResult result2 = new RaceResult(participant2);
            RaceResult result3 = new RaceResult(participant3);

            participant1.AssignRaceResult(result1);
            participant2.AssignRaceResult(result2);
            participant3.AssignRaceResult(result3);

            participant1.AddToParticipantsList(CurrentRace);
            participant2.AddToParticipantsList(CurrentRace);
            participant3.AddToRaceLeaderboard(CurrentRace);
            
            //Participant participant1 = new Participant(user1, CurrentRace, "0CjDbthpFmSsIzQfUPnMtGWdZSm1", raceId);
            //Participant participant2 = new Participant(User2, CurrentRace, "QKYX5PVw7LWo3RWBMMglhKlNttX2", raceId);
            //Participant participant3 = new Participant(User3, CurrentRace, "cKNoka7HtXSS5973vEyW2QrQbnD3", raceId);
            //Participant participant4 = new Participant(User4, CurrentRace);
            //Participant participant5 = new Participant(User5, CurrentRace);
            //Participant participant6 = new Participant(User6, CurrentRace);

            //PopulateRaceResultFromFile(result1, "RaceYa.DB.FASTactivity_8915103095.gpx");
            //PopulateRaceResultFromFile(result2, "RaceYa.DB.FASTactivity_8937870612.gpx");
            //PopulateRaceResultFromFile(result3, "RaceYa.DB.activity_9486210614.gpx");
            //PopulateRaceResultFromFile(participant4.Result, "RaceYa.DB.activity_9578996388.gpx");
            //PopulateRaceResultFromFile(participant5.Result, "RaceYa.DB.activity_9643381559.gpx");
            //PopulateRaceResultFromFile(participant6.Result, "RaceYa.DB.activity_9731960401.gpx");
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




