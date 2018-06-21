using System;
using System.Collections.Generic;
using System.Linq;
using Sampler.Container;
using Sampler.Contracts;
using Sampler.Enums;
using Sampler.Utilities;

namespace Sampler.Processing
{
    public class MeasurementPrinter
    {
        private readonly IPrinter _printer;

        public MeasurementPrinter(IPrinter printer)
        {
            _printer = printer;
        }

        public void PrintMeasurementsByMeasurementType(Dictionary<MeasurementType, IEnumerable<Measurement>> mappedMeasurements)
        {
            foreach (var measurementMap in mappedMeasurements)
            {
                var measurementType = measurementMap.Key;
                var associatedMeasurements = measurementMap.Value;
                PrintMeasurements(measurementType, associatedMeasurements);
            }
        }

        public void PrintMeasurementsByMeasurementTime(Dictionary<MeasurementType, IEnumerable<Measurement>> mappedMeasurements)
        {
            var uniqueMeasurementTimes = GetUniqueMeasurementTimes(mappedMeasurements);
            foreach (var measurementTime in uniqueMeasurementTimes)
            {
                var associatedMeasurements = GetAssociatedMeasurements(measurementTime, mappedMeasurements);
                PrintMeasurements(measurementTime, associatedMeasurements);
            }
        }

        private static IEnumerable<Measurement> GetAssociatedMeasurements(DateTime measurementTime, Dictionary<MeasurementType, IEnumerable<Measurement>> mappedMeasurements)
        {
            var allMeasurements = GetAllMeasurements(mappedMeasurements);
            return allMeasurements.Where(measurement => measurement.MeasurementTime.Equals(measurementTime));
        }

        private static IEnumerable<DateTime> GetUniqueMeasurementTimes(Dictionary<MeasurementType, IEnumerable<Measurement>> mappedMeasurements)
        {
            var allMeasurements = GetAllMeasurements(mappedMeasurements);
            return new HashSet<DateTime>(allMeasurements.Select(measurement => measurement.MeasurementTime));
        }

        private void PrintMeasurements(MeasurementType measurementType, IEnumerable<Measurement> sampledMeasurements)
        {
            Print($"Measurement Type: {measurementType.GetDescription()}");
            foreach (var measurement in sampledMeasurements)
            {
                Print($"\t{measurement}");
            }
        }

        private void PrintMeasurements(DateTime samplingPoint, IEnumerable<Measurement> associatedMeasurements)
        {
            Print($"Measurement Time: {samplingPoint.ToString(Globals.DateTimeFormat)}");
            foreach (var measurement in associatedMeasurements)
            {
                Print($"\t{measurement}");
            }
        }

        private void Print(string stringToPrint)
        {
            _printer.Print(stringToPrint);
        }

        private static IEnumerable<Measurement> GetAllMeasurements(Dictionary<MeasurementType, IEnumerable<Measurement>> mappedMeasurements)
        {
            return mappedMeasurements.SelectMany(keyValuePair => keyValuePair.Value);
        }
    }
}