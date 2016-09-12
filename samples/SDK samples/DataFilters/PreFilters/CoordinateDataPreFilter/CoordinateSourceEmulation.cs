using System;
using System.Collections.Generic;
using System.Linq;
using TotalApi.Telematics;
using TotalApi.Utils.Spatials;
using TotalApi.Utils.Wcf.Extensions;

namespace CoordinateDataPreFilter
{
    public class CoordinateSourceEmulation
    {
        public static IEnumerable<Coordinate> LoadCoordsValues(string source)
        {
            var path = $"gpx.{source}.gpx";
            var gpx = GpxParser.LoadGpxTracksFromResource(path);
            return gpx.Select(c =>
            {
                var coord = new Coordinate(c.UtcDate, c.LatitudeDegree * Math.PI / 180.0, c.LongitudeDegree * Math.PI / 180.0, c.Direction, c.Velocity, c.Altitude);
                // В БД пишутся округлённые значения данных. Эти округления выполняются при сериализации данных. 
                coord = coord.ToJson().FromJson<Coordinate>();
                return coord;
            });
        }
    }
}
