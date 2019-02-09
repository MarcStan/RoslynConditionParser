using System.Linq;

namespace RoslynConditionParser.Core
{
    public static class ScriptFunctions
    {
        /// <summary>
        /// A bit ugly, but only way to provide state to static
        /// functions via roslyn that I have found.
        /// </summary>
        public static Globals Globals { get; set; }

        public static int sensor(string sensorId)
        {
            var sensorValue = GetSensor(sensorId).LastValue;
            return sensorValue;
        }

        private static Sensor GetSensor(string sensorId)
        {
            return Globals.Sensors.Single(s => s.SensorId == sensorId);
        }
    }
}
