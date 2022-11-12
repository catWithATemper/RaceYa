using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RaceYa.Models
{
    public class ObservableLeaderBoardItem : INotifyPropertyChanged
    {
        private int rank;
        public int Rank {
            get
            {
                return rank;
            }
            set
            {
                rank = value;
                NotifyPropertyChanged();
            }
        }

        private string name;
        public string Name 
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        private double coveredDistance; // in km
        public double  CoveredDistance //in km
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

        private bool isCurrentParticipant;
        public bool IsCurrentParticipant
        {
            get
            {
                return isCurrentParticipant;
            }
            set
            {
                isCurrentParticipant = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableLeaderBoardItem(int rank, string name, double coveredDistance, bool isCurrentParticipant)
        {
            Rank = rank;
            Name = name;
            CoveredDistance = coveredDistance;
            IsCurrentParticipant = isCurrentParticipant;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
