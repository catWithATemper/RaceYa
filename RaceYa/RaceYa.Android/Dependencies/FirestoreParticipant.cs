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

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreParticipant))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreParticipant : Java.Lang.Object, IFirestoreParticipant
    {
        public bool Insert(Participant participant)
        {
            try
            {
                Dictionary<string, Java.Lang.Object> userDocument = new Dictionary<string, Java.Lang.Object>
                {
                    {"userId", participant.UserId},
                    {"raceId", participant.RaceId},
                };
                var collection = FirebaseFirestore.Instance.Collection("participants");
                collection.Add(new HashMap(userDocument));
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool InsertWithCustomId(Participant participant, string id)
        {
            try
            {
                var document = FirebaseFirestore.Instance.Collection("participants").Document(id);
                Dictionary<string, Java.Lang.Object> userDocument = new Dictionary<string, Java.Lang.Object>
                {
                    {"userId", participant.UserId},
                    {"raceId", participant.RaceId},
                };
                document.Update(userDocument);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}