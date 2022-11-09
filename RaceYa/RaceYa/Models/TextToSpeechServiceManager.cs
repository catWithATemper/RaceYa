using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class TextToSpeechServiceManager
    {
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


        /*
        public Timer TextToSpeechTimer;

        public void StartTextToSpeech()
        {
            TextToSpeechTimer = new Timer(30000);//30 seconds
            TextToSpeechTimer.Elapsed += OnTimedEvent;
            TextToSpeechTimer.AutoReset = true;
            TextToSpeechTimer.Enabled = true;
        }

        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            await TextToSpeech.SpeakAsync("Run faster");
        }
        */
    }
}
