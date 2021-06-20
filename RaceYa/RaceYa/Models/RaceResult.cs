using System;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    class RaceResult
    {
        public int RouteLength { get; }
        public Location CurrentLocation { get; set; }
        Location PreviousLocation { get; set; }

        Location startingPoint;

        DateTime startTime;

        TimeSpan timeSinceStart;
        public double Distance { get; set; }
        public double AverageSpeed { get; set; }

        public RaceResult()
        {
            RouteLength = 100;
            Distance = 0;
            AverageSpeed = 0;
        }

        
        public void SetCurrentLocation(Location newReading)
        {
            PreviousLocation = CurrentLocation;
            CurrentLocation = newReading;
        }

        public void SetStartingPoint()
        {
            startingPoint = CurrentLocation;
        }
        

        public void SetStartTime()
        {
            startTime = startingPoint.Timestamp.DateTime;
        }

        public void SetTimeSinceStart()
        {
            timeSinceStart = CurrentLocation.Timestamp.DateTime - startTime;
        }

        public double CalculateDistance()
        {
            Distance += Location.CalculateDistance(CurrentLocation, PreviousLocation, DistanceUnits.Kilometers) * 1000;
            return Distance;
        }

        public double CalculateAverageSpeed()
        {
            AverageSpeed = Distance / timeSinceStart.TotalSeconds;
            return AverageSpeed;
        }
    }
}
