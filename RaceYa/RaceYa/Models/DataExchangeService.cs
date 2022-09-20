using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class DataExchangeService
    {
        private static DataExchangeService instance = null;

        public Race CurrentRace = new Race();
        public Dictionary<string, User> Users { get; } = new Dictionary<string, User>();
        public Dictionary<string, Participant> Participants { get; } = new Dictionary<string, Participant>();

        public DataExchangeService()
        {
            User User1 = new User("Amy");
            User User2 = new User("Bob");
            User User3 = new User("Lin");
            User User4 = new User("Sam");
            User User5 = new User("Tom");
            User User6 = new User("Zoe");

            Users.Add(User1.Name, User1);
            Users.Add(User2.Name, User2);
            Users.Add(User3.Name, User3);
            Users.Add(User4.Name, User4);
            Users.Add(User5.Name, User5);
            Users.Add(User6.Name, User6);

            Participant Participant1 = new Participant(User1, CurrentRace);
            Participant Participant2 = new Participant(User2, CurrentRace);
            Participant Participant3 = new Participant(User3, CurrentRace);
            Participant Participant4 = new Participant(User4, CurrentRace);
            Participant Participant5 = new Participant(User5, CurrentRace);
            Participant Participant6 = new Participant(User6, CurrentRace);

            Participant1.AddToLeaderboard();
            Participant2.AddToLeaderboard();
            Participant3.AddToLeaderboard();
            Participant4.AddToLeaderboard();
            Participant5.AddToLeaderboard();
            Participant6.AddToLeaderboard();

            RaceResult Result1 = new RaceResult(Participant1);
            RaceResult Result2 = new RaceResult(Participant2);
            RaceResult Result43 = new RaceResult(Participant3);
            RaceResult Result4 = new RaceResult(Participant4);
            RaceResult Result5 = new RaceResult(Participant5);
            RaceResult Result6 = new RaceResult(Participant6);
        }

        public static DataExchangeService Instance()
        {
            if (instance == null)
                instance = new DataExchangeService();
            return instance;
        }
    }
}




