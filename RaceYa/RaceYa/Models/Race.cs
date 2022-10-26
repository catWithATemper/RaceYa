using System;
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
        public float RouteLength { get; set; } //in meters
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

        //Each pair contains a participant and their average speed
        public static SortedDictionary<Participant, double> FinalLeaerBoard { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<string> observableLeaderBoard;
        public ObservableCollection<String> ObservableLeaderBoard
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

        public Race()
        {
            //Hardcoded values
            RouteLength = 5000;
            EndDate = DateTime.Parse("November 15, 2022");

            Participants = new List<Participant>();

            leaderBoard = new SortedDictionary<Participant, double>(new ParticipantComparer());
            LeaderBoard = new SortedDictionary<Participant, double>(new ParticipantComparer());

            observableLeaderBoard = new ObservableCollection<string>();
            ObservableLeaderBoard = new ObservableCollection<string>();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //TODO: Add method to adjust for any extra distance run and save a final leaderboard
        //based on average speed
        //Improve the stopwatch synchronization. Consider the closest timestamp to the current participant
        //even though it may be "in the future"

        public void UpdateLeaderBoard()
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

                        Console.WriteLine("List " + LeaderBoard.Count);
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

                                participant.Result.CurrentRaceTime = participant.Result.RaceTimes.ElementAt(index - 1);
                                participant.Result.RaceTimeIndex = index - 1;

                                LeaderBoard.Add(participant, participant.Result.CurrentRaceTime.Value);

                                UpdateObservableLeaderboard();

                                Console.WriteLine("List " + LeaderBoard.Count);
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

        public void UpdateObservableLeaderboard()
        {
            ObservableLeaderBoard.Clear();
            foreach (Participant participant in LeaderBoard.Keys)
            {
                ObservableLeaderBoard.Add(participant.User.Name + " " + LeaderBoard[participant]);
            }
        }

    }
}
