using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    class SpeedComparer : IComparer<Participant>
    {
        public int Compare(Participant part1, Participant part2)
        {
            if(part1.Result.AverageSpeed.CompareTo(part2.Result.AverageSpeed) != 0)
            {
                //The result is negated 
                return - part1.Result.AverageSpeed.CompareTo(part2.Result.AverageSpeed);
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
