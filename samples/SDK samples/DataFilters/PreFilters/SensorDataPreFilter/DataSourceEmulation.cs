using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TotalApi.Telematics;
using TotalApi.Utils.Extensions;
using TotalApi.Utils.Wcf.Extensions;

namespace SensorDataPreFilter
{
    public class DataSourceEmulation
    {
        public static SensorValues LoadSensorValues(string source, bool fillDateTime = false, int sensorType = 0, int sensorNumber = 0)
        {
            var text = Assembly.GetCallingAssembly().GetResourceAllText($"sensor.{source}.txt");
            var sensorData = Series(text, ';', fillDateTime, sensorType, sensorNumber);
            var res = new SensorValues(new SourceInfo("test") { SensorType = sensorType, SensorNumber = sensorNumber }, sensorData);
            return res;
        }

        private static IEnumerable<SensorData> Series(string source, char separator = ',', bool fillDateTime = false, int sensorType = 0, int sensorNumber = 0)
        {
            source = source?.Replace("\r\n", separator.AsString());
            source = source?.Replace("\r", separator.AsString());
            source = source?.Replace("\n", separator.AsString());
            //var now = DateTime.Today;
            var now = DateTime.Today;
            var c = 0;
            return source == null
                ? null : source == ""
                ? Enumerable.Empty<SensorData>() : source.Split(separator)
                .Where(s => s.IsNotEmpty(true))
                .Select(s =>
                {
                    var value = s.AsDouble();
                    var time = fillDateTime ? now.AddSeconds(c++) : DateTime.MinValue;
                    var pair = s.Replace("\t", " ").Split(' ');
                    if (pair.Length > 1)
                    {
                        time = now.Date + TimeSpan.Parse(pair[0]);
                        value = pair[1].AsDouble();
                        now = time;
                        c = 1;
                    }
                    var res = new SensorData { Value = value, DataType = SensorDataType.Float, UtcDate = time, SensorType = sensorType, Number = sensorNumber };
                    res = res.ToJson().FromJson<SensorData>();
                    return res;
                });
        }
    }
}
