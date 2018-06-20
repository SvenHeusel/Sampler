using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sampler.Container;
using Sampler.Enums;

namespace Sampler.Test.Container
{
    [TestClass]
    public class MeasurementTests
    {
        #region Equals

        [TestMethod]
        public void Equals_ReferenceEqualObjects_ShouldReturnTrue()
        {
            Measurement measurement, referenceEqualMeasurement;
            GetReferenceEqualMeasurements(out measurement, out referenceEqualMeasurement);
            Assert.IsTrue(Equals(measurement, referenceEqualMeasurement));
        }

        [TestMethod]
        public void Equals_ObjectsWithIdenticalValues_ShouldReturnTrue()
        {
            Measurement firstMeasurement, secondMeasurement;
            GetIdenticalMeasurements(out firstMeasurement, out secondMeasurement);
            Assert.IsTrue(Equals(firstMeasurement, secondMeasurement));
        }

        [TestMethod]
        public void Equals_DifferentMeasurementTime_ShouldReturnFalse()
        {
            Measurement earlierMeasurement, laterMeasurement;
            GetMeasurementsWithTimeDifference(out earlierMeasurement, out laterMeasurement);
            Assert.IsFalse(Equals(earlierMeasurement, laterMeasurement));
        }

        [TestMethod]
        public void Equals_DifferentMeasurementValue_ShouldReturnFalse()
        {
            Measurement lowerMeasurement, higherMeasurement;
            GetMeasurementsWithValueDifference(out lowerMeasurement, out higherMeasurement);
            Assert.IsFalse(Equals(lowerMeasurement, higherMeasurement));
        }

        [TestMethod]
        public void Equals_DifferentTypes_ShouldReturnFalse()
        {
            Measurement heartRateMeasurement, temperatureMeasurement;
            GetMeasurementsWithTypeDifference(out heartRateMeasurement, out temperatureMeasurement);
            Assert.IsFalse(Equals(heartRateMeasurement, temperatureMeasurement));
        }

        #endregion Equals

        #region GetHashCode

        [TestMethod]
        public void GetHashCode_ReferenceEqualObjects_ShouldReturnIdenticalHashCode()
        {
            Measurement measurement, referenceEqualMeasurement;
            GetReferenceEqualMeasurements(out measurement, out referenceEqualMeasurement);
            Assert.AreEqual(measurement.GetHashCode(), referenceEqualMeasurement.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_ObjectsWithIdenticalValues_ShouldReturnIdenticalHashCode()
        {
            Measurement firstMeasurement, secondMeasurement;
            GetIdenticalMeasurements(out firstMeasurement, out secondMeasurement);
            Assert.AreEqual(firstMeasurement.GetHashCode(), secondMeasurement.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_DifferentMeasurementTime_ShouldReturnDifferentHashCode()
        {
            Measurement earliertMeasurement, laterMeasurement;
            GetMeasurementsWithTimeDifference(out earliertMeasurement, out laterMeasurement);
            Assert.AreNotEqual(earliertMeasurement.GetHashCode(), laterMeasurement.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_DifferentMeasurementValue_ShouldReturnDifferentHashCode()
        {
            Measurement lowerMeasurement, higherMeasurement;
            GetMeasurementsWithValueDifference(out lowerMeasurement, out higherMeasurement);
            Assert.AreNotEqual(lowerMeasurement.GetHashCode(), higherMeasurement.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_DifferentMeasurementType_ShoudlReturnDifferentHashCode()
        {
            Measurement heartRateMeasurement, temperatureMeasurement;
            GetMeasurementsWithTypeDifference(out heartRateMeasurement, out temperatureMeasurement);
            Assert.AreNotEqual(heartRateMeasurement.GetHashCode(), temperatureMeasurement.GetHashCode());
        }

        #endregion GetHashCode

        #region ToString

        [TestMethod]
        public void ToString_ShouldPrintDesiredOutputFormat()
        {
            const string expectedStringRepresentation = "{2018-06-25T09:30:00, TEMP, 37.00}";
            var correspondingDateTime = new DateTime(2018, 6, 25, 9, 30, 0);
            var correspondingMeasurement = new Measurement(correspondingDateTime, 37.0d, MeasurementType.Temperature);
            Assert.AreEqual(expectedStringRepresentation, correspondingMeasurement.ToString());
        }

        #endregion ToString

        #region Helper Methods

        private static void GetReferenceEqualMeasurements(out Measurement measurement, out Measurement referenceEqualMeasurement)
        {
            measurement = new Measurement(DateTime.MaxValue, 0d, MeasurementType.HeartRate);
            referenceEqualMeasurement = measurement;
        }

        private static void GetIdenticalMeasurements(out Measurement firstMeasurement, out Measurement secondMeasurement)
        {
            firstMeasurement = new Measurement(DateTime.MaxValue, 0d, MeasurementType.HeartRate);
            secondMeasurement = new Measurement(DateTime.MaxValue, 0d, MeasurementType.HeartRate);
        }

        private static void GetMeasurementsWithTimeDifference(out Measurement earlierMeasurement, out Measurement laterMeasurement)
        {
            earlierMeasurement = new Measurement(DateTime.MinValue, 0d, MeasurementType.HeartRate);
            laterMeasurement = new Measurement(DateTime.MaxValue, 0d, MeasurementType.HeartRate);
        }

        private static void GetMeasurementsWithValueDifference(out Measurement lowerMeasurement, out Measurement higherMeasurement)
        {
            lowerMeasurement = new Measurement(DateTime.MaxValue, 0d, MeasurementType.HeartRate);
            higherMeasurement = new Measurement(DateTime.MaxValue, 1d, MeasurementType.HeartRate);
        }

        private static void GetMeasurementsWithTypeDifference(out Measurement heartRateMeasurement, out Measurement temperatureMeasurement)
        {
            heartRateMeasurement = new Measurement(DateTime.MaxValue, 0d, MeasurementType.HeartRate);
            temperatureMeasurement = new Measurement(DateTime.MaxValue, 0d, MeasurementType.Temperature);
        }

        #endregion Helper Methods
    }
}