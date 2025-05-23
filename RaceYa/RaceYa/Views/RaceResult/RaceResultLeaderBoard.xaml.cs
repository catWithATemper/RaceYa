﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultLeaderBoard : ContentPage
    {
        public static GlobalContext Context = GlobalContext.Instance();

        public RaceResultLeaderBoard()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Context.LatestRace != null)
            {
                Context.LatestRace.CalculateObservableFinalLeaderBoard();
                leaderBoardView.ItemsSource = Context.LatestRace.ObservableFinalLeaderBoard;
            }
        }
    }
}