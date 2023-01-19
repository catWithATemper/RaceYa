using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RaceYa.Models.CollectionItems
{
    public class FinalLeaderBoardItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [Ignored]
        private string participantId;

        [MapTo("participantId")]
        public string ParticipantId
        {
            get
            {
                return participantId;
            }
            set
            {
                participantId = value;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        private string name;

        [MapTo("name")]
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

        [Ignored]
        private double averageSpeedKmH;

        [MapTo("averageSpeedKmH")]
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

        [Ignored]
        private double averagePacInMillise;

        [MapTo("averagePaceInMillis")]
        public double AveragePaceInMillis
        {
            get
            {
                return averagePacInMillise;
            }
            set
            {
                averagePacInMillise = value;
                NotifyPropertyChanged();
            }
        }

        [Ignored]
        private double evaluatedDistanceInKm;

        [MapTo("evaluatedDistanceInKm")]
        public double EvaluatedDistanceInKm
        {
            get
            {
                return evaluatedDistanceInKm;
            }
            set
            {
                evaluatedDistanceInKm = value;
                NotifyPropertyChanged();
            }
        }

        public FinalLeaderBoardItem()
        {
        }

        public FinalLeaderBoardItem(string participantId, string name, double averageSpeedKmH, double averagePaceInMillis, double evaluatedDistanceInKm)
        {
            ParticipantId = participantId;
            Name = name;
            AverageSpeedKmH = averageSpeedKmH;
            AveragePaceInMillis = averagePaceInMillis;
            EvaluatedDistanceInKm = evaluatedDistanceInKm;
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
