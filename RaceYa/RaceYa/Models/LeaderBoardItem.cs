using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaceYa.Models
{
    public class LeaderBoardItem : INotifyPropertyChanged
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

        private double coveredDistance;
        public double  CoveredDistance 
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

        public event PropertyChangedEventHandler PropertyChanged;

        public LeaderBoardItem(int rank, string name, double coveredDistance)
        {
            Rank = rank;
            Name = name;
            CoveredDistance = coveredDistance;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
