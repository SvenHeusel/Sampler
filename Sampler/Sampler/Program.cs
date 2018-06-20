using System;
using System.Collections.Generic;
using System.Linq;
using Sampler.Container;
using Sampler.Contracts;
using Sampler.Enums;
using Sampler.Processing;
using Sampler.Utilities;

namespace Sampler
{
    internal class Program
    {
        private static IMeasurementSorter _measurementSorter;
        private static GridCalculator _gridCalculator;
        private static MeasurementAccumulator _measurementAccumulator;

        private static void Main(string[] args)
        {
            _measurementSorter = new MeasurementSorter();
            _gridCalculator = new GridCalculator(Globals.SamplingGridMinutes);
            _measurementAccumulator = new MeasurementAccumulator(_gridCalculator);

            var inputData = GenerateInputData();
            SampleDataDriven(inputData);
            Console.ReadLine();
        }

        private static IEnumerable<Measurement> GenerateInputData()
        {
            var firstMeasurementTime = new DateTime(2017, 1, 3, 10, 4, 45);
            var secondMeasurementTime = new DateTime(2017, 1, 3, 10, 1, 18);
            var thirdMeasurementTime = new DateTime(2017, 1, 3, 10, 9, 7);
            var fourthMeasurementTime = new DateTime(2017, 1, 3, 10, 3, 34);
            var fifthMeasurementTime = new DateTime(2017, 1, 3, 10, 2, 1);
            var sixthMeasurementTime = new DateTime(2017, 1, 3, 10, 5, 0);
            var seventhMeasurementTime = new DateTime(2017, 1, 3, 10, 5, 1);

            var firstMeasurement = new Measurement(firstMeasurementTime, 35.79d, MeasurementType.Temperature);
            var secondMeasurement = new Measurement(secondMeasurementTime, 98.78d, MeasurementType.SpO2);
            var thirdMeasurement = new Measurement(thirdMeasurementTime, 35.01d, MeasurementType.Temperature);
            var fourthMeasurement = new Measurement(fourthMeasurementTime, 96.49d, MeasurementType.SpO2);
            var fifthMeasurement = new Measurement(fifthMeasurementTime, 35.82d, MeasurementType.Temperature);
            var sixthMeasurement = new Measurement(sixthMeasurementTime, 97.17d, MeasurementType.SpO2);
            var seventhMeasurement = new Measurement(seventhMeasurementTime, 95.08d, MeasurementType.SpO2);

            return new List<Measurement>() { firstMeasurement, secondMeasurement,thirdMeasurement,
                fourthMeasurement, fifthMeasurement, sixthMeasurement, seventhMeasurement };
        }

        private static void SampleDataDriven(IEnumerable<Measurement> unsampledMeasurements)
        {
            var chronologicalMeasurements = OrderMeasurementsByTime(unsampledMeasurements);
            var measurementTypes = EnumUtilities.GetEnumValues<MeasurementType>();

            foreach (var enumValue in measurementTypes)
            {
                var chronologicalMeasurementsOfType = OrderMeasurementsByType(chronologicalMeasurements, enumValue);
                var sampledMeasurements = _measurementAccumulator.CreateSampledMeasurements(chronologicalMeasurementsOfType);
                PrintMeasurements(enumValue, sampledMeasurements);
            }
        }

        private static void PrintMeasurements(MeasurementType enumValue, IEnumerable<Measurement> sampledMeasurements)
        {
            Console.WriteLine($"Measurement Type: {enumValue.GetDescription()}");
            foreach (var measurement in sampledMeasurements)
            {
                Console.WriteLine($"\t{measurement}");
            }
        }

        private static void Sample(DateTime startOfSampling, IEnumerable<Measurement> unsampledMeasurements)
        {
            var chronologicalMeasurements = OrderMeasurementsByTime(unsampledMeasurements);
            var firstGridPoint = _gridCalculator.GetNextGridPoint(startOfSampling);
        }

        private static Dictionary<MeasurementType, IOrderedEnumerable<Measurement>> OrderMeasurements(IEnumerable<Measurement> unsampledMeasurements)
        {
            var sampledMeasurements = new Dictionary<MeasurementType, IOrderedEnumerable<Measurement>>();
            var availableEnumValues = EnumUtilities.GetEnumValues<MeasurementType>();
            var chronologicalMeasurements = OrderMeasurementsByTime(unsampledMeasurements);

            return sampledMeasurements;
        }

        private static IOrderedEnumerable<Measurement> OrderMeasurementsByTime(IEnumerable<Measurement> unorderedMeasurements)
        {
            return _measurementSorter.SortByTimeAscending(unorderedMeasurements);
        }

        private static IEnumerable<Measurement> OrderMeasurementsByType(IEnumerable<Measurement> unsampledMeasurements, MeasurementType enumValue)
        {
            return _measurementSorter.SortByType(unsampledMeasurements, enumValue);
        }
    }
}