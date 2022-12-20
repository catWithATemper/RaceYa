using RaceYa.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaceYa.Models
{
    public class GlobalContext
    {
        public static GlobalContext instance = null;
        public static GlobalContext Instance()
        {
            if (instance == null)
                instance = new GlobalContext();
            return instance;
        }

        public bool UserIsAuthenticated = false;

        public Race CurrentRace { get; set; }

        public User CurrentUser { get; set; }

        public Participant CurrentParticipant { get; set; }
        public RaceResult CurrentParticipantResult { get; set; }

        public RaceResult LatestResult { get; set; }

        public async Task LoadRaceData(Race race)
        {
            List<User> users = await FirestoreUser.Read();
            foreach (User user in users)
            {
                Participant participant = await FirestoreParticipant.ReadParticpantByUserAndRace(user.Id, race.Id);
                if (participant != null)
                {
                    participant.AssignRace(race);
                    participant.AssignUser(user);
                    RaceResult result = await FirestoreRaceResult.ReadRaceRaesultByParticipantId(participant.Id);
                    result.RaceParticipant = participant;
                    participant.Result = result;
                    participant.AddToParticipantsList(race);
                    participant.AddToRaceLeaderboard(race);

                    RaceResultGPX resultGPX = await FirestoreRaceResultGPX.ReadRaceResultGPXByParticipantAndResultIds(participant.Id, result.Id);
                }
            }
        }

        public async Task SetUpNextParticipantContext()
        {
            CurrentParticipant = await FirestoreParticipant.ReadParticpantByUserAndRace(CurrentUser.Id, CurrentRace.Id);
            if (CurrentParticipant != null)
            {
                CurrentParticipant.AssignRace(CurrentRace);
                CurrentParticipant.AssignUser(CurrentUser);

                CurrentParticipantResult = new RaceResult(CurrentParticipant);

                CurrentParticipantResult.RaceParticipant = CurrentParticipant;
                CurrentParticipant.Result = CurrentParticipantResult;
                CurrentParticipantResult.GPXRequired = true;

                foreach (Participant participant in CurrentRace.Participants)
                {
                    if (participant.Id == CurrentParticipant.Id)
                    {
                        CurrentRace.Participants.Remove(participant);
                        CurrentRace.Participants.Add(CurrentParticipant);
                        break;
                    }
                }

                foreach (Participant participant in CurrentRace.LeaderBoard)
                {
                    if (participant.Id == CurrentParticipant.Id)
                    {
                        CurrentRace.LeaderBoard.Remove(participant);
                        CurrentRace.LeaderBoard.Add(CurrentParticipant);
                        break;
                    }
                }

                CurrentRace.CurrentParticipant = CurrentParticipant;
                CurrentParticipant.IsCurrentParticipant = true;
            }
        }
    }
}
