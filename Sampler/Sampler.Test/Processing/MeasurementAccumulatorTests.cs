using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sampler.Container;
using Sampler.Contracts;
using Sampler.Enums;
using Sampler.Processing;

namespace Sampler.Test.Processing
{
    [TestClass]
    public class MeasurementAccumulatorTests
    {
        private const MeasurementType HeartRateType = MeasurementType.HeartRate;
        private const MeasurementType Spo2Type = MeasurementType.SpO2;

        private const int SamplingIntervalMinutes = 5;
        private readonly IConfigurationStorage _configurationStorage;

        public MeasurementAccumulatorTests()
        {
            var configurationStorageMock = new Mock<IConfigurationStorage>();
            configurationStorageMock.Setup(storage => storage.SamplingGridMinutes).Returns(SamplingIntervalMinutes);
            _configurationStorage = configurationStorageMock.Object;
        }

        [TestMethod]
        public void CreateSampledMeasurements_SquashesMeasurementsOfSameSamplingPoint()
        {
            var gridCalculator = new GridCalculator(_configurationStorage);

            var firstMeasurementTime = new DateTime(2018, 6, 25, 9, 21, 45);
            var secondMeasurementTime = new DateTime(2018, 6, 25, 9, 23, 12);
            var thirdMeasurementTime = new DateTime(2018, 6, 25, 9, 23, 15);

            var firstMeasurement = new Measurement(firstMeasurementTime, 1d, HeartRateType);
            var secondMeasurement = new Measurement(secondMeasurementTime, 2d, HeartRateType);
            var thirdMeasurement = new Measurement(thirdMeasurementTime, 3d, HeartRateType);

            var measurements = new List<Measurement>() { firstMeasurement, secondMeasurement, thirdMeasurement };

            var expectedMeasurementTime = gridCalculator.GetNextGridPoint(thirdMeasurementTime);
            var expectedMeasurement = new Measurement(expectedMeasurementTime, 3d, HeartRateType);

            var measurementAccumulator = new MeasurementAccumulator(gridCalculator);
            var accumulatedMeasurements = measurementAccumulator.CreateSampledMeasurements(measurements);

            Assert.AreEqual(1, accumulatedMeasurements.Count());
            Assert.AreEqual(expectedMeasurement, accumulatedMeasurements.Single());
        }

        [TestMethod]
        public void CreateSampledMeasurements_ReturnsMultipleMeasurementsForMultipleSamplingPoints()
        {
            var gridCalculator = new GridCalculator(_configurationStorage);

            var firstMeasurementTime = new DateTime(2018, 6, 25, 9, 21, 45);
            var secondMeasurementTime = new DateTime(2018, 6, 25, 9, 27, 12);
            var thirdMeasurementTime = new DateTime(2018, 6, 25, 9, 57, 15);

            const double firstMeasurementValue = 1d;
            const double secondMeasurementValue = 2d;
            const double thirdMeasurementValue = 3d;

            var firstMeasurement = new Measurement(firstMeasurementTime, firstMeasurementValue, HeartRateType);
            var secondMeasurement = new Measurement(secondMeasurementTime, secondMeasurementValue, HeartRateType);
            var thirdMeasurement = new Measurement(thirdMeasurementTime, thirdMeasurementValue, HeartRateType);
            var measurements = new List<Measurement>() { firstMeasurement, secondMeasurement, thirdMeasurement };

            var expectedFirstMeasurementTime = gridCalculator.GetNextGridPoint(firstMeasurementTime);
            var expectedSecondMeasurementTime = gridCalculator.GetNextGridPoint(secondMeasurementTime);
            var expectedThirdMeasurementTime = gridCalculator.GetNextGridPoint(thirdMeasurementTime);

            var expectedFirstMeasurement = new Measurement(expectedFirstMeasurementTime, firstMeasurementValue, HeartRateType);
            var expectedSecondMeasurement = new Measurement(expectedSecondMeasurementTime, secondMeasurementValue, HeartRateType);
            var expectedThirdMeasurement = new Measurement(expectedThirdMeasurementTime, thirdMeasurementValue, HeartRateType);

            var measurementAccumulator = new MeasurementAccumulator(gridCalculator);
            var accumulatedMeasurements = measurementAccumulator.CreateSampledMeasurements(measurements);

            Assert.AreEqual(3, accumulatedMeasurements.Count());
            Assert.AreEqual(expectedFirstMeasurement, accumulatedMeasurements.ElementAt(0));
            Assert.AreEqual(expectedSecondMeasurement, accumulatedMeasurements.ElementAt(1));
            Assert.AreEqual(expectedThirdMeasurement, accumulatedMeasurements.ElementAt(2));
        }

        [TestMethod]
        public void CreateSampledMeasurements_HeartRateIntegrationTest()
        {
            var gridCalculator = new GridCalculator(_configurationStorage);

            var firstMeasurementTime = new DateTime(2017, 1, 3, 10, 2, 1);
            var secondMeasurementTime = new DateTime(2017, 1, 3, 10, 4, 45);
            var thirdMeasurementTime = new DateTime(2017, 1, 3, 10, 9, 7);

            const double firstMeasurementValue = 35.82d;
            const double secondMeasurementValue = 35.79d;
            const double thirdMeasurementValue = 35.01d;

            var firstMeasurement = new Measurement(firstMeasurementTime, firstMeasurementValue, HeartRateType);
            var secondMeasurement = new Measurement(secondMeasurementTime, secondMeasurementValue, HeartRateType);
            var thirdMeasurement = new Measurement(thirdMeasurementTime, thirdMeasurementValue, HeartRateType);

            var measurements = new List<Measurement>() { firstMeasurement, secondMeasurement, thirdMeasurement };

            var expectedFirstMeasurementTime = gridCalculator.GetNextGridPoint(secondMeasurementTime);
            var expectedSecondMeasurementTime = gridCalculator.GetNextGridPoint(thirdMeasurementTime);

            var expectedFirstMeasurement = new Measurement(expectedFirstMeasurementTime, secondMeasurementValue, HeartRateType);
            var expectedSecondMeasurement = new Measurement(expectedSecondMeasurementTime, thirdMeasurementValue, HeartRateType);

            var measurementAccumulator = new MeasurementAccumulator(gridCalculator);
            var accumulatedMeasurements = measurementAccumulator.CreateSampledMeasurements(measurements);

            Assert.AreEqual(2, accumulatedMeasurements.Count());
            Assert.AreEqual(expectedFirstMeasurement, accumulatedMeasurements.ElementAt(0));
            Assert.AreEqual(expectedSecondMeasurement, accumulatedMeasurements.ElementAt(1));
        }

        [TestMethod]
        public void CreateSampledMeasurements_Sp02IntegrationTest()
        {
            var gridCalculator = new GridCalculator(_configurationStorage);

            var firstMeasurementTime = new DateTime(2017, 1, 3, 10, 1, 18);
            var secondMeasurementTime = new DateTime(2017, 1, 3, 10, 3, 43);
            var thirdMeasurementTime = new DateTime(2017, 1, 3, 10, 5, 0);
            var fourthMeasurementTime = new DateTime(2017, 1, 3, 10, 5, 1);

            const double firstMeasurementValue = 98.78d;
            const double secondMeasurementValue = 96.49d;
            const double thirdMeasurementValue = 97.17d;
            const double fourthMeasurementValue = 95.08d;

            var firstMeasurement = new Measurement(firstMeasurementTime, firstMeasurementValue, Spo2Type);
            var secondMeasurement = new Measurement(secondMeasurementTime, secondMeasurementValue, Spo2Type);
            var thirdMeasurement = new Measurement(thirdMeasurementTime, thirdMeasurementValue, Spo2Type);
            var fourthMeasurement = new Measurement(fourthMeasurementTime, fourthMeasurementValue, Spo2Type);

            var measurements = new List<Measurement>() { firstMeasurement, secondMeasurement, thirdMeasurement, fourthMeasurement };

            var expectedFirstMeasurementTime = gridCalculator.GetNextGridPoint(thirdMeasurementTime);
            var expectedSecondMeasurementTime = gridCalculator.GetNextGridPoint(fourthMeasurementTime);

            var expectedFirstMeasurement = new Measurement(expectedFirstMeasurementTime, thirdMeasurementValue, Spo2Type);
            var expectedSecondMeasurement = new Measurement(expectedSecondMeasurementTime, fourthMeasurementValue, Spo2Type);

            var measurementAccumulator = new MeasurementAccumulator(gridCalculator);
            var accumulatedMeasurements = measurementAccumulator.CreateSampledMeasurements(measurements);

            Assert.AreEqual(2, accumulatedMeasurements.Count());
            Assert.AreEqual(expectedFirstMeasurement, accumulatedMeasurements.ElementAt(0));
            Assert.AreEqual(expectedSecondMeasurement, accumulatedMeasurements.ElementAt(1));
        }
    }
}