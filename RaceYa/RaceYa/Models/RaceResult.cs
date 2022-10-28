using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class RaceResult : INotifyPropertyChanged
    {
        public Participant RaceParticipant;

        public Location CurrentLocation { get; set; }

        private string latitude;
        public string Latitude
        {
            get
            {
                if (CurrentLocation != null)
                {
                    return latitude;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                latitude = value;
                NotifyPropertyChanged();
            }
        }

        private string longitude;
        public string Longitude
        {
            get
            {
                if (CurrentLocation != null)
                {
                    return longitude;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                longitude = value;
                NotifyPropertyChanged();
            }
        }

        Location PreviousLocation { get; set; }

        Location StartingPoint;

        DateTime StartTime;

        public TimeSpan TimeSinceStart;

        private double coveredDistance;
        public double CoveredDistance 
        { 
            get 
            { 
                return coveredDistance; 
            }
            set 
            {
                coveredDistance = value;
                NotifyPropertyChanged();
            }
        }

        private double averageSpeed;
        public double AverageSpeed
        {
            get
            {
                return averageSpeed;
            }
            set
            {
                averageSpeed = value;
                NotifyPropertyChanged();
            }
        }

        public SortedDictionary<TimeSpan, double> RaceTimes;

        public KeyValuePair<TimeSpan, double> CurrentRaceTime;

        public int RaceTimeIndex { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool RaceCompleted;

        public TimeSpan RaceCompletionTime;

        public RaceResult()
        {   
            CoveredDistance = 0;
            AverageSpeed = 0;
            RaceTimes = new SortedDictionary<TimeSpan, double>();
            CurrentRaceTime = new KeyValuePair<TimeSpan, double>(new TimeSpan(0), 0);
            RaceTimeIndex = 0;

            RaceCompleted = false;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetCurrentLocation(Location newReading)
        {
            PreviousLocation = CurrentLocation;
            CurrentLocation = newReading;

            Latitude = Math.Round(CurrentLocation.Latitude, 6).ToString();
            Longitude = Math.Round(CurrentLocation.Longitude, 6).ToString();
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
