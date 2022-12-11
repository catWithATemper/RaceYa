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
    public class DataExchangeService
    {
        public static DataExchangeService instance = null;

        public bool UserIsAuthenticated = false;

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

        public static DataExchangeService Instance()
        {
            if (instance == null)
                instance = new DataExchangeService();
            return instance;
        }

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

            await LoadRaceResults();

            await AssignRaceResults();

            await AddToParticipantsList();

            await AddParticipantsToLeaderBoard();

            await SetRaceCurrentParticipant();

        }

        private async Task CreateCurrentUser()
        {
            User currentUser = new User("Giulia", "FyvaJPbLpUVnrBgW4j51fO4NVuG3");
            FirestoreUser.Add(currentUser);
        }

        private async Task CreateCurrentParticipant()
        {
            CurrentParticipant = new Participant(CurrentUser, CurrentRace, CurrentUser.Id, CurrentRace.Id);
            await FirestoreParticipant.Add(CurrentParticipant);

            //CurrentParticipantResult = new RaceResult(CurrentParticipant);
            //await FirestoreRaceResult.Add(CurrentParticipantResult, CurrentParticipant.Id);
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
            CurrentRace = await FirestoreRace.ReadRaceById("GMr4O2hnqLKk3GcgRVWG");
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
            Participant1 = await FirestoreParticipant.ReadParticipantById("VAIvehEKEVyqiJDXS3CB");
            Participant2 = await FirestoreParticipant.ReadParticipantById("cTCkOqOLuQxTgiPW1i9Z");
            Participant3 = await FirestoreParticipant.ReadParticipantById("nmNSpT54ekMYpthufTX6");
            CurrentParticipant = await FirestoreParticipant.ReadParticipantById("z7CKjFcD1Sbb6bR8IJl0");

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
            /*
            Participant1.AssignRaceResult(Result1);
            Participant2.AssignRaceResult(Result2);
            Participant3.AssignRaceResult(Result3);
            CurrentParticipant.AssignRaceResult(CurrentParticipantResult);
            */

            /*
            Participant1.AddToParticipantsList(CurrentRace);
            Participant2.AddToParticipantsList(CurrentRace);
            Participant3.AddToParticipantsList(CurrentRace);
            CurrentParticipant.AddToParticipantsList(CurrentRace);
            */

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
            Result1 = await FirestoreRaceResult.ReadRaceRaesultByParticipantId("VAIvehEKEVyqiJDXS3CB");
            Result2 = await FirestoreRaceResult.ReadRaceRaesultByParticipantId("cTCkOqOLuQxTgiPW1i9Z");
            Result3 = await FirestoreRaceResult.ReadRaceRaesultByParticipantId("nmNSpT54ekMYpthufTX6");
            CurrentParticipantResult = await FirestoreRaceResult.ReadRaceRaesultByParticipantId("z7CKjFcD1Sbb6bR8IJl0");

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

            await FirestoreRaceResult.Add(Result1, "VAIvehEKEVyqiJDXS3CB");
            await FirestoreRaceResult.Add(Result2, "cTCkOqOLuQxTgiPW1i9Z");
            await FirestoreRaceResult.Add(Result3, "nmNSpT54ekMYpthufTX6");
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




