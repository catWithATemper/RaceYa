using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceYa.Helpers
{
    public interface IFirestore
    {
        bool Insert(Race race);
        Task<bool> Update(Race race);
        Task<bool> Delete(Race race);
        Task<List<Race>> Read();
        Task<Race> ReadNextRace();
    }

    public class Firestore
    {
        private static IFirestore firestore = DependencyService.Get<IFirestore>();

        public static bool Insert(Race race)
        {
            return firestore.Insert(race);
        }

        public static async Task<bool> Update(Race race)
        {
            return await firestore.Update(race);
        }

        public static async Task<bool> Delete(Race race)
        {
            return await firestore.Delete(race);
        }

        public static async Task<List<Race>> Read()
        {
            return await firestore.Read();
        }

        public static async Task<Race> ReadNextRace()
        {
            return await firestore.ReadNextRace();
        }
    }
}

