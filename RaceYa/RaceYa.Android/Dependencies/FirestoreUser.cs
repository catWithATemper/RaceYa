using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaceYa.Helpers;
using Firebase.Firestore;
using Xamarin.Forms;
using Firebase.Firestore.Auth;
using RaceYa.Models;
using Java.Util;

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreUser))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreUser : Java.Lang.Object, IFirestoreUser
    {
        public bool Insert(Models.User user)
        {
            try
            {
                Dictionary<string, Java.Lang.Object> userDocument = new Dictionary<string, Java.Lang.Object>
            {
                {"name", user.Name },
                {"userId", user.UserId},
                
            };
                var collection = FirebaseFirestore.Instance.Collection("users");
                collection.Add(new HashMap(userDocument));
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