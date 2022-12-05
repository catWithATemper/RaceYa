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

  
        public Participant(User user, Race race, string userId, string raceId)
        {
            User = user;
            Race = race;

            UserId = userId;
            RaceId = raceId;

            FirestoreParticipant.Add(this);

            Result = new RaceResult(this);
            Race.Participants.Add(this);
            Race.LeaderBoard.Add(this, 0);
        }
    }
}
