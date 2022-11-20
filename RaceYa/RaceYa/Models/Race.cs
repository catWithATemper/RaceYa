﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RaceYa.Models
{
    public class Race : INotifyPropertyChanged
    {
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public double RouteLength { get; set; } //in meters

        public double RouteLengthInKm { get; set; }

        public string Description { get; set; }
        public Participant CurrentParticipant { get; set; }
        public List<Participant> Participants { get; set; }

        //Each pair contains a participant and the distance covered by the participant at the current time
        private SortedDictionary<Participant, double> leaderBoard;
        public SortedDictionary<Participant, double> LeaderBoard
        {
            get
            {
                return leaderBoard;
            }
            set
            {
                leaderBoard = value;
                NotifyPropertyChanged();
            }
        }

        private SortedDictionary<Participant, FinalLeaderBoardItem> finalLeaderBoard;

        public SortedDictionary<Participant, FinalLeaderBoardItem> FinalLeaderBoard
        {
            get
            {
                return finalLeaderBoard;
            }
            set
            {
                finalLeaderBoard = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ObservableLeaderBoardItem> observableLeaderBoard;
        public ObservableCollection<ObservableLeaderBoardItem> ObservableLeaderBoard
        {
            get
            {
                return observableLeaderBoard;
            }
            set
            {
                observableLeaderBoard = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<FinalLeaderBoardItem> observableFinalLeaderBoard;
        
        public ObservableCollection<FinalLeaderBoardItem> ObservableFinalLeaderBoard
        {
            get
            {
                return observableFinalLeaderBoard;
            }
            set
            {
                observableFinalLeaderBoard = value;
                NotifyPropertyChanged();
            }
        }

        public Race(double routeLengthInKm, DateTime startDate, DateTime endDate, string description)
        {
            RouteLengthInKm = routeLengthInKm;
            RouteLength = RouteLengthInKm * 1000;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;

            Participants = new List<Participant>();
            LeaderBoard = new SortedDictionary<Participant, double>(new LeaderBoardComparer());
            ObservableLeaderBoard = new ObservableCollection<ObservableLeaderBoardItem>();
            FinalLeaderBoard = new SortedDictionary<Participant, FinalLeaderBoardItem>(new FinalLeaderBoardComparer());
            ObservableFinalLeaderBoard = new ObservableCollection<FinalLeaderBoardItem>();

            //Debug
            Console.WriteLine("Race created:");
            Console.WriteLine("Route Length: " + RouteLength + " Start date: " + StartDate + " End date: " +
                              EndDate + " Description: " + Description);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateLeaderBoard()
        {
            if (CurrentParticipant.Result.RaceCompleted == false)
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.Result.RaceTimes != null)
                    {
                        if (participant == CurrentParticipant)
                        {
                            LeaderBoard.Remove(participant);
                            participant.Result.CurrentRaceTime = new KeyValuePair<TimeSpan, double>(participant.Result.TimeSinceStart,
                                                                                                    participant.Result.CoveredDistance);
                            LeaderBoard.Add(participant, participant.Result.CurrentRaceTime.Value);

                            UpdateObservableLeaderboard();

                            participant.Result.LeaderBoardRank = Array.IndexOf(LeaderBoard.Keys.ToArray(), participant) + 1;

                            Console.WriteLine("Leaderboard " + LeaderBoard.Count);
                            Console.WriteLine("Name Time Distance");
                            foreach (var p in LeaderBoard)
                            {
                                Console.WriteLine(p.Key.User.Name + " " +
                                                  p.Key.Result.CurrentRaceTime.Key + " " +
                                                  p.Key.Result.CurrentRaceTime.Value);
                            }
                        }
                        else
                        {
                            for (int index = participant.Result.RaceTimeIndex; index < participant.Result.RaceTimes.Count; index++)
                            {
                                if (participant.Result.RaceTimes.ElementAt(index).Key > CurrentParticipant.Result.TimeSinceStart)
                                {
                                    LeaderBoard.Remove(participant);

                                    SynchronizeRaceResults(participant, index);

                                    LeaderBoard.Add(participant, participant.Result.CurrentRaceTime.Value);

                                    UpdateObservableLeaderboard();

                                    participant.Result.LeaderBoardRank = Array.IndexOf(LeaderBoard.Keys.ToArray(), participant) + 1;

                                    Console.WriteLine("Leaderboard " + LeaderBoard.Count);
                                    Console.WriteLine("Name Time Distance");
                                    foreach (var p in LeaderBoard)
                                    {
                                        Console.WriteLine(p.Key.User.Name + " " +
                                                          p.Key.Result.CurrentRaceTime.Key + " " +
                                                          p.Key.Result.CurrentRaceTime.Value);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }


        public void SynchronizeRaceResults(Participant participant, int index)
        {
            //Find the difference between the current participant's stopwatch value and the participant's 
            //stopwatch value identified by the RaceTimeIndex property. Then do the same with the stopwatch
            //value identified by RaceTimeIndex - 1. Finally use the participant's stopwatch value with the
            //smaller difference for the speed comparison.          
            if ((participant.Result.RaceTimes.ElementAt(index).Key -
                CurrentParticipant.Result.TimeSinceStart).Duration() >
                (participant.Result.RaceTimes.ElementAt(index - 1).Key -
                CurrentParticipant.Result.TimeSinceStart).Duration())
            {
                participant.Result.CurrentRaceTime = participant.Result.RaceTimes.ElementAt(index - 1);
                participant.Result.RaceTimeIndex = index - 1;
            }
            else
            {
                participant.Result.CurrentRaceTime = participant.Result.RaceTimes.ElementAt(index);
            }
        }


        public void UpdateObservableLeaderboard()
        {
            ObservableLeaderBoard.Clear();
            foreach (Participant participant in LeaderBoard.Keys)
            {
                ObservableLeaderBoard.Add(new ObservableLeaderBoardItem(Array.IndexOf(LeaderBoard.Keys.ToArray(), participant) + 1,
                                                                        participant.User.Name,
                                                                        Math.Round(LeaderBoard[participant] / 1000, 2),
                                                                        participant.IsCurrentParticipant));
            }
        }

        public void CalculateFinalLeaderBoard()
        {
            //First erify that the final leaderboard hasn't been populated yet.
            if (FinalLeaderBoard.Count == 0)
            {
                foreach (Participant participant in Participants)
                {
                    FinalLeaderBoard.Add(participant,
                                         new FinalLeaderBoardItem(Array.IndexOf(FinalLeaderBoard.Keys.ToArray(), participant) + 1,
                                                                  participant.User.Name,
                                                                  participant.Result.AverageSpeedKmH,
                                                                  participant.Result.AveragePace));
                    participant.Result.LeaderBoardRank = Array.IndexOf(FinalLeaderBoard.Keys.ToArray(), participant) + 1;

                    //debug
                    Console.WriteLine("Final leaderboard " + FinalLeaderBoard.Count);
                    Console.WriteLine("Name Speed Distance Time EvaluatedDistance");
                    //debug
                    foreach (var p in FinalLeaderBoard)
                    {
                        Console.WriteLine(p.Key.User.Name + " " +
                                          p.Key.Result.AverageSpeed + " "+
                                          p.Key.Result.CoveredDistance + " " +
                                          p.Key.Result.TimeSinceStart + " " +
                                          p.Key.Result.EvaluatedDistance + "\n");
                    }
                }

                ObservableFinalLeaderBoard.Clear();
                foreach (Participant participant in FinalLeaderBoard.Keys)
                {
                    ObservableFinalLeaderBoard.Add(new FinalLeaderBoardItem(Array.IndexOf(FinalLeaderBoard.Keys.ToArray(), participant) + 1,
                                                                            participant.User.Name,
                                                                            participant.Result.AverageSpeedKmH,
                                                                            participant.Result.AveragePace));
                }
            }
        }
    }
}
