using System.Collections.Generic;

namespace RaceYa.Models
{
    class ParticipantComparer : IComparer<Participant>
    {
        public int Compare(Participant part1, Participant part2)
        {
            if (part1.Result.CurrentRaceTime.Value.CompareTo(part2.Result.CurrentRaceTime.Value) != 0)
            {
                //The result is negated, because a particpant with a larger covered distance should be higher in rank
                // in the leaderboard
                return - part1.Result.CurrentRaceTime.Value.CompareTo(part2.Result.CurrentRaceTime.Value);
            }
            else if (part1.Result.CurrentRaceTime.Key.CompareTo(part2.Result.CurrentRaceTime.Key) != 0)
            {
                return part1.Result.CurrentRaceTime.Key.CompareTo(part2.Result.CurrentRaceTime.Key);
            }
            else if (part1.User.Name.CompareTo(part2.User.Name) != 0)
            {
                return part1.User.Name.CompareTo(part2.User.Name);
            }
            else
            {
                return 0;
            }
        }
    }
}
