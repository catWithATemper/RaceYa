using RaceYa.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async void SetUpNextParticipantContext()
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
