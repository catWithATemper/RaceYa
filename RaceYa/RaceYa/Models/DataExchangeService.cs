using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class DataExchangeService
    {
        private static DataExchangeService instance = null;

        public Race CurrentRace = new Race();

        public DataExchangeService()
        {
            
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

                //Then read the whole file and calculate distance and speed for each location reading
                foreach (XElement trkseg in trk.Elements())
                {
                    foreach (XElement trkpt in trkseg.Elements())
                    {
                        double lat = (double)trkpt.Attribute("lat");
                        double lon = (double)trkpt.Attribute("lon");

                        DateTime Time = DateTime.Parse(trkpt.Element(ns + "time").Value);
                        Location currentLocation = new Location(lat, lon, Time);

                        result.SetCurrentLocation(currentLocation);
                        result.SetStartingPoint();
                        result.CoveredDistance = result.CalculateCoveredDistance();
                        result.CalculateTimeSinceStart();
                        if (result.CoveredDistance != 0)
                        {
                            result.AverageSpeed = result.CalculateAverageSpeed();
                        }
                    }
                }
            }
        }

        public static DataExchangeService Instance()
        {
            if (instance == null)
                instance = new DataExchangeService();
            return instance;
        }

        public void SyncData()
        {
            User user1 = new User("Amy");
            User User2 = new User("Bob");
            User User3 = new User("Lin");
            User User4 = new User("Sam");
            User User5 = new User("Tom");
            User User6 = new User("Zoe");

            Participant participant1 = new Participant(user1, CurrentRace);
            Participant participant2 = new Participant(User2, CurrentRace);
            Participant participant3 = new Participant(User3, CurrentRace);
            Participant participant4 = new Participant(User4, CurrentRace);
            Participant participant5 = new Participant(User5, CurrentRace);
            Participant participant6 = new Participant(User6, CurrentRace);

            /*
            RaceResult result1 = new RaceResult(participant1);
            RaceResult result2 = new RaceResult(participant2);
            RaceResult result3 = new RaceResult(participant3);
            RaceResult result4 = new RaceResult(participant4);
            RaceResult result5 = new RaceResult(participant5);
            RaceResult result6 = new RaceResult(participant6);
            */

            PopulateRaceResultFromFile(participant1.Result, "RaceYa.DB.20190829_181304.gpx");
            PopulateRaceResultFromFile(participant2.Result, "RaceYa.DB.20200601_174558.gpx");
            PopulateRaceResultFromFile(participant3.Result, "RaceYa.DB.20201009_175328.gpx");
            PopulateRaceResultFromFile(participant4.Result, "RaceYa.DB.20210526_173957.gpx");
            PopulateRaceResultFromFile(participant5.Result, "RaceYa.DB.20210530_143238.gpx");
            PopulateRaceResultFromFile(participant6.Result, "RaceYa.DB.20210602_183136.gpx");
        }
    }
}




