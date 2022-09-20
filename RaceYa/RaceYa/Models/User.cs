using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class User
    {
        public string Name { get; set; }

        public User(string name)
        {
            Name = name;
        }

        /*
        public bool JoinRace(Race race)
        {
            bool joined = false;

            if (!race.Leaderboard.Exists(p => p.User.Name == Name))
            {
                Participant participant = new Participant(this, race);
                joined = true;
            }
            else
            {
                joined = true;
            }
            return joined;
        }
        */
    }
}
