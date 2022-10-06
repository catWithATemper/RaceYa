using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class RaceResult
    {
        //public Participant Participant {get; set;}
        public Location CurrentLocation { get; set; }
        Location PreviousLocation { get; set; }

        Location StartingPoint;

        DateTime StartTime;
        public TimeSpan TimeSinceStart { get; set; }
        public double CoveredDistance { get; set; }
        public double AverageSpeed { get; set; }

        public SortedDictionary<TimeSpan, double> RaceTimes;

        public KeyValuePair<TimeSpan, double> CurrentRaceTime;
        public int RaceTimeIndex { get; set; }

        public RaceResult()
        {
            //Participant = participant;
            //Participant.Result = this;
            
            CoveredDistance = 0;
            AverageSpeed = 0;
            RaceTimes = new SortedDictionary<TimeSpan, double>();
            CurrentRaceTime = new KeyValuePair<TimeSpan, double>(new TimeSpan(0), 0);
            RaceTimeIndex = 0;
        }

        public void SetCurrentLocation(Location newReading)
        {
            PreviousLocation = CurrentLocation;
            CurrentLocation = newReading;
        }

        public void SetStartingPoint()
        {
            StartingPoint = CurrentLocation;
            StartTime = StartingPoint.Timestamp.DateTime;
        }

        public void CalculateTimeSinceStart()
        {
            TimeSinceStart = CurrentLocation.Timestamp.DateTime - StartTime;
        }

        public double CalculateCoveredDistance()
        {
            CoveredDistance += Location.CalculateDistance(CurrentLocation, PreviousLocation, DistanceUnits.Kilometers) * 1000;

            if (!RaceTimes.ContainsKey(TimeSinceStart))

                RaceTimes.Add(TimeSinceStart, CoveredDistance);

            return CoveredDistance;
        }

        public double CalculateAverageSpeed()
        {
            if (TimeSinceStart.TotalSeconds != 0)
            {
                AverageSpeed = CoveredDistance / TimeSinceStart.TotalSeconds;
                return AverageSpeed;
            }
            else
            {
                return 0;
            }
        }
    }
}
