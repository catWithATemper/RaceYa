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
        Task<bool> Update(RaceResult result, string participantId);
        Task<List<RaceResult>> Read();
        Task<bool> Delete(RaceResult result, string participantId);
        Task<RaceResult> ReadLatestRaceResult(string userId);
        Task<List<RaceResult>> ReadAllResultsForUser(string userId);
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

        public static async Task<bool> Update(RaceResult result, string participantId)
        {
            return await firestoreRaceResult.Update(result, participantId);
        }

        public static async Task<List<RaceResult>> Read()
        {
            return await firestoreRaceResult.Read();
        }

        public static async Task<bool> Delete(RaceResult result, string participantId)
        {
            return await firestoreRaceResult.Delete(result, participantId);
        }

        public static async Task<RaceResult> ReadLatestRaceResult(string userId)
        {
            return await firestoreRaceResult.ReadLatestRaceResult(userId);
        }

        public static async Task<List<RaceResult>> ReadAllResultsForUser(string userId)
        {
            return await firestoreRaceResult.ReadAllResultsForUser(userId);
        }
    }
}
