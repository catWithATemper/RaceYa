namespace RaceYa.Models
{
    public class Participant
    {
        public User User { get; set; }
        public Race Race { get; set; }

        public RaceResult Result;

        public bool IsCurrentParticipant = false;

        public Participant(User user, Race race)
        {
            User = user;
            Race = race;
            Result = new RaceResult();
            Race.Participants.Add(this);
            Race.LeaderBoard.Add(this, 0);
        }
    }
}
