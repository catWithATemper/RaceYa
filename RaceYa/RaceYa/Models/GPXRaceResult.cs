﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    class GPXRaceResult
    {
        List<Location> LocationReadings = new List<Location>();
        public double Distance { get; set; }
        public double AverageSpeed { get; set; }

        GPXParser LocationProvider = new GPXParser();

        public GPXRaceResult()
        {
            LocationReadings = GetLocationReadings();
            Distance = CalculateDistance();
            AverageSpeed = CalculateAverageSpeed();
        }

        List<Location> GetLocationReadings()
        {
            List<Location> LocationReadings = new List<Location>();

            foreach (TrackPoint Reading in LocationProvider.LocationReadings)
            {
                Location Location = new Location(Reading.latitude, Reading.longitude, Reading.TimeStamp);
                LocationReadings.Add(Location);
            }
            return LocationReadings;
        }

        double CalculateDistance()
        {
            LocationReadings = GetLocationReadings();
            double distance = 0;
            for (int i = 0; i < LocationReadings.Count - 1; i++)
            {
                distance += Location.CalculateDistance(LocationReadings[i], LocationReadings[i + 1], DistanceUnits.Kilometers) *1000;
            }
            return distance;
        }

        double CalculateAverageSpeed()
        {
            int n = LocationReadings.Count;
            Distance = CalculateDistance();
            DateTime StartTime = LocationReadings[0].Timestamp.DateTime;
            DateTime EndTime = LocationReadings[n - 1].Timestamp.DateTime;
            TimeSpan NetTime = EndTime.Subtract(StartTime);
            double speed = Distance / NetTime.TotalSeconds;

            return speed;
        }
    }
}
