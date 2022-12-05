using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RaceYa.Models;
using Xamarin.Forms;

namespace RaceYa.Helpers
{

    public interface IFirestoreParticipant
    {
        Task<string> Add(Participant participant);
    }

    public class FirestoreParticipant
    {
        private static IFirestoreParticipant firestoreParticipant = DependencyService.Get<IFirestoreParticipant>();

        public static async Task<string> Add(Participant participant)
        {
            return await firestoreParticipant.Add(participant);
        }
    }
}
