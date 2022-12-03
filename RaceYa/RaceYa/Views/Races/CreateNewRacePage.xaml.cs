using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using RaceYa.Models;
using RaceYa.Helpers;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewRacePage : ContentPage
    {
        //TODO: Add hours and minutes to the race end date, so that the race ends at
        // 23:59 and not at 00:00

        public static DataExchangeService Service = DataExchangeService.Instance();

        public double RouteLength;

        public DateTime StartDate = DateTime.Today;

        public DateTime EndDate = DateTime.Today;

        public string Description;

        public CreateNewRacePage()
        {
            InitializeComponent();

            startDatePicker.MinimumDate = DateTime.Today;
            startDatePicker.MaximumDate = DateTime.Today.AddMonths(1);

            endDatePicker.MinimumDate = DateTime.Today;
            endDatePicker.MaximumDate = StartDate.AddMonths(1);
        }

        private async void routeLengthEntry_Completed(object sender, EventArgs e)
        {
            await ValidateRouteLength();
        }

        private async void startDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            await ValidateStartDate();
        }

        private async void endDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            await ValidateEndDate();
        }

        private async void descriptionEditor_Completed(object sender, EventArgs e)
        {
            await ValidateDescription();
        }

        private async Task<bool> ValidateRouteLength()
        {
            ///Route length in km
            double routeLength = 0;
            if (double.TryParse(routeLengthEntry.Text, out routeLength) == true)
            {
                double.TryParse(routeLengthEntry.Text, out routeLength);

                if (routeLength < 1 || routeLength > 50)
                {
                    await DisplayAlert("Input error", "Specify a numeric value between 1 and 50 km for the root length", "OK");
                    return false;
                }
                else
                {
                    RouteLength = routeLength;
                    return true;
                }
            }
            else
            {
                await DisplayAlert("Input error", "Specify a numeric value between 1 and 50 km for the root length", "OK");
                return false;    
            }
        }

        private async Task<bool> ValidateStartDate()
        {
            DateTime startDate = startDatePicker.Date;

            if (startDate < startDatePicker.MinimumDate)
            {
                await DisplayAlert("Input error", "The earliest possible start date is today", "Ok");
                return false;
            }
            else if (startDate > startDatePicker.MaximumDate)
            {
                await DisplayAlert("Input error", "The latest possible end date is in one month", "Ok");
                return false;
            }
            else
            {
                StartDate = startDate;

                endDatePicker.MinimumDate = StartDate;
                endDatePicker.MaximumDate = StartDate.AddMonths(1);

                return true;
            }
        }

        private async Task<bool> ValidateEndDate()
        {
            DateTime endDate = endDatePicker.Date;

            if (endDate < endDatePicker.MinimumDate)
            {
                await DisplayAlert("Input error", "The earliest possible end date is today", "Ok");
                return false;
            }
            else if (endDate > endDatePicker.MaximumDate)
            {
                await DisplayAlert("Input error", "The latest possible end date is one month from the start date", "Ok");
                return false;
            }
            else if (endDate < startDatePicker.Date)
            {
                await DisplayAlert("Input error", "The latest possible end date is one month from the start date", "Ok");
                return false;
            }
            else
            {
                EndDate = endDate;
                return true;
            }
        }

        private async Task<bool> ValidateDescription()
        {
            if (string.IsNullOrEmpty(descriptionEditor.Text))
            {
                await DisplayAlert("Input Error", "Write a short description", "Ok");
                return false;
            } 
            else if (descriptionEditor.Text.Length > 50)
            {
                await DisplayAlert("Description too long", "Maximum text length 50 characters", "OK");
                return false;    
            }
            else
            {
                Description = descriptionEditor.Text;
                return true;
            }
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            //routelength in km
            bool ValidateRouteLengthResult = await ValidateRouteLength();
            bool ValidateStartDateResult = await ValidateStartDate();
            bool ValidateEndDateResult = await ValidateEndDate();
            bool ValidateDesscriptionResult = await ValidateDescription();

            if (ValidateRouteLengthResult && ValidateStartDateResult && ValidateEndDateResult && ValidateDesscriptionResult)
            {
                await saveRace(RouteLength, StartDate, EndDate, Description);

                Service.Races.Add(new Race(RouteLength, StartDate, EndDate, Description));
            }

            routeLengthEntry.Text = "";
            descriptionEditor.Text = "";

            var tabbedPage = this.Parent as TabbedPage;
            tabbedPage.CurrentPage = tabbedPage.Children[0];
        }


        private async Task saveRace(double routelength, DateTime startDate, DateTime endDate, string description)
        {
            //route length in km
            try
            {
                Race newRace = new Race()
                {
                    RouteLengthInKm = routelength,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = description,
                };

                string id = await FirestoreRace.Insert(newRace);
                if (id != null)
                {
                    await DisplayAlert("Success", "Race saved", "Ok");
                }
                else
                    await DisplayAlert("Failure", "Race was not saved, please try again", "Ok");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}