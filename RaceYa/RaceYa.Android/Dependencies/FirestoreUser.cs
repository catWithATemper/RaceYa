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
using Plugin.CloudFirestore;
using System.Threading.Tasks;

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreUser))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreUser : IFirestoreUser //,Java.Lang.Object 
    {
        public async Task<string> Add(Models.User user)
        {
            try
            {
                var documentReference = await CrossCloudFirestore.Current.Instance.Collection("users").AddAsync(user);

                user.Id = documentReference.Id;

                return documentReference.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<Models.User> ReadUserById(string id)
        {
            try
            {
                var document = await CrossCloudFirestore.Current.Instance.Collection("users").Document(id).GetAsync();
                var user = document.ToObject<Models.User>();

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<Models.User> ReadUserByUserId(string uId)
        {
            try
            {
                var query = await CrossCloudFirestore.Current.Instance.Collection("users").WhereEqualsTo("userId", uId).LimitTo(1).GetAsync();
                var users = query.ToObjects<Models.User>();

                return users.ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<List<Models.User>> Read()
        {
            try
            {
                var group = await CrossCloudFirestore.Current.Instance.CollectionGroup("users").GetAsync();

                var users = group.ToObjects<Models.User>();

                return users.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}