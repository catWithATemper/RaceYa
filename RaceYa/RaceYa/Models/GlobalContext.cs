using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class GlobalContext
    {
        public static GlobalContext instance = null;
        public static GlobalContext Instance()
        {
            if (instance == null)
                instance = new GlobalContext();
            return instance;
        }

        public bool UserIsAuthenticated = false;

        public Race CurrentRace { get; set; }

        public User CurrentUser { get; set; }

        public Participant CurrentParticipant { get; set; }
        public RaceResult CurrentParticipantResult { get; set; }
    }
}
