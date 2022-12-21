using RaceYa.Helpers;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace RaceYa.Models
{
    public class Participant
    {
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

        }

        public Participant(User user, Race race, string userId, string raceId)
        {
            User = user;
            Race = race;

            UserId = userId;
            RaceId = raceId;
        }

        public void AddToParticipantsList(Race race)
        {
            race.Participants.Add(this);
        }

        public void AssignRaceResult(RaceResult result)
        {
            Result = result;
            Result.ParticipantId = Id;
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
            race.LeaderBoard.Add(this);
        }
    }
}
