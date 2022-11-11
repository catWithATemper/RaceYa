using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RaceYa.Models
{
    public class FinalLeaderBoardItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int rank;
        public int Rank
        {
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

        public FinalLeaderBoardItem(int rank, string name, double averageSpeed, TimeSpan averagePace)
        {
            Rank = rank;
            Name = name;
            AverageSpeed = averageSpeed;
            AveragePace = averagePace;
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
