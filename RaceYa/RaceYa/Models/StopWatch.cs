using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace RaceYa.Models
{
    public class StopWatch : INotifyPropertyChanged
    {
        public Timer ActualTimer;

        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime
        {
            get
            {
                return elapsedTime;
            }
            set
            {
                elapsedTime = value;
                NotifyPropertyChanged();
            }
        }

        private string elapsedTimeString;
        public String ElapsedTimeString
        {
            get
            {
                return elapsedTimeString;
            }
            set
            {
                elapsedTimeString = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime StartTime;

        public StopWatch()
        {
            ElapsedTime = new TimeSpan();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetTimer()
        {
            ActualTimer = new Timer(100);

            ActualTimer.Elapsed += OnTimedEvent;
            ActualTimer.AutoReset = true;

            StartTime = DateTime.Now;

            ActualTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ElapsedTimeString = (StartTime - e.SignalTime).ToString(@"hh\:mm\:ss\.f");
        }
    }
}
