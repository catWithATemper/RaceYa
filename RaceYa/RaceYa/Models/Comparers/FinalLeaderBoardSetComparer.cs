using System.Collections.Generic;

namespace RaceYa.Models
{
    class FinalLeaderBoardSetComparer : IComparer<Participant>
    {
        public int Compare(Participant part1, Participant part2)
        {
            //Participants who completed the race have all run the same distance. Non-finishers 
            //(i.e. those who ran a shorter distance) are added to the bottom of the leaderboard. 
            //The result is negated.
            if (part1.Result.EvaluatedDistanceInMeters.CompareTo(part2.Result.EvaluatedDistanceInMeters) != 0)
            {
                return - part1.Result.EvaluatedDistanceInMeters.CompareTo(part2.Result.EvaluatedDistanceInMeters);
            }
            else if(part1.Result.AverageSpeedKmH.CompareTo(part2.Result.AverageSpeedKmH) != 0)
            {
                //The result is negated because a particpant with a larger average speed should be higher in rank
                // in the leaderboard
                return - part1.Result.AverageSpeedKmH.CompareTo(part2.Result.AverageSpeedKmH);
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

        /*
        public int Compare(Participant part1, Participant part2)
        {
            if(part1.Result.AverageSpeed.CompareTo(part2.Result.AverageSpeed) != 0)
            {
                //The result is negated because a particpant with a larger average speed should be higher in rank
                // in the leaderboard
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
        */
    }
}
