using RaceYa.Helpers;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace RaceYa.Models
{
    public class Participant
    {
        //TODO: Call "AddToLeaderboard() before starting the race

        [Id]
        public string Id { get; set; }

        [MapTo("userId")]
        public string UserId { get; set; }

        [MapTo("raceId")]
        public string RaceId { get; set; }

        [Ignored]
        public User User { get; set; }

        [Ignored]
        public Race Race { get; set; }

        [Ignored]
        public RaceResult Result;

        [Ignored]
        public bool IsCurrentParticipant = false;


        public Participant()
        {
            /*
            Result = new RaceResult(this);
            Race.Participants.Add(this);
            Race.LeaderBoard.Add(this, 0);
            */
        }

        //Call empty constructor from this constructor
        public Participant(User user, Race race, string userId, string raceId)
        {
            User = user;
            Race = race;

            UserId = userId;
            RaceId = raceId;

            //Result = new RaceResult(this);
            //Race.Participants.Add(this);
            //Race.LeaderBoard.Add(this, 0);
        }

        public void AddToParticipantsList(Race race)
        {
            race.Participants.Add(this);
            //race.LeaderBoard.Add(this, 0);
        }

        public void AssignRaceResult(RaceResult result)
        {
            Result = result;
        }

        public void AssignRace(Race race)
        {
            Race = race;
        }

        public void AssignUser(User user)
        {
            User = user;
        }

        public void AddToRaceLeaderboard(Race race)
        {
            race.LeaderBoard.Add(this, 0);
        }
    }
}
