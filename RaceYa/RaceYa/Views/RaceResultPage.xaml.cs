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
    public partial class RaceResultPage : ContentPage
    {

        RaceResult result = new RaceResult();
        public RaceResultPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = result;
            distanceLabel.Text = result.Distance.ToString();
            avgSpeedLabel.Text = result.AverageSpeed.ToString();

            //distanceLabel.BindingContext = result.Distance;
            //avgSpeedLabel.BindingContext = result.AverageSpeed;
        }


         async void OnButtonClicked(object sender, EventArgs e)
         {
            (sender as Button).Text = "Stop Clicking me!";
            await result.CalculateRaceResult();

            distanceLabel.SetValue(Label.TextProperty, result.Distance);
            avgSpeedLabel.SetValue(Label.TextProperty, result.AverageSpeed);


            //distanceLabel.Text = result.Distance.ToString();
            //avgSpeedLabel.Text = result.AverageSpeed.ToString();
         }

        



    }

}