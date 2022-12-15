using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RaceYa.Models
{
    public class ObservableFinalLeaderBoardItem : INotifyPropertyChanged
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

        private double averageSpeedKmH;
        
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

        public ObservableFinalLeaderBoardItem(int rank, string name, double averageSpeedKmH, TimeSpan averagePace)
        {
            Rank = rank;
            Name = name;
            AverageSpeedKmH = averageSpeedKmH;
            AveragePace = averagePace;
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
