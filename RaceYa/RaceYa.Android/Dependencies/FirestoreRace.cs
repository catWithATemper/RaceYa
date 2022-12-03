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

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreRace))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreRace : Java.Lang.Object, IFirestoreRace //,IOnCompleteListener
    {

        //List<Race> races;
        //bool hasReadRaces = false;


        RaceListener raceListener = new RaceListener();
        RaceListListener raceListListener = new RaceListListener();
        InsertListener insertListener = new InsertListener();

        public FirestoreRace()
        {
            //races = new List<Race>();
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

        public bool InsertOld(Race race)
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

        public async Task<String> Insert(Race race)
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
                collection.Add(new HashMap(raceDocument)).AddOnCompleteListener(insertListener);

                for (int i = 0; i < 50; i++)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    if (insertListener.hasInserted)
                        break;
                }


                return insertListener.documentId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public bool InsertWithCustomId(Race race, String id)
        {
            try
            {
                var raceCollection = FirebaseFirestore.Instance.Collection("races");
                var document = raceCollection.Document(id);
                Dictionary<string, Java.Lang.Object> userDocument = new Dictionary<string, Java.Lang.Object>
                {
                    {"routeLengthInKm", race.RouteLengthInKm },
                    {"startDate", race.StartDate.ToString("yyyy-MM-ddTHH:mm:ss")},
                    {"endDate", race.EndDate.ToString("yyyy-MM-ddTHH:mm:ss")},
                    {"description", race.Description},
                    {"userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid },
                };
                document.Update(userDocument);
                //raceCollection.Add(document);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /*
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
        */

        public async Task<List<Race>> Read()
        {
            try
            {
                //hasReadRaces = false;
                CollectionReference collection = FirebaseFirestore.Instance.Collection("races");
                //Query query = collection.WhereEqualTo("userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid);

                Query query = collection;
                query.Get().AddOnCompleteListener(raceListListener);

                for (int i = 0; i < 50; i++)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    if (raceListListener.hasReadRaces)
                        break;
                }

                return raceListListener.races;
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
                //hasReadRaces = false;
                CollectionReference collection = FirebaseFirestore.Instance.Collection("races");

                Query query = collection.OrderBy("endDate").Limit(1);
                query.Get().AddOnCompleteListener(raceListListener);

                for (int i = 0; i < 50; i++)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    if (raceListListener.hasReadRaces)
                        break;
                }

                return raceListListener.races[0];
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
                FirebaseFirestore.Instance.Collection("races")
                    .Document(id).Get().AddOnCompleteListener(raceListener);

                for (int i = 0; i < 50; i++)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    if (raceListener.hasReadRace)
                        break;
                }

                return raceListener.race;

                /*
                hasReadRaces = false;
                CollectionReference collection = FirebaseFirestore.Instance.Collection("races");

                Query query = collection.WhereEqualTo("id", id);
                query.Get().AddOnCompleteListener(this);

                for (int i = 0; i < 50; i++)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    if (hasReadRaces)
                        break;
                }
                return races[0];
                */
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }

    class RaceListener : Java.Lang.Object, IOnCompleteListener
    {
        public Race race;
        public bool hasReadRace = false;

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var doc = (DocumentSnapshot)task.Result;
                double routeLengthInKm;
                if (double.TryParse(doc.Get("routeLengthInKm").ToString(), out routeLengthInKm) == true)
                {
                    race = new Race()
                    {
                        RouteLengthInKm = routeLengthInKm,
                        StartDate = DateTime.Parse(doc.Get("startDate").ToString()),
                        EndDate = DateTime.Parse(doc.Get("endDate").ToString()),
                        Description = doc.Get("description").ToString(),
                        UserId = doc.Get("userId").ToString(),
                        Id = doc.Id
                    };
                }
                hasReadRace = true;
            }
        }
    }

    public class RaceListListener : Java.Lang.Object, IOnCompleteListener
    {
        public List<Race> races;
        public bool hasReadRaces = false;

        public RaceListListener()
        {
            races = new List<Race>();
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
    }

    class InsertListener : Java.Lang.Object, IOnCompleteListener
    {
        public String documentId;
        public bool hasInserted = false;

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var doc = (DocumentReference)task.Result;
                documentId = doc.Id;
                hasInserted = true;
            }
        }
    }

}