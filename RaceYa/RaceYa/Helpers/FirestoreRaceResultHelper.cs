﻿using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceYa.Helpers
{
    public interface IFirestoreRaceResult
    {
        Task<string> Add(RaceResult result, string particpantId);
        Task<RaceResult> ReadRaceResultByParticipantId(string participantId);
    }

    public class FirestoreRaceResult
    {
        private static IFirestoreRaceResult firestoreRaceResult = DependencyService.Get<IFirestoreRaceResult>();

        public static async Task<string> Add(RaceResult result, string participantId)
        {
            return await firestoreRaceResult.Add(result, participantId);
        }

        public static async Task<RaceResult> ReadRaceRaesultByParticipantId(string participantId)
        {
            return await firestoreRaceResult.ReadRaceResultByParticipantId(participantId);
        }
    }
}
