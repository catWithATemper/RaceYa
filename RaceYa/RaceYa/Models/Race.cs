using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class Race
    {
        public DateTime EndDate { get; set; }
        public float RouteLength { get; set; } //in km
        public List<Participant> Leaderboard { get; set; }

        public Race()
        {
            RouteLength = 5000;
            Leaderboard = new List<Participant>();
        }
    }
}
