using RaceYa.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class DBQuickStartService
    {
        public static DBQuickStartService instance = null;

        public static GlobalContext Context = GlobalContext.Instance();

        public bool dataCreated = false;

        public static DBQuickStartService Instance()
        {
            if (instance == null)
                instance = new DBQuickStartService();
            return instance;
        }

        //Populates the database with all necessary data for a race, with the exception of users
        public async Task CreateData()
        {
            List<RaceResult> sampleResults = new List<RaceResult>();

            List<string> FileNames = new List<string>{
                "RaceYa.DB.FASTactivity_8915103095.gpx",
                "RaceYa.DB.FASTactivity_8937870612.gpx",
                "RaceYa.DB.activity_9486210614.gpx"
            };

            Race currentRace = new Race(0.2,
                                   DateTime.Parse("2022-12-11T00:00:00"),
                                   DateTime.Parse("2022-12-17T00:00:00"),
                                   "Test run");
            currentRace.UserId = "gj34J8kMNbIouWftxhgM";
            await FirestoreRace.Add(currentRace);

            List<User> users = await FirestoreUser.Read();
            var index = 0;
            foreach (User user in users)
            {
                Participant participant = new Participant(user, currentRace, user.Id, currentRace.Id);
                await FirestoreParticipant.Add(participant);

                RaceResult result = new RaceResult(participant);
                result.GPXRequired = true;
                await FirestoreRaceResult.Add(result, participant.Id);
                RaceResultGPX resultGPX = new RaceResultGPX();
                await FirestoreRaceResultGPX.Add(resultGPX, participant.Id, result.Id);

                if (participant.UserId != Context.CurrentUser.Id)
                {
                    sampleResults.Add(result);

                    PopulateRaceResultFromFile(result, FileNames[index], currentRace.RouteLength);
                    FirestoreRaceResult.Update(result, participant.Id);

                    resultGPX.Track.TrackSegment = result.TrackSegment;
                    FirestoreRaceResultGPX.Update(resultGPX, participant.Id, result.Id);

                    result.GPXRequired = false;

                    index++;
                }
            }
            dataCreated = true;
        }

        public void PopulateRaceResultFromFile(RaceResult result, string fileName, double routeLength)
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
                        if (result.CoveredDistance <= routeLength)
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
                    if (result.CoveredDistance > routeLength)
                    {
                        result.RaceCompleted = true;
                        break;
                    }
                }
            }
        }
    }
}




