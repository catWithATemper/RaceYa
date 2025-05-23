﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace RaceYa.Models
{
    public class RaceResult : INotifyPropertyChanged
    {
        [Id]
        public string Id { get; set; }

        [MapTo("participantId")]
        public string ParticipantId { get; set; }

        [MapTo("raceId")]
        public string RaceId { get; set; }

        [Ignored]
        public Participant RaceParticipant;

        [Ignored]
        public Location CurrentLocation { get; set; }

        [Ignored]
        private string latitude;

        [Ignored]
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

        [Ignored]
        private string longitude;

        [Ignored]
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

        [Ignored]
        Location PreviousLocation { get; set; }

        [Ignored]
        Location StartingPoint;

        [Ignored]
        private DateTime startTime;

        [MapTo("startTime")]
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

        private double timeSinceStartInMillis;

        [MapTo("timeSinceStartInMillis")]
        public double TimeSinceStartInMillis
        {
            get
            {
                return timeSinceStartInMillis;
            }
            set
            {
                timeSinceStartInMillis = value;
            }
        }

        [Ignored]
        public TimeSpan TimeSinceStart
        {
            get
            {
                return TimeSpan.FromMilliseconds(timeSinceStartInMillis);
            }
            set
            {
                timeSinceStartInMillis = value.TotalMilliseconds;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        private double coveredDistance;

        [MapTo("coveredDistance")]
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

        [Ignored]
        private double coveredDistanceInKm;

        [Ignored]
        public double CoveredDistanceInKm
        {
            get
            {
                return CoveredDistance / 1000; 
            }
            set
            {
                coveredDistanceInKm = value;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        private double averageSpeed; //m/s

        [MapTo("averageSpeedMeterSecond")]
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

        [Ignored]
        private double averageSpeedKmH; // km/h

        [Ignored]
        public double AverageSpeedKmH
        {
            get
            {
                return AverageSpeed * 3.6;
            }
            set
            {
                averageSpeedKmH = value;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        private double averagePaceInMillis;

        [MapTo("averagePaceInMillis")]
        public double AveragePaceInMillis
        {
            get
            {
                return averagePaceInMillis;
            }
            set
            {
                averagePaceInMillis = value;
            }
        }

        [Ignored]
        public TimeSpan AveragePace
        {
            get
            {
                return TimeSpan.FromMilliseconds(averagePaceInMillis);
            }
            set
            {
                averagePaceInMillis = value.TotalMilliseconds;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        private string averagePaceString;

        [Ignored]
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

        [Ignored]
        private double remainingDistance;

        [Ignored]
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

        [Ignored]
        private double evaluatedDistanceInMeters;

        [MapTo("evaluatedDistanceInMeters")]
        public double EvaluatedDistanceInMeters
        {
            get
            {
                return evaluatedDistanceInMeters;
            }
            set
            {
                evaluatedDistanceInMeters = value;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        public SortedDictionary<TimeSpan, double> RaceTimes;

        [MapTo("raceTimesMillis")]
        public SortedDictionary<double, double> RaceTimesInMillis
        {
            get
            {
                SortedDictionary<double, double> convertedDictionary = new SortedDictionary<double, double>();
                foreach (TimeSpan key in RaceTimes.Keys)
                {
                    convertedDictionary.Add(key.TotalMilliseconds, RaceTimes[key]);
                }
                return convertedDictionary;
            }
            set
            {
                SortedDictionary<TimeSpan, double> convertedDictionary = new SortedDictionary<TimeSpan, double>();
                foreach (double key in value.Keys)
                {
                    convertedDictionary.Add(TimeSpan.FromMilliseconds(key), value[key]);
                }
                RaceTimes = convertedDictionary;
            }
        }

        [Ignored]
        public KeyValuePair<TimeSpan, double> CurrentRaceTime;

        [Ignored]
        public int RaceTimeIndex { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [Ignored]
        public bool RaceCompleted;

        [Ignored]
        private int? leaderBoardRank;

        [MapTo("leaderboardRank")]
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

        [Ignored]
        private double accuracy;

        [Ignored]
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

        [Ignored]
        private double gpsSpeed;

        [Ignored]
        public double GPSSpeed
        {
            get
            {
                return gpsSpeed;
            }
            set
            {
                gpsSpeed = value;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        public bool GPXRequired;

        [Ignored]
        public TrackSegment TrackSegment { get; set; }

        public RaceResult()
        {
            TrackSegment = new TrackSegment();  
        }

        public RaceResult(Participant participant)
        {
            RaceParticipant = participant;

            CoveredDistance = 0;
            coveredDistanceInKm = 0;
            AverageSpeed = 0;
            AveragePace = new TimeSpan(0, 0, 0);
            AveragePaceString = "00:00";

            RaceTimes = new SortedDictionary<TimeSpan, double>();
            CurrentRaceTime = new KeyValuePair<TimeSpan, double>(new TimeSpan(0), 0);
            RaceTimeIndex = 0;

            RaceCompleted = false;

            LeaderBoardRank = null;

            StartTime = DateTime.Now;

            //Debug
            Accuracy = 0;

            GPXRequired = false;
            TrackSegment = new TrackSegment();     
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AssignRaceId(string raceId)
        {
            RaceId = raceId;
        }

        public void SetCurrentLocation(Location newReading)
        {
            PreviousLocation = CurrentLocation;
            CurrentLocation = newReading;

            if (RaceCompleted == false)
            {
                CalculateTimeSinceStart();

                if (PreviousLocation != null)
                {
                    CalculateCoveredDistance();
                }

                Latitude = Math.Round(CurrentLocation.Latitude, 6).ToString();
                Longitude = Math.Round(CurrentLocation.Longitude, 6).ToString();

                if (GPXRequired)
                {
                    TrackPoint trackPoint = new TrackPoint();
                    trackPoint.Coordinates = new GeoPoint(newReading.Latitude, newReading.Longitude);
                    trackPoint.Time = newReading.Timestamp.DateTime;
                    TrackSegment.TrackPoints.Add(trackPoint);
                }
            }
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

            //
            CoveredDistanceInKm = CoveredDistance / 1000;
            CalculateAveragePace();

            return CoveredDistance;
        }

        private double CalculateAverageSpeed()
        {
            if (TimeSinceStart.TotalSeconds != 0)
            {
                AverageSpeed = CoveredDistance / TimeSinceStart.TotalSeconds;

                //
                AverageSpeedKmH = AverageSpeed * 3.6; //Moved to properties

                return AverageSpeed;
            }
            else
            {
                return 0;
            }
        }

        public void CalculateAveragePace()
        {
            if (CoveredDistanceInKm != 0 && CoveredDistance != 0)
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
                EvaluatedDistanceInMeters = CoveredDistance;
            }
            else
            {
                EvaluatedDistanceInMeters = RaceParticipant.Race.RouteLength;
            }
        }
    }
}
