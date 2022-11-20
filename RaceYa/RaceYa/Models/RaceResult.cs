﻿using System;
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

        private DateTime startTime;

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                NotifyPropertyChanged();
            }
        }

        private TimeSpan timeSinceStart;
        public TimeSpan TimeSinceStart
        {
            get
            {
                return timeSinceStart;
            }
            set
            {
                timeSinceStart = value;
                NotifyPropertyChanged();
            }
        }

        private double coveredDistance;
        public double CoveredDistance //meters
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

        private double coveredDitanceInKm;

        public double CoveredDistanceInKm
        {
            get
            {
                return coveredDitanceInKm;
            }
            set
            {
                coveredDitanceInKm = value;
                NotifyPropertyChanged();
            }
        }

        private double averageSpeed; //m/s
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

        private double averageSpeedKmH; // km/h

        public double AverageSpeedKmH
        {
            get
            {
                return averageSpeedKmH;
            }
            set
            {
                averageSpeedKmH = value;
                NotifyPropertyChanged();
            }
        }


        private TimeSpan averagePace;

        public TimeSpan AveragePace
        {
            get
            {
                return averagePace;
            }
            set
            {
                averagePace = value;
                NotifyPropertyChanged();
            }
        }

        private string averagePaceString;

        public string AveragePaceString
        {
            get
            {
                return averagePaceString;
            }
            set
            {
                averagePaceString = value;
                NotifyPropertyChanged();
            }
        }

        private double remainingDistance;

        public double RemainingDistance //in km
        {
            get
            {
                return remainingDistance;
            }
            set
            {
                remainingDistance = value;
                NotifyPropertyChanged();
            }
        }

        private double evaluatedDistance;
        
        public double EvaluatedDistance
        {
            get
            {
                return evaluatedDistance;
            }
            set
            {
                evaluatedDistance = value;
                NotifyPropertyChanged();
            }
        }

        public SortedDictionary<TimeSpan, double> RaceTimes;

        public KeyValuePair<TimeSpan, double> CurrentRaceTime;

        public int RaceTimeIndex { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool RaceCompleted;

        private int? leaderBoardRank;

        public int? LeaderBoardRank
        {
            get
            {
                return leaderBoardRank;
            }
            set
            {
                leaderBoardRank = value;
                NotifyPropertyChanged();
            }
        }

        private double accuracy;
        public double Accuracy
        {
            get
            {
                return accuracy;
            }
            set
            {
                accuracy = value;
                NotifyPropertyChanged();
            }
        }

        public RaceResult(Participant participant)
        {
            RaceParticipant = participant;

            CoveredDistance = 0;
            coveredDitanceInKm = 0;
            AverageSpeed = 0;
            AveragePace = new TimeSpan(0, 0, 0);
            AveragePaceString = "00:00";
            RemainingDistance = participant.Race.RouteLength / 1000;
            RaceTimes = new SortedDictionary<TimeSpan, double>();
            CurrentRaceTime = new KeyValuePair<TimeSpan, double>(new TimeSpan(0), 0);
            RaceTimeIndex = 0;

            RaceCompleted = false;

            LeaderBoardRank = null;

            //Debug
            Accuracy = 0;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetCurrentLocation(Location newReading)
        {
            PreviousLocation = CurrentLocation;
            CurrentLocation = newReading;

            CalculateTimeSinceStart();

            if (PreviousLocation != null)
            {
                CalculateCoveredDistance();
            }

            Latitude = Math.Round(CurrentLocation.Latitude, 6).ToString();
            Longitude = Math.Round(CurrentLocation.Longitude, 6).ToString();
        }

        public void SetStartingPoint()
        {
            StartingPoint = CurrentLocation;
            StartTime = StartingPoint.Timestamp.DateTime;
        }

        private void CalculateTimeSinceStart()
        {
            TimeSinceStart = CurrentLocation.Timestamp.DateTime - StartTime;
        }

        private double CalculateCoveredDistance()
        {
            CoveredDistance += Location.CalculateDistance(CurrentLocation, PreviousLocation, DistanceUnits.Kilometers) * 1000;

            if (!RaceTimes.ContainsKey(TimeSinceStart))
            {
                RaceTimes.Add(TimeSinceStart, CoveredDistance);
            }

            DetermineEvaluatedDistance();
            CalculateAverageSpeed();

            RemainingDistance = (RaceParticipant.Race.RouteLength - CoveredDistance) / 1000;

            CoveredDistanceInKm = CoveredDistance / 1000;
            CalculateAveragePace();

            return CoveredDistance;
        }

        private double CalculateAverageSpeed()
        {
            if (TimeSinceStart.TotalSeconds != 0)
            {
                AverageSpeed = CoveredDistance / TimeSinceStart.TotalSeconds;

                AverageSpeedKmH = AverageSpeed * 3.6;

                return AverageSpeed;
            }
            else
            {
                return 0;
            }
        }

        public void CalculateAveragePace()
        {
            if (CoveredDistanceInKm != 0 && CoveredDistance !=0)
            {
                double paceAsDouble = TimeSinceStart.TotalMinutes / CoveredDistanceInKm;

                int minutes = (int)Math.Floor(paceAsDouble);
                int seconds = (int)((paceAsDouble - Math.Floor(paceAsDouble)) * 60);

                AveragePace = new TimeSpan(0, minutes, seconds);

                //Console.WriteLine("Speed m/s: " + AverageSpeed + " Pace min/km " + AveragePace);     
            }
            else
            {
                AveragePace = new TimeSpan(0, 0, 0);
            }
            AveragePaceString = AveragePace.ToString(@"mm\:ss");
        }

        public void DetermineEvaluatedDistance()
        {
            if (CoveredDistance <= RaceParticipant.Race.RouteLength)
            {
                EvaluatedDistance = CoveredDistance;
            }
            else
            {
                EvaluatedDistance = RaceParticipant.Race.RouteLength;
            }
        }
    }
}
