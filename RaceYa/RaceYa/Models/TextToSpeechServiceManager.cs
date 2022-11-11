using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Timers;

namespace RaceYa.Models
{
    public class TextToSpeechServiceManager
    {
        public Timer TextToSpeechTimer;

        public RaceResult Result;

        public TextToSpeechServiceManager(RaceResult result)
        {
            Result = result;
        }

        public void StartTextToSpeech()
        {
            TextToSpeechTimer = new Timer(30000);//30 seconds
            TextToSpeechTimer.Elapsed += OnTimedEvent;
            TextToSpeechTimer.AutoReset = true;
            TextToSpeechTimer.Enabled = true;
        }

        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (Result.LeaderBoardRank != null)
            {
                string rank = ManageOridnalNumber(Result.LeaderBoardRank);

                await TextToSpeech.SpeakAsync("Leaderboard position: " + rank + " place.");
            }
        }

        private string ManageOridnalNumber(int? num)
        {
            if (num <= 0) return num.ToString();

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }

        public void StopTextToSpeech()
        {
            TextToSpeechTimer.Dispose();
        }

        /*
        * Implementation with Threading.Timers.Timer
        public AutoResetEvent autoEvent = new AutoResetEvent(false);

        public Timer TextToSpeechTimer; 

        public void StartTextToSpeech()
        {
            TextToSpeechTimer = new Timer(CallTextToSpeech, autoEvent, 15000, 15000);
        }

        public async void CallTextToSpeech(Object stateInfo)
        {
            await TextToSpeech.SpeakAsync("Go faster");
        }
        */
    }
}
