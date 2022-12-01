using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RaceYa.Helpers
{

    public interface IFirestoreUser
    {
        bool Insert(User user);
    }

    public class FirestoreUser
    {
        private static IFirestoreUser firestoreUser = DependencyService.Get<IFirestoreUser>();

        public static bool Insert(User user)
        {
            return firestoreUser.Insert(user);
        }
    }

}
