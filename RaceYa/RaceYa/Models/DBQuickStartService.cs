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

        public bool UserIsAuthenticated = false;

        public static GlobalContext Context = GlobalContext.Instance();

        /*
        public Race CurrentRace;

        public User User1,
                    User2,
                    User3,
                    CurrentUser;

        public Participant Participant1,
                           Participant2,
                           Participant3,
                           CurrentParticipant;

        public RaceResult Result1,
                          Result2,
                          Result3,
                          CurrentParticipantResult;

        public RaceResultGPX CurrentParticipantResultGPX;
        */

        public bool dataCreated = false;

        public static DBQuickStartService Instance()
        {
            if (instance == null)
                instance = new DBQuickStartService();
            return instance;
        }

        public async Task LoadRaceData(Race race)
        {
            List<User> users = await FirestoreUser.Read();
            foreach (User user in users)
            {
                Participant participant = await FirestoreParticipant.ReadParticpantByUserAndRace(user.Id, race.Id);
                if (participant != null)
                {
                    participant.AssignRace(race);
                    participant.AssignUser(user);
                    RaceResult result = await FirestoreRaceResult.ReadRaceRaesultByParticipantId(participant.Id);
                    result.RaceParticipant = participant;
                    participant.Result = result;
                    participant.AddToParticipantsList(race);
                    participant.AddToRaceLeaderboard(race);

                    RaceResultGPX resultGPX = await FirestoreRaceResultGPX.ReadRaceResultGPXByParticipantAndResultIds(participant.Id, result.Id);
                }
            }
        }

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
                await FirestoreRaceResult.Add(result, participant.Id);

                if (participant.UserId != Context.CurrentUser.Id)
                {
                    sampleResults.Add(result);

                    PopulateRaceResultFromFile(result, FileNames[index], currentRace.RouteLength);
                    FirestoreRaceResult.Update(result, participant.Id);
                    index++;
                }
            }
            dataCreated = true;
        }

        /*
        public async void SyncData()
        {
            //await CreateCurrentUser();

            await LoadUsers();

            //await CreateRaces();
            await LoadRaces();
            
            //await CreateCurrentParticipant();

            //await CreateParticipants();
            
            await LoadParticipants();
            
            await CreateEmptyRaceResults();
            await CreateCurrentParticipantResult();

            //await SaveCurrentParticipantResult();

            //await LoadRaceResults();

            await AssignRaceResults();

            await AddToParticipantsList();

            await AddParticipantsToLeaderBoard();

            //await SetRaceCurrentParticipant();

            //await LoadResultGPX();
        }
        */
        /*
        private async Task CreateCurrentUser()
        {
            User currentUser = new User("Giulia", "FyvaJPbLpUVnrBgW4j51fO4NVuG3");
            FirestoreUser.Add(currentUser);
        }

        private async Task CreateCurrentParticipant()
        {
            CurrentParticipant = new Participant(CurrentUser, CurrentRace, CurrentUser.Id, CurrentRace.Id);
            await FirestoreParticipant.Add(CurrentParticipant);
        }

        private async Task CreateCurrentParticipantResult()
        {
            CurrentParticipantResult = new RaceResult(CurrentParticipant);
        }

        private async Task SaveCurrentParticipantResult()
        {
            await FirestoreRaceResult.Add(CurrentParticipantResult, CurrentParticipant.Id);
        }

        private async Task SetRaceCurrentParticipant()
        {
            CurrentRace.CurrentParticipant = CurrentParticipant;
            CurrentParticipant.IsCurrentParticipant = true;
            CurrentParticipantResult.GPXRequired = true;
        }

        private async Task LoadResultGPX()
        {
            CurrentParticipantResultGPX = await FirestoreRaceResultGPX.ReadRaceResultGPXByParticipantAndResultIds(CurrentParticipant.Id, CurrentParticipantResult.Id);       
        }

        private async Task LoadUsers()
        {
            User1 = await FirestoreUser.ReadUserById("1fg7XZGXXTdpzkOvxVVP");
            User2 = await FirestoreUser.ReadUserById("GWSGg18LUi0TG5j8WlT0");
            User3 = await FirestoreUser.ReadUserById("iCHsytUAsCgjkf935GkE");
            CurrentUser = await FirestoreUser.ReadUserById("gj34J8kMNbIouWftxhgM");
        }

        private async Task CreateRaces()
        {          
            CurrentRace = new Race(0.2, 
                                   DateTime.Parse("2022-12-11T00:00:00"),
                                   DateTime.Parse("2022-12-17T00:00:00"),
                                   "Test run");
            CurrentRace.UserId = "2MYcvjksqetYk1uCn4Eb";
            await FirestoreRace.Add(CurrentRace);
        }

        private async Task LoadRaces()
        {
            CurrentRace = await FirestoreRace.ReadRaceById("WRKHBMX79lN2a5nraAJQ");
        }

        private async Task CreateParticipants()
        {
            Participant1 = new Participant(User1, CurrentRace, User1.Id, CurrentRace.Id);
            await FirestoreParticipant.Add(Participant1);
            Participant2 = new Participant(User2, CurrentRace, User2.Id, CurrentRace.Id);
            await FirestoreParticipant.Add(Participant2);
            Participant3 = new Participant(User3, CurrentRace, User3.Id, CurrentRace.Id);
            await FirestoreParticipant.Add(Participant3);
        }

        //Also sets up the relationships with the participant's user and race
        private async Task LoadParticipants()
        {
            Participant1 = await FirestoreParticipant.ReadParticipantById("GXuURFnjNYCDQqmQLBsl");
            Participant2 = await FirestoreParticipant.ReadParticipantById("nL3gwvrBQaG1pqZzBMFw");
            Participant3 = await FirestoreParticipant.ReadParticipantById("sRuntZTUsGKyro6qaHnA");
            CurrentParticipant = await FirestoreParticipant.ReadParticipantById("nEv8ookNvMcqtfgJaM36");

            Participant1.AssignUser(User1);
            Participant2.AssignUser(User2);
            Participant3.AssignUser(User3);
            CurrentParticipant.AssignUser(CurrentUser);

            Participant1.AssignRace(CurrentRace);
            Participant2.AssignRace(CurrentRace);
            Participant3.AssignRace(CurrentRace);
            CurrentParticipant.AssignRace(CurrentRace);
        }

        private async Task CreateEmptyRaceResults()
        {
            Result1 = new RaceResult(Participant1);
            Result2 = new RaceResult(Participant2);
            Result3 = new RaceResult(Participant3);
        }

        //Run both when reading results from database and when creating new ones
        private async Task AddParticipantsToLeaderBoard()
        {
            Participant1.AddToRaceLeaderboard(CurrentRace);
            Participant2.AddToRaceLeaderboard(CurrentRace);
            Participant3.AddToRaceLeaderboard(CurrentRace);
            CurrentParticipant.AddToRaceLeaderboard(CurrentRace);
        }

        private async Task AssignRaceResults()
        {
            Participant1.AssignRaceResult(Result1);
            Participant2.AssignRaceResult(Result2);
            Participant3.AssignRaceResult(Result3);
            CurrentParticipant.AssignRaceResult(CurrentParticipantResult);
        }


        private async Task AddToParticipantsList()
        {
            Participant1.AddToParticipantsList(CurrentRace);
            Participant2.AddToParticipantsList(CurrentRace);
            Participant3.AddToParticipantsList(CurrentRace);
            CurrentParticipant.AddToParticipantsList(CurrentRace);
        }

        private async Task LoadRaceResults()
        {
            Result1 = await FirestoreRaceResult.ReadRaceRaesultByParticipantId("GXuURFnjNYCDQqmQLBsl");
            Result2 = await FirestoreRaceResult.ReadRaceRaesultByParticipantId("nL3gwvrBQaG1pqZzBMFw");
            Result3 = await FirestoreRaceResult.ReadRaceRaesultByParticipantId("sRuntZTUsGKyro6qaHnA");
            CurrentParticipantResult = await FirestoreRaceResult.ReadRaceRaesultByParticipantId("nEv8ookNvMcqtfgJaM36");

            Result1.RaceParticipant = Participant1;
            Result2.RaceParticipant = Participant2;
            Result3.RaceParticipant = Participant3;
            CurrentParticipantResult.RaceParticipant = CurrentParticipant; 
        }

        public async void PopulateRaceResultsFromFiles()
        {
            PopulateRaceResultFromFile(Result1, "RaceYa.DB.FASTactivity_8915103095.gpx");
            PopulateRaceResultFromFile(Result2, "RaceYa.DB.FASTactivity_8937870612.gpx");
            PopulateRaceResultFromFile(Result3, "RaceYa.DB.activity_9486210614.gpx");
            //PopulateRaceResultFromFile(participant4.Result, "RaceYa.DB.activity_9578996388.gpx");
            //PopulateRaceResultFromFile(participant5.Result, "RaceYa.DB.activity_9643381559.gpx");
            //PopulateRaceResultFromFile(participant6.Result, "RaceYa.DB.activity_9731960401.gpx");

            await FirestoreRaceResult.Add(Result1, "GXuURFnjNYCDQqmQLBsl");
            await FirestoreRaceResult.Add(Result2, "nL3gwvrBQaG1pqZzBMFw");
            await FirestoreRaceResult.Add(Result3, "sRuntZTUsGKyro6qaHnA");
        }
        */

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

        /*
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
        */
    }
}




