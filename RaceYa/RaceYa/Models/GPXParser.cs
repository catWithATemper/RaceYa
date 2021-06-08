using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;


namespace RaceYa.Models
{
    class GPXParser
    {
        public List<TrackPoint> LocationReadings = new List<TrackPoint>();

        public GPXParser()
        {
            LocationReadings = ParseData();
        }

        private List<TrackPoint> ParseData()
        {
            List<TrackPoint> Readings = new List<TrackPoint>();

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(GPXParser)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("RaceYa.Sample_Data.20210526_173957.gpx");
            using (var reader = new System.IO.StreamReader(stream))
            {
                XDocument gpxFile = XDocument.Load(reader);
                XNamespace ns = "http://www.topografix.com/GPX/1/1";
                XElement gpx = gpxFile.Element(ns + "gpx");
                XElement trk = gpx.Element(ns + "trk");

                DateTime time = new DateTime();

                foreach (XElement trkseg in trk.Elements())
                {
                    foreach (XElement trkpt in trkseg.Elements())
                    {
                        double lat = (double)trkpt.Attribute("lat");
                        double lon = (double)trkpt.Attribute("lon");
                        time = (DateTime.Parse(trkpt.Element(ns + "time").Value));
                        TrackPoint LocationReading = new TrackPoint(lat, lon, time);
                        Readings.Add(LocationReading);
                    }
                }
                Console.WriteLine(Readings.Count);
                return Readings;
            }
        }
    }
}
