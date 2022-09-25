using System;
using System.Collections.Generic;
using System.Linq;

namespace RaceYa.Models
{
    public class Race
    {
        public DateTime EndDate { get; set; }
        public float RouteLength { get; set; } //in km
        public Participant CurrentParticipant { get; set; }
        public static List<Participant> Participants { get; set; }
        public static SortedDictionary<Participant, double> LeaerBoard {get; set;}

        public Race()
        {
            RouteLength = 5000;
            Participants = new List<Participant>();
            LeaerBoard = new SortedDictionary<Participant, double>(new ParticipantComparer());
        }

        public void UpdateLeaderBoard()
        {
            foreach (Participant participant in Participants)
            {
                if (!(participant.Result.RaceTimes == null))
                {
                    for (int index = participant.Result.RaceTimeIndex; index < participant.Result.RaceTimes.Count; index++)
                    {
                        if (participant.Result.RaceTimes.ElementAt(index).Key > CurrentParticipant.Result.TimeSinceStart)
                        {
                            participant.Result.NewestRaceTime = participant.Result.RaceTimes.ElementAt(index - 1);
                            participant.Result.RaceTimeIndex = index - 1;

                            LeaerBoard.Remove(participant);
                            LeaerBoard.Add(participant, participant.Result.NewestRaceTime.Value);
                            //LeaerBoard[participant] = participant.Result.NewestRaceTime.Value;
                        }
                    }
                }
            }
        }
    }
}
