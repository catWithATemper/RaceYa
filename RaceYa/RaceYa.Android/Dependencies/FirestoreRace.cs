using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Interop;
using Java.Util;
using RaceYa.Helpers;
using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreRace))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreRace : Java.Lang.Object, IFirestoreRace //,IOnCompleteListener
    {  
        public FirestoreRace()
        {
            //races = new List<Race>();
        }

        public async Task<bool> Delete(Race race)
        {
            try
            {
                await CrossCloudFirestore.Current.Instance.Collection("racess").Document(race.Id).DeleteAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<String> Add(Race race)
        {
            try
            {
                var collectionReference = CrossCloudFirestore.Current.Instance.Collection("races");
                var documentReference = await collectionReference.AddAsync(race);
                //var documentReference = await CrossCloudFirestore.Current.Instance.Collection("races").AddAsync(race);

                race.Id = documentReference.Id;

                return documentReference.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<List<Race>> Read()
        {
            try
            {
                var group = await CrossCloudFirestore.Current.Instance.CollectionGroup("races").GetAsync();

                var races = group.ToObjects<Race>();

                return races.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> Update(Race race)
        {
            try
            {
                await CrossCloudFirestore.Current.Instance.Collection("races").Document(race.Id).UpdateAsync(race);
                return true;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<Race> ReadNextRace()
        {
            try
            {
                var query = await CrossCloudFirestore.Current.Instance.Collection("races").OrderBy("endDate").LimitTo(1).GetAsync();
                var races = query.ToObjects<Race>();

                return races.ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<Race> ReadRaceById(string id)
        {
            try
            {
                var document = await CrossCloudFirestore.Current.Instance.Collection("races").Document(id).GetAsync();
                var race = document.ToObject<Race>();

                return race;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<List<Race>> ReadRacesByUserId(string userId)
        {
            List<Race> enteredRaces = new List<Race>();
            try
            {
                IQuerySnapshot query = await CrossCloudFirestore.Current.Instance.Collection("participants")
                                                                                 .WhereEqualsTo("userId", userId)
                                                                                 .GetAsync();
                IEnumerable<Participant> participants = query.ToObjects<Participant>();

                foreach (Participant participant in participants)
                {
                    Race race = await ReadRaceById(participant.RaceId);
                    enteredRaces.Add(race);
                }
                return enteredRaces;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<List<Race>> ReadRacesForSigningUp(string userId)
        {
            List<Race> allRaces = await Read();
            List<Race> availableRaces = new List<Race>();

            try
            {
                foreach (Race race in allRaces)
                {
                    var query = await CrossCloudFirestore.Current.Instance.Collection("participants")
                                                                          .WhereEqualsTo("userId", userId)
                                                                          .WhereEqualsTo("raceId", race.Id)
                                                                          .LimitTo(1)
                                                                          .GetAsync();
                    var participants = query.ToObjects<Participant>();

                    if (participants.ToList().Count == 0)
                    {
                        availableRaces.Add(race);
                    }
                }
                return availableRaces;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}