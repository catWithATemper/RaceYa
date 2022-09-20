using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class Participant
    {
        public User User { get; set; }
        public Race Race { get; set; }

        public RaceResult Result;

        public Participant(User user, Race race)
        {
            User = user;
            Race = race;
            
            //Result = new RaceResult();
        }

        public void AddToLeaderboard()
        {
            Race.Leaderboard.Add(this);
        }
    }
}
