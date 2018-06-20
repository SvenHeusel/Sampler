using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sampler.Container;
using Sampler.Enums;
using Sampler.Processing;

namespace Sampler.Test.Processing
{
    [TestClass]
    public class MeasurementSorterTests
    {
        [TestMethod]
        public void SortByTimeAscending_ShouldMaintainOrderForChronologicalMeasurements()
        {
            var earlierInstant = DateTime.Now;
            var laterInstant = earlierInstant.AddMinutes(1);
            Assert.IsTrue(earlierInstant < laterInstant);

            var earlierMeasurement = new Measurement(earlierInstant, 0d, MeasurementType.HeartRate);
            var laterMeasurement = new Measurement(laterInstant, 0d, MeasurementType.HeartRate);

            var unsortedMeasurements = new List<Measurement>() { earlierMeasurement, laterMeasurement };

            var measurementSorter = new MeasurementSorter();
            var orderedMeasurements = measurementSorter.SortByTimeAscending(unsortedMeasurements);
            Assert.AreEqual(earlierMeasurement, orderedMeasurements.ElementAt(0));
            Assert.AreEqual(laterMeasurement, orderedMeasurements.ElementAt(1));
        }

        [TestMethod]
        public void SortByTimeAscending_ShouldReverseOrderForChronologicallyFlippedMeasurements()
        {
            var earlierInstant = DateTime.Now;
            var laterInstant = earlierInstant.AddMinutes(1);

            Assert.IsTrue(earlierInstant < laterInstant);
            var earlierMeasurement = new Measurement(earlierInstant, 0d, MeasurementType.HeartRate);
            var laterMeasurement = new Measurement(laterInstant, 0d, MeasurementType.HeartRate);

            var unsortedMeasurements = new List<Measurement>() { laterMeasurement, earlierMeasurement };

            var measurementSorter = new MeasurementSorter();
            var orderedMeasurements = measurementSorter.SortByTimeAscending(unsortedMeasurements);
            Assert.AreEqual(earlierMeasurement, orderedMeasurements.ElementAt(0));
            Assert.AreEqual(laterMeasurement, orderedMeasurements.ElementAt(1));
        }

        [TestMethod]
        public void SortByType_ShouldReturnOnlyMeasurementsOfDesiredType()
        {
            var instant = DateTime.Now;
            var heartRateMeasurement = new Measurement(instant, 0d, MeasurementType.HeartRate);
            var temperatureMeasurement = new Measurement(instant, 0d, MeasurementType.Temperature);
            var spo2Measurement = new Measurement(instant, 0d, MeasurementType.SpO2);

            var measurementList = new List<Measurement>() { heartRateMeasurement, temperatureMeasurement, spo2Measurement };

            var measurementSorter = new MeasurementSorter();
            var temperatureMeasurements = measurementSorter.SortByType(measurementList, MeasurementType.HeartRate);
            Assert.IsTrue(temperatureMeasurements.All(measurement => measurement.Type == MeasurementType.HeartRate));
        }
    }
}