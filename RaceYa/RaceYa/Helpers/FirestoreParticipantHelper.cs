using System;
using System.Collections.Generic;
using System.Text;
using RaceYa.Models;
using Xamarin.Forms;

namespace RaceYa.Helpers
{

    public interface IFirestoreParticipant
    {
        bool Insert(Participant participant);
    }

    public class FirestoreParticipant
    {
        private static IFirestoreParticipant firestoreParticipant = DependencyService.Get<IFirestoreParticipant>();

        public static bool Insert(Participant participant)
        {
            return firestoreParticipant.Insert(participant);
        }
    }
}
