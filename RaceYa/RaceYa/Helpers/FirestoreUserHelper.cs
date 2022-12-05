using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceYa.Helpers
{

    public interface IFirestoreUser
    {
        Task<string> Add(User user);
    }

    public class FirestoreUser
    {
        private static IFirestoreUser firestoreUser = DependencyService.Get<IFirestoreUser>();

        public static async Task<string> Add(User user)
        {
            return await firestoreUser.Add(user);
        }
    }

}
