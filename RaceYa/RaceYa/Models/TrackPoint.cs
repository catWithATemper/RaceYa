using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    class TrackPoint
    {
        public double latitude;
        public double longitude;
        public DateTime TimeStamp;

        public TrackPoint(double lat, double lon, DateTime time)
        {
            latitude = lat;
            longitude = lon;
            TimeStamp = time;
        }

        public override string ToString()
        {
            return "Lat " + latitude + " Lon " + longitude + " Time " + TimeStamp;
        }
    }
}
