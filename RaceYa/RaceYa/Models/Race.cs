using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using RaceYa.Models.CollectionItems;

namespace RaceYa.Models
{
    public class Race : INotifyPropertyChanged
    {
        //TDDO fix CalculateFinalLEaderboard() and CalculateIDFinalLeaderboard(): change conddition for the if statement
        //TODO: Consider changing the update frequency for the ObservableLeaderBoard

        [Id]
        public string Id { get; set; }

        [MapTo("userId")]
        public string UserId { get; set; } //The user who created the race

        [MapTo("endDate")]
        public DateTime EndDate { get; set; }

        [MapTo("startDate")]
        public DateTime StartDate { get; set; }

        [Ignored]
        public double RouteLength //in meters
        {
            get { return RouteLengthInKm * 1000; }
        }

        [MapTo("routeLengthinKm")]
        public double RouteLengthInKm { get; set; }

        [MapTo("description")]
        public string Description { get; set; }

        [Ignored]
        public Participant CurrentParticipant { get; set; }

        [Ignored]
        public List<Participant> Participants { get; set; }

        //Each pair contains a participant and the distance covered by the participant at the current time
        [Ignored]
        private SortedSet<Participant> leaderBoard;

        [Ignored]
        public SortedSet<Participant> LeaderBoard
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

        [Ignored]
        public SortedSet<Participant> finalLeaderBoardSet;

        [Ignored]
        public SortedSet<Participant> FinalLeaderBoardSet
        {
            get
            {
                return finalLeaderBoardSet;
            }
            set
            {
                finalLeaderBoardSet = value;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        public bool FinalLeaderBoardSetCalculated = false;

        [Ignored]
        private List<FinalLeaderBoardItem> finalLeaderBoard;

        [MapTo("finalLeaderBoard")]
        public List<FinalLeaderBoardItem> FinalLeaderBoard
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

        [Ignored]
        public bool FinalLeaderBoardCalculated = false;

        public event PropertyChangedEventHandler PropertyChanged;

        [Ignored]
        private ObservableCollection<ObservableLeaderBoardItem> observableLeaderBoard;

        [Ignored]
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

        [Ignored]
        private ObservableCollection<ObservableFinalLeaderBoardItem> observableFinalLeaderBoard;

        [Ignored]
        public ObservableCollection<ObservableFinalLeaderBoardItem> ObservableFinalLeaderBoard
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
            StartDate = startDate;
            EndDate = endDate;
            Description = description;

            Participants = new List<Participant>();
            LeaderBoard = new SortedSet<Participant>(new LeaderBoardComparer());
            ObservableLeaderBoard = new ObservableCollection<ObservableLeaderBoardItem>();
            finalLeaderBoardSet = new SortedSet<Participant>(new FinalLeaderBoardSetComparer());
            FinalLeaderBoard = new List<FinalLeaderBoardItem>();//
            ObservableFinalLeaderBoard = new ObservableCollection<ObservableFinalLeaderBoardItem>();
        }

        public Race()
        {
            Participants = new List<Participant>();
            LeaderBoard = new SortedSet<Participant>(new LeaderBoardComparer());
            ObservableLeaderBoard = new ObservableCollection<ObservableLeaderBoardItem>();
            finalLeaderBoardSet = new SortedSet<Participant>(new FinalLeaderBoardSetComparer());
            FinalLeaderBoard = new List<FinalLeaderBoardItem>();//
            ObservableFinalLeaderBoard = new ObservableCollection<ObservableFinalLeaderBoardItem>();
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
                            //Check if you can read the current participant's RaceTimes list and pick the newest element
                            //instead of creating a new CurrentRaceTime value pair from scratch.
                            LeaderBoard.Remove(participant);
                            participant.Result.CurrentRaceTime = new KeyValuePair<TimeSpan, double>(participant.Result.TimeSinceStart,
                                                                                                    participant.Result.CoveredDistance);
                            LeaderBoard.Add(participant);

                            UpdateObservableLeaderboard();

                            participant.Result.LeaderBoardRank = Array.IndexOf(LeaderBoard.ToArray(), participant) + 1;
                        }
                        else
                        {
                            for (int index = participant.Result.RaceTimeIndex; index < participant.Result.RaceTimes.Count; index++)
                            {
                                if (participant.Result.RaceTimes.ElementAt(index).Key > CurrentParticipant.Result.TimeSinceStart)
                                {
                                    LeaderBoard.Remove(participant);

                                    SynchronizeRaceResults(participant, index);

                                    LeaderBoard.Add(participant);

                                    UpdateObservableLeaderboard();

                                    participant.Result.LeaderBoardRank = Array.IndexOf(LeaderBoard.ToArray(), participant) + 1;

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
            foreach (Participant participant in LeaderBoard)
            {
                ObservableLeaderBoard.Add(new ObservableLeaderBoardItem(Array.IndexOf(LeaderBoard.ToArray(), participant) + 1,
                                                                        participant.User.Name,
                                                                        participant.Result.CurrentRaceTime.Value / 1000,
                                                                        participant.IsCurrentParticipant));
            }

            Console.WriteLine("Leaderboard: ");
            foreach (Participant participant in LeaderBoard)
            {
                Console.WriteLine("Position: " + (Array.IndexOf(LeaderBoard.ToArray(), participant) + 1) +
                                   " Name: " + participant.User.Name +
                                   " Time: " + participant.Result.CurrentRaceTime.Key +
                                   " Distance: " + participant.Result.CurrentRaceTime.Value);
            }
        }

        public void CalculateFinalLeaderBoardSet()
        {
            if (FinalLeaderBoardSetCalculated == false)
            {
                FinalLeaderBoardSet.Clear();
                foreach (Participant participant in Participants)
                {
                    FinalLeaderBoardSet.Add(participant);
                }
            }
            FinalLeaderBoardSetCalculated = true;
        }

        public void CalculateFinalLeaderBoard()
        {
            if (FinalLeaderBoardCalculated == false)
            {
                finalLeaderBoard.Clear();
                foreach (Participant participant in FinalLeaderBoardSet)
                {
                    FinalLeaderBoard.Add(new FinalLeaderBoardItem(participant.Id,
                                                                  participant.User.Name,
                                                                  participant.Result.AverageSpeedKmH,
                                                                  participant.Result.AveragePaceInMillis));
         
                    participant.Result.LeaderBoardRank = Array.IndexOf(FinalLeaderBoardSet.ToArray(), participant) + 1;
                }

                FinalLeaderBoardCalculated = true;
                
                ObservableFinalLeaderBoard.Clear();
                foreach (FinalLeaderBoardItem item in FinalLeaderBoard)
                {
                    ObservableFinalLeaderBoard.Add(new ObservableFinalLeaderBoardItem(Array.IndexOf(FinalLeaderBoard.ToArray(), item) + 1,
                                                                                      item.Name,
                                                                                      item.AverageSpeedKmH,
                                                                                      TimeSpan.FromMilliseconds(item.AveragePaceInMillis)));
                }
            }
        }
    }
}
