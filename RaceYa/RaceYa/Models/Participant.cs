using RaceYa.Helpers;

namespace RaceYa.Models
{
    public class Participant
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RaceId { get; set; }

        public User User { get; set; }
        public Race Race { get; set; }

        public RaceResult Result;

        public bool IsCurrentParticipant = false;

  
        public Participant(User user, Race race, string userId, string raceId)
        {
            User = user;
            Race = race;

            UserId = userId;
            RaceId = raceId;

            FirestoreParticipant.Insert(this);

            Result = new RaceResult(this);
            Race.Participants.Add(this);
            Race.LeaderBoard.Add(this, 0);
        }
    }
}
