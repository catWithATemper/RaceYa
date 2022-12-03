using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceYa.Helpers
{
    public interface IFirestoreRace
    {
        Task<String> Insert(Race race);
        Task<bool> Update(Race race);
        Task<bool> Delete(Race race);
        Task<List<Race>> Read();
        Task<Race> ReadNextRace();
        Task<Race> ReadRaceById(string id);
        bool InsertWithCustomId(Race race, String id);
    }

    public class FirestoreRace
    {
        private static IFirestoreRace firestoreRace = DependencyService.Get<IFirestoreRace>();

        public static async Task<string> Insert(Race race)
        {
            return await firestoreRace.Insert(race);
        }

        public static async Task<bool> Update(Race race)
        {
            return await firestoreRace.Update(race);
        }

        public static async Task<bool> Delete(Race race)
        {
            return await firestoreRace.Delete(race);
        }

        public static async Task<List<Race>> Read()
        {
            return await firestoreRace.Read();
        }

        public static async Task<Race> ReadNextRace()
        {
            return await firestoreRace.ReadNextRace();
        }

        public static async Task<Race> ReadRaceById(string id)
        {
            return await firestoreRace.ReadRaceById(id);
        }

        public static bool InsertWithCustomId(Race race, String id)
        {
            return firestoreRace.InsertWithCustomId(race, id);
        }
    }
}

