﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using RaceYa.Helpers;
using RaceYa.Models;
using Plugin.CloudFirestore;
using System.Threading.Tasks;

[assembly: Dependency(typeof(RaceYa.Droid.Dependencies.FirestoreRaceResult))]
namespace RaceYa.Droid.Dependencies
{
    class FirestoreRaceResult : IFirestoreRaceResult
    {

        public async Task<string> Add(RaceResult result, string participantId)
        {
            try
            {
                var documentReference = await CrossCloudFirestore.Current.Instance
                                                                 .Collection("participants")
                                                                 .Document(participantId)
                                                                 .Collection("raceResults")
                                                                 .AddAsync(result);
                result.Id = documentReference.Id;

                return documentReference.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> Update(RaceResult result, string participantId)
        {
            try
            {
                await CrossCloudFirestore.Current.Instance.Collection("participants")
                                                          .Document(participantId)
                                                          .Collection("raceResults")
                                                          .Document(result.Id)
                                                          .UpdateAsync(result);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<RaceResult> ReadRaceResultByParticipantId(string participantId)
        {
            try
            {
                var query = await CrossCloudFirestore.Current.Instance.Collection("participants")
                                                                      .Document(participantId)
                                                                      .Collection("raceResults")
                                                                     .LimitTo(1).GetAsync();

                var results = query.ToObjects<RaceResult>();

                return results.ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

    }
}