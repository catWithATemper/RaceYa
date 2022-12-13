using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using RaceYa.Helpers;
using RaceYa.Models;
using Plugin.CloudFirestore;
using System.Threading.Tasks;

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreParticipant))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreParticipant : IFirestoreParticipant //Java.Lang.Object
    {
        public async Task<string> Add(Participant participant)
        {
            try
            {
                var documentReference = await CrossCloudFirestore.Current.Instance.Collection("participants").AddAsync(participant);

                participant.Id = documentReference.Id;

                return documentReference.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public async Task<Participant> ReadParticipantById(string id)
        {
            try
            {
                var document = await CrossCloudFirestore.Current.Instance.Collection("participants").Document(id).GetAsync();
                var participant = document.ToObject<Participant>();

                return participant;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<Participant> ReadParticipantByUserAndRace(string userId, string raceId)
        {
            try
            {
                var query = await CrossCloudFirestore.Current.Instance.Collection("participants")
                                                                      .WhereEqualsTo("userId", userId)
                                                                      .WhereEqualsTo("raceId", raceId)
                                                                      .LimitTo(1)
                                                                      .GetAsync();
                var participants = query.ToObjects<Participant>();

                return participants.ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}