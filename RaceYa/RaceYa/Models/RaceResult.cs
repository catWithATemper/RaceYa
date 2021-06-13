using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RaceYa.Models
{
    class RaceResult
    {
        int RouteLength { get; }

        public List<Location> LocationReadings = new List<Location>();

        //Tracker ResultTracker = new Tracker();

        public double Distance { get; set; }
        public double AverageSpeed { get; set; }

        CancellationTokenSource cts;

        public RaceResult()
        {
            RouteLength = 100;

            Distance = 0;

            //CalculateRaceResult();
        }

        public async Task CalculateRaceResult()
        {
            Location currentLocation = await GetCurrentLocation();

            Location startingPoint = currentLocation;
            DateTime StartTime = startingPoint.Timestamp.DateTime;

            Location previousLocation;

            while (Distance <= RouteLength)
            {
                previousLocation = currentLocation;
                //await Task.Delay(1000);
                currentLocation = await GetCurrentLocation();
                Distance += Location.CalculateDistance(currentLocation, previousLocation, DistanceUnits.Kilometers) * 1000;

                TimeSpan timeSinceStart = currentLocation.Timestamp.DateTime - StartTime;

                AverageSpeed = Distance / timeSinceStart.TotalSeconds;
                Console.WriteLine("Distance " + Distance + " Speed " + AverageSpeed);
            }
        }

        public async Task<Location> GetCurrentLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            cts = new CancellationTokenSource();
            var location = await Geolocation.GetLocationAsync(request, cts.Token);

            return location;
        }
    }
}
