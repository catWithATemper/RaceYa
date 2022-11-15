using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewRacePage : ContentPage
    {
        public CreateNewRacePage()
        {
            InitializeComponent();

            startDatePicker.MinimumDate = DateTime.Today;
            startDatePicker.MaximumDate = DateTime.Today.AddMonths(1);

            endDatePicker.MinimumDate = DateTime.Today;
            endDatePicker.MaximumDate = startDatePicker.Date.AddMonths(1);
        }

        private async void routeLengthEntry_Completed(object sender, System.EventArgs e)
        {
            double routeLength = 0;
            if(double.TryParse(routeLengthEntry.Text, out routeLength) == true)
            {
                double.TryParse(routeLengthEntry.Text, out routeLength);

                if (routeLength < 1 || routeLength > 50)
                {
                    await DisplayAlert("Input error", "Specify a numeric value between 1 and 50 km for the root length", "OK");
                }
            }
            else
            {
                await DisplayAlert("Input error", "Specify a numeric value between 1 and 50 km for the root length", "OK");
            }
        }

        private void startDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void endDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}