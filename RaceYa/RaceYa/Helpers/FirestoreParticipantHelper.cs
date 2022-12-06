﻿using System;
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

        Task<Participant> ReadParticipantById(string id);
    }

    public class FirestoreParticipant
    {
        private static IFirestoreParticipant firestoreParticipant = DependencyService.Get<IFirestoreParticipant>();

        public static async Task<string> Add(Participant participant)
        {
            return await firestoreParticipant.Add(participant);
        }

        public static async Task<Participant> ReadParticipantById(string id)
        {
            return await firestoreParticipant.ReadParticipantById(id);
        }
    }
}
