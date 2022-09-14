using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class Participant
    {
        public User user { get; set; }
        public Race race { get; set; }

        public RaceResult Result;

        public Participant()
        {
            //Result.RouteLength = CurrentRace.RouteLength;
        }
    }
}
