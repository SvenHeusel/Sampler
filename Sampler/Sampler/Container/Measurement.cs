using System;
using System.Globalization;
using Sampler.Enums;
using Sampler.Utilities;

namespace Sampler.Container
{
    public class Measurement
    {
        #region Backing Fields & Properties

        private const double Tolerance = 0.000001d;
        public DateTime MeasurementTime { get; }
        public double MeasurementValue { get; }
        public MeasurementType Type { get; }

        #endregion Backing Fields & Properties

        #region Construction Methods

        public Measurement(DateTime measurementTime, double measurementValue, MeasurementType type)
        {
            MeasurementTime = measurementTime;
            MeasurementValue = FormatDouble(measurementValue);
            Type = type;
        }

        #endregion Construction Methods

        #region Private Methods

        private double FormatDouble(double input)
        {
            return Math.Truncate(input * 100) / 100;
        }

        #endregion Private Methods

        #region Overrides

        public override bool Equals(object obj)
        {
            var comparator = obj as Measurement;
            return comparator != null && Equals(comparator);
        }

        public bool Equals(Measurement comparator)
        {
            return comparator != null &&
                   comparator.MeasurementTime.Equals(MeasurementTime) &&
                   Math.Abs(comparator.MeasurementValue - MeasurementValue) < Tolerance &&
                   comparator.Type == Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = (int)Globals.HashCodeBase;
                hash = (hash * Globals.HashCodeMultiplier) ^ MeasurementTime.GetHashCode();
                hash = (hash * Globals.HashCodeMultiplier) ^ MeasurementValue.GetHashCode();
                hash = (hash * Globals.HashCodeMultiplier) ^ Type.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            var timeString = MeasurementTime.ToString(Globals.DateTimeFormat);
            var typeDescription = Type.GetDescription();
            //var valueString = $"{MeasurementValue:N2}";
            var valueString = MeasurementValue.ToString("0,0.00", new CultureInfo("en-US", false));
            return $"{{{timeString}, {typeDescription}, {valueString}}}";
        }

        #endregion Overrides
    }
}