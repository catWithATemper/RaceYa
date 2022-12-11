using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaceYa.Models
{
    public class RaceResultGPX
    {
        [Id]
        public string Id { get; set; }

        [MapTo("track")]
        public Track Track { get; set; }

        public RaceResultGPX()
        {
            Track = new Track();
        }
    }

    public class Track
    {
        [MapTo("TrackSegment")]
        public TrackSegment TrackSegment { get; set; }

        public Track()
        {
            TrackSegment = new TrackSegment();
        }
    }

    public class TrackSegment
    {
        [MapTo("trackPoints")]
        public List<TrackPoint> TrackPoints { get; set; }

        public TrackSegment()
        {
            TrackPoints = new List<TrackPoint>();
        }
    }

    public class TrackPoint
    {
        [MapTo("coordinates")]
        public GeoPoint Coordinates { get; set; }

        [MapTo("time")]
        public DateTime Time { get; set; }
    }
}
