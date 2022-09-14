using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class RaceResult
    {
        public Location CurrentLocation { get; set; }
        Location PreviousLocation { get; set; }

        Location StartingPoint;

        DateTime StartTime;

        TimeSpan TimeSinceStart;
        public double CoveredDistance { get; set; }
        public double AverageSpeed { get; set; }

        public Dictionary<TimeSpan, double> RaceTimes;

        public RaceResult()
        {
            CoveredDistance = 0;
            AverageSpeed = 0;
        }

        
        public void SetCurrentLocation(Location newReading)
        {
            PreviousLocation = CurrentLocation;
            CurrentLocation = newReading;
        }

        public void SetStartingPoint()
        {
            StartingPoint = CurrentLocation;
        }
        

        public void SetStartTime()
        {
            StartTime = StartingPoint.Timestamp.DateTime;
        }

        public void CalculateTimeSinceStart()
        {
            TimeSinceStart = CurrentLocation.Timestamp.DateTime - StartTime;
        }

        public double CalculateCoveredDistance()
        {
            CoveredDistance += Location.CalculateDistance(CurrentLocation, PreviousLocation, DistanceUnits.Kilometers) * 1000;

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
