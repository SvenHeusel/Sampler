using System;
using System.Collections.Generic;
using Sampler.Container;
using Sampler.Contracts;

namespace Sampler.Processing
{
    public class MeasurementAccumulator : IMeasurementAccumulator
    {
        private readonly IGridCalculator _gridCalculator;

        public MeasurementAccumulator(IGridCalculator gridCalculator)
        {
            _gridCalculator = gridCalculator;
        }

        public IEnumerable<Measurement> CreateSampledMeasurements(IEnumerable<Measurement> inputMeasurements)
        {
            using (var enumerator = inputMeasurements.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return new List<Measurement>();

                var measurements = new List<Measurement>();

                while (true)
                {
                    var currentMeasurement = enumerator.Current;
                    var currentMeasurementGridPoint = GetCorrespondingGridPoint(currentMeasurement);
                    if (!enumerator.MoveNext())
                    {
                        UpdateAndAddMeasurement(measurements, currentMeasurement, currentMeasurementGridPoint);
                        break;
                    }

                    var nextMeasurement = enumerator.Current;
                    var nextMeasurementSamplingPoint = GetCorrespondingGridPoint(nextMeasurement);

                    if (currentMeasurementGridPoint.Equals(nextMeasurementSamplingPoint))
                        continue;

                    UpdateAndAddMeasurement(measurements, currentMeasurement, currentMeasurementGridPoint);
                }

                return measurements;
            }
        }

        private DateTime GetCorrespondingGridPoint(Measurement currentMeasurement)
        {
            return _gridCalculator.GetNextGridPoint(currentMeasurement.MeasurementTime);
        }

        private static void UpdateAndAddMeasurement(ICollection<Measurement> measurements, Measurement currentMeasurement, DateTime currentMeasurementSamplingPoint)
        {
            var updatedMeasurement = UpdateMeasurementTime(currentMeasurement, currentMeasurementSamplingPoint);
            measurements.Add(updatedMeasurement);
        }

        private static Measurement UpdateMeasurementTime(Measurement measurement, DateTime updatedMeasurementTime)
        {
            return new Measurement(updatedMeasurementTime, measurement.MeasurementValue, measurement.Type);
        }
    }
}