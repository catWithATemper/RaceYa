using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class RaceResult
    {

        public Participant Participant {get; set;}
        public Location CurrentLocation { get; set; }
        Location PreviousLocation { get; set; }

        Location StartingPoint;

        DateTime StartTime;
        public TimeSpan TimeSinceStart { get; set; }
        public double CoveredDistance { get; set; }
        public double AverageSpeed { get; set; }

        public Dictionary<TimeSpan, double> RaceTimes;

        public RaceResult(Participant participant)
        {
            Participant = participant;
            
            CoveredDistance = 0;
            AverageSpeed = 0;
            RaceTimes = new Dictionary<TimeSpan, double>();
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
            AverageSpeed = CoveredDistance / TimeSinceStart.TotalSeconds;
            return AverageSpeed;
        }
    }
}
