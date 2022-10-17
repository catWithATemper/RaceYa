using System;
using System.Collections.Generic;
using System.Linq;

namespace RaceYa.Models
{
    public class Race
    {
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public float RouteLength { get; set; } //in meters
        public Participant CurrentParticipant { get; set; }
        public static List<Participant> Participants { get; set; }

        //Each pair contains a participant and the distance covered by the participant at the current time
        public static SortedDictionary<Participant, double> LeaerBoard {get; set;}

        //Each pair contains a participant and their average speed
        public static SortedDictionary<Participant, double> FinalLeaerBoard { get; set; }

        public Race()
        {
            //Hardcoded values
            RouteLength = 5000;
            EndDate = DateTime.Parse("October 30, 2022");

            Participants = new List<Participant>();
            LeaerBoard = new SortedDictionary<Participant, double>(new ParticipantComparer());
        }

        //TODO: Add method to adjust for any extra distance run and save a final leaderboard
        //based on average speed

        public void UpdateLeaderBoard()
        {
            foreach (Participant participant in Participants)
            {
                if (participant.Result.RaceTimes != null)
                {
                    for (int index = participant.Result.RaceTimeIndex; index < participant.Result.RaceTimes.Count; index++)
                    {
                        if (participant.Result.RaceTimes.ElementAt(index).Key > CurrentParticipant.Result.TimeSinceStart)
                        {
                            //Add logic for adjusting to current participant's current time
                            participant.Result.CurrentRaceTime = participant.Result.RaceTimes.ElementAt(index - 1);
                            participant.Result.RaceTimeIndex = index - 1;

                            LeaerBoard[participant] = participant.Result.CurrentRaceTime.Value;
                        }
                    }
                }
            }
        }
    }
}
