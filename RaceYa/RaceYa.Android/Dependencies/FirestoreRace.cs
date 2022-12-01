using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Util;
using RaceYa.Helpers;
using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreRace))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreRace : Java.Lang.Object, IFirestoreRace, IOnCompleteListener
    {

        List<Race> races;
        bool hasReadRaces = false;

        public FirestoreRace()
        {
            races = new List<Race>();
        }

        public async Task<bool> Delete(Race race)
        {
            try
            {
                var collection = FirebaseFirestore.Instance.Collection("races");
                collection.Document(race.Id).Delete();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Insert(Race race)
        {
            try
            {
                Dictionary<string, Java.Lang.Object> raceDocument = new Dictionary<string, Java.Lang.Object>
            {
                {"routeLengthInKm", race.RouteLengthInKm },
                {"startDate", race.StartDate.ToString("yyyy-MM-ddTHH:mm:ss")},
                {"endDate", race.EndDate.ToString("yyyy-MM-ddTHH:mm:ss")},
                {"description", race.Description},
                {"userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid },
            };
                var collection = FirebaseFirestore.Instance.Collection("races");
                collection.Add(new HashMap(raceDocument));
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                QuerySnapshot documents = (QuerySnapshot)task.Result;

                races.Clear();
                foreach (DocumentSnapshot doc in documents.Documents)
                {
                    double routeLengthInKm;
                    if (double.TryParse(doc.Get("routeLengthInKm").ToString(), out routeLengthInKm) == true)
                    {
                        Race newRace = new Race()
                        {
                            RouteLengthInKm = routeLengthInKm,
                            StartDate = DateTime.Parse(doc.Get("startDate").ToString()),
                            EndDate = DateTime.Parse(doc.Get("endDate").ToString()),
                            Description = doc.Get("description").ToString(),
                            UserId = doc.Get("userId").ToString(),
                            Id = doc.Id
                        };
                        races.Add(newRace);
                    }
                }
            }
            else
            {
                races.Clear();
            }
            hasReadRaces = true;
        }


        public async Task<List<Race>> Read()
        {
            try
            {
                hasReadRaces = false;
                CollectionReference collection = FirebaseFirestore.Instance.Collection("races");
                //Query query = collection.WhereEqualTo("userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid);

                Query query = collection;
                query.Get().AddOnCompleteListener(this);

                for (int i = 0; i < 50; i++)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    if (hasReadRaces)
                        break;
                }

                return races;
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
                Dictionary<string, Java.Lang.Object> raceDocument = new Dictionary<string, Java.Lang.Object>
            {
                {"routeLengthInKm", race.RouteLengthInKm },
                {"startDate", race.StartDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss")},
                {"endDate", race.EndDate.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss")},
                {"description", race.Description},
                {"userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid },
            };
                var collection = FirebaseFirestore.Instance.Collection("races");
                collection.Document(race.Id).Update(raceDocument);
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
                hasReadRaces = false;
                CollectionReference collection = FirebaseFirestore.Instance.Collection("races");

                Query query = collection.OrderBy("endDate").Limit(1);
                query.Get().AddOnCompleteListener(this);

                for (int i = 0; i < 50; i++)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    if (hasReadRaces)
                        break;
                }

                return races[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}