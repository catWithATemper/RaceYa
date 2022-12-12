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
        Task<User> ReadUserById(string id);
        Task<User> ReadUserByUserId(string uId);
        Task<List<User>> Read();

    }

    public class FirestoreUser
    {
        private static IFirestoreUser firestoreUser = DependencyService.Get<IFirestoreUser>();

        public static async Task<string> Add(User user)
        {
            return await firestoreUser.Add(user);
        }

        public static async Task<User> ReadUserById(string id)
        {
            return await firestoreUser.ReadUserById(id);
        }

        public static async Task<User> ReadUserByUserId(string uId)
        {
            return await firestoreUser.ReadUserByUserId(uId);
        }

        public static async Task<List<User>> Read()
        {
            return await firestoreUser.Read();
        }
    }
}
