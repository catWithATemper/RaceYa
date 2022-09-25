using System.Collections.Generic;

namespace RaceYa.Models
{
    class ParticipantComparer : IComparer<Participant>
    {
        public int Compare(Participant part1, Participant part2)
        {
            if (part1.Result.NewestRaceTime.Value.CompareTo(part2.Result.NewestRaceTime.Value) != 0)
            {
                return - part1.Result.NewestRaceTime.Value.CompareTo(part2.Result.NewestRaceTime.Value);
            }
            else if (part1.Result.NewestRaceTime.Key.CompareTo(part2.Result.NewestRaceTime.Key) != 0)
            {
                return part1.Result.NewestRaceTime.Key.CompareTo(part2.Result.NewestRaceTime.Key);
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
