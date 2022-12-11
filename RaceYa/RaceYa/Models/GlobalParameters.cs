using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class GlobalParameters
    {
        public static GlobalParameters instance = null;
        public static GlobalParameters Instance()
        {
            if (instance == null)
                instance = new GlobalParameters();
            return instance;
        }

        public Race NextRace { get; set; }

        public User CurrentUser { get; set; }

        public Participant CurrentParticipant { get; set; }
        public RaceResult CurrentParticipantResult { get; set; }
    }
}
