using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    public class LocationServiceManager
    {
        CancellationTokenSource cts;

        public async Task<Location> GetTestLocation()
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(4));
            cts = new CancellationTokenSource();

            Location location = null;
            bool locationIsValid = false;

            while (locationIsValid == false)
            {
                location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location == null || location.Accuracy > 5)
                {
                    locationIsValid = false;
                }
                else
                {
                    locationIsValid = true;
                }
            }
            return location;
        }

        public async Task<Location> GetCurrentLocation()
        {
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(4));
            cts = new CancellationTokenSource();

            int accuracy = 5;

            Location location = null;
            bool locationIsValid = false;

            while (locationIsValid == false)
            {
                location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location == null || location.Accuracy > accuracy)
                {
                    locationIsValid = false;
                    accuracy++;
                }
                else
                {
                    locationIsValid = true;
                }
            }
            return location;
        }
    }
}
