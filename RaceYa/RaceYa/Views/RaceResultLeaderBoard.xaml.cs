using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RaceYa.Models;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaceResultLeaderBoard : ContentPage
    {
        public static DataExchangeService Service = DataExchangeService.Instance();

        public RaceResultLeaderBoard()
        {
            InitializeComponent();

            leaderBoardView.ItemsSource = Service.CurrentRace.ObservableFinalLeaderBoard;


        }
    }
}