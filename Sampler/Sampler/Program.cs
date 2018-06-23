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
        private static MeasurementSorter _measurementSorter;
        private static IConfigurationStorage _configurationStorage;
        private static IGridCalculator _gridCalculator;
        private static IMeasurementAccumulator _measurementAccumulator;
        private static IPrinter _printer;
        private static MeasurementPrinter _measurementPrinter;

        private static void Main(string[] args)
        {
            _measurementSorter = new MeasurementSorter();
            _configurationStorage = new ConfigurationStorage();
            _gridCalculator = new GridCalculator(_configurationStorage);
            _measurementAccumulator = new MeasurementAccumulator(_gridCalculator);
            _printer = new ConsolePrinter();
            _measurementPrinter = new MeasurementPrinter(_printer);

            var inputData = GenerateInputData();
            var measurementMap = SampleDataDriven(inputData);

            _measurementPrinter.PrintMeasurementsByMeasurementType(measurementMap);
            //_measurementPrinter.PrintMeasurementsByMeasurementTime(measurementMap);
            Console.ReadLine();
        }

        private static Dictionary<MeasurementType, IEnumerable<Measurement>> SampleDataDriven(IEnumerable<Measurement> unsampledMeasurements)
        {
            var mappedMeasurements = new Dictionary<MeasurementType, IEnumerable<Measurement>>();
            var chronologicalMeasurements = OrderMeasurementsByTime(unsampledMeasurements);
            var measurementTypes = EnumUtilities.GetEnumValues<MeasurementType>();

            foreach (var enumValue in measurementTypes)
            {
                var sampledMeasurements = GetSampledMeasurementsOfType(chronologicalMeasurements, enumValue);
                mappedMeasurements.Add(enumValue, sampledMeasurements);
            }

            return mappedMeasurements;
        }

        private static IEnumerable<Measurement> GetSampledMeasurementsOfType(IOrderedEnumerable<Measurement> chronologicalMeasurements, MeasurementType enumValue)
        {
            var chronologicalMeasurementsOfType = OrderMeasurementsByType(chronologicalMeasurements, enumValue);
            return _measurementAccumulator.CreateSampledMeasurements(chronologicalMeasurementsOfType);
        }

        private static IOrderedEnumerable<Measurement> OrderMeasurementsByTime(IEnumerable<Measurement> unorderedMeasurements)
        {
            return _measurementSorter.SortByTimeAscending(unorderedMeasurements);
        }

        private static IEnumerable<Measurement> OrderMeasurementsByType(IEnumerable<Measurement> unsampledMeasurements, MeasurementType enumValue)
        {
            return _measurementSorter.SortByType(unsampledMeasurements, enumValue);
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
    }
}