using RaceYa.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceYa.Helpers
{

    public interface IFirestoreRaceResultGPX
    {
        Task<string> Add(RaceResultGPX resultGPX, string particpantId, string resultId);
        Task<RaceResultGPX> ReadRaceResultGPXByParticipantAndResultIds(string participantId, string resultId);
        Task<bool> Update(RaceResultGPX resultGPX, string participantId, string resultId);
    }

    class FirestoreRaceResultGPX
    {
        private static IFirestoreRaceResultGPX firestoreRaceResultGPX = DependencyService.Get<IFirestoreRaceResultGPX>();

        public static async Task<string> Add(RaceResultGPX resultGPX, string participantId, string resultId)
        {
            return await firestoreRaceResultGPX.Add(resultGPX, participantId, resultId);
        }

        public static async Task<RaceResultGPX> ReadRaceResultGPXByParticipantAndResultIds(string participantId, string resultId)
        {
            return await firestoreRaceResultGPX.ReadRaceResultGPXByParticipantAndResultIds(participantId, resultId);
        }

        public static async Task<bool> Update(RaceResultGPX resultGPX, string participantId, string resultId)
        {
            return await firestoreRaceResultGPX.Update(resultGPX, participantId, resultId);
        }
    }
}
