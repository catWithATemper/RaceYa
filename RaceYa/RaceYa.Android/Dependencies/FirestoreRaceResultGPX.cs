using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CloudFirestore;
using RaceYa.Helpers;
using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreRaceResultGPX))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreRaceResultGPX : IFirestoreRaceResultGPX
    {

        public async Task<string> Add(RaceResultGPX resultGPX, string participantId, string resultId)
        {
            try
            {
                var documentReference = await CrossCloudFirestore.Current.Instance
                                                                 .Collection("participants")
                                                                 .Document(participantId)
                                                                 .Collection("raceResults")
                                                                 .Document(resultId)
                                                                 .Collection("raceResultGPX")
                                                                 .AddAsync(resultGPX);
                resultGPX.Id = documentReference.Id;

                return documentReference.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<RaceResultGPX> ReadRaceResultGPXByParticipantAndResultIds(string participantId, string resultId)
        {
            try
            {
                var query = await CrossCloudFirestore.Current.Instance.Collection("participants")
                                                                      .Document(participantId)
                                                                      .Collection("raceResults")
                                                                      .Document(resultId)
                                                                      .Collection("raceResultGPX")
                                                                     .LimitTo(1).GetAsync();

                var results = query.ToObjects<RaceResultGPX>();

                return results.ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}