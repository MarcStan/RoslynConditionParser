using System;

namespace RoslynConditionParser.Core
{
    public class Sensor
    {
        private readonly Func<int> _valueProvider;

        public Sensor(string sensorId, Func<int> valueProvider)
        {
            SensorId = sensorId;
            _valueProvider = valueProvider;
        }

        public string SensorId { get; set; }

        public int LastValue => _valueProvider();
    }
}
