using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sampler.Processing;

namespace Sampler.Test.Processing
{
    [TestClass]
    public class GridCalculatorTests
    {
        private const int SampleIntervalMinutes = 5;

        [TestMethod]
        public void GetNextGridPoint_ShouldReturnNextFiveMinuteMark()
        {
            var testDateTime = new DateTime(2018, 6, 25, 9, 23, 5);
            var expectedDateTime = new DateTime(2018, 6, 25, 9, 25, 0);

            var gridCalculator = new GridCalculator(SampleIntervalMinutes);
            var actualDateTime = gridCalculator.GetNextGridPoint(testDateTime);
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }

        [TestMethod]
        public void GetNextGridPoint_ShouldReturnSamplingPointIfItMatchesGrid()
        {
            var testDateTime = new DateTime(2018, 6, 25, 9, 25, 0);
            var gridCalculator = new GridCalculator(SampleIntervalMinutes);
            var actualDateTime = gridCalculator.GetNextGridPoint(testDateTime);
            Assert.AreEqual(testDateTime, actualDateTime);
        }

        [TestMethod]
        public void GetNextGridPoint_ShouldHandleHourOverflow()
        {
            var testDateTime = new DateTime(2018, 6, 25, 9, 56, 24);
            var expectedDateTime = new DateTime(2018, 6, 25, 10, 0, 0);
            var gridCalculator = new GridCalculator(SampleIntervalMinutes);
            var actualDateTime = gridCalculator.GetNextGridPoint(testDateTime);
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }

        [TestMethod]
        public void GetNextGridPoint_ShouldHandleDayOverflow()
        {
            var testDateTime = new DateTime(2018, 6, 25, 23, 56, 24);
            var expectedDateTime = new DateTime(2018, 6, 26, 0, 0, 0);
            var gridCalculator = new GridCalculator(SampleIntervalMinutes);
            var actualDateTime = gridCalculator.GetNextGridPoint(testDateTime);
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }

        [TestMethod]
        public void GetNextGridPoint_ShouldHandleMonthOverflow()
        {
            var testDateTime = new DateTime(2018, 6, 30, 23, 56, 24);
            var expectedDateTime = new DateTime(2018, 7, 1, 0, 0, 0);
            var gridCalculator = new GridCalculator(SampleIntervalMinutes);
            var actualDateTime = gridCalculator.GetNextGridPoint(testDateTime);
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }

        [TestMethod]
        public void GetNextGridPoint_ShouldHandleYearOverflow()
        {
            var testDateTime = new DateTime(2018, 12, 31, 23, 56, 24);
            var expectedDateTime = new DateTime(2019, 1, 1, 0, 0, 0);
            var gridCalculator = new GridCalculator(SampleIntervalMinutes);
            var actualDateTime = gridCalculator.GetNextGridPoint(testDateTime);
            Assert.AreEqual(expectedDateTime, actualDateTime);
        }

        [TestMethod]
        public void GetNextGridPoint_WorksWithVaryingIntervals()
        {
            var testDateTime = new DateTime(2018, 6, 25, 9, 23, 56);

            const int fiveMinuteInterval = 5;
            var expectedFiveMinuteDateTime = new DateTime(2018, 6, 25, 9, 25, 0);
            Assert.AreEqual(expectedFiveMinuteDateTime, GetActualDateTime(fiveMinuteInterval, testDateTime));

            const int sixMinuteInterval = 6;
            var expectedSixMinuteDateTime = new DateTime(2018, 6, 25, 9, 24, 0);
            Assert.AreEqual(expectedSixMinuteDateTime, GetActualDateTime(sixMinuteInterval, testDateTime));

            const int sevenMinuteInterval = 7;
            var expectedSevenMinuteDateTime = new DateTime(2018, 6, 25, 9, 28, 0);
            Assert.AreEqual(expectedSevenMinuteDateTime, GetActualDateTime(sevenMinuteInterval, testDateTime));

            const int twentyMinuteInterval = 20;
            var expectedTwentyMinuteDateTime = new DateTime(2018, 6, 25, 9, 40, 0);
            Assert.AreEqual(expectedTwentyMinuteDateTime, GetActualDateTime(twentyMinuteInterval, testDateTime));
        }

        private static DateTime GetActualDateTime(int interval, DateTime testDateTime)
        {
            var fiveMinuteGridCalculator = new GridCalculator(interval);
            var actualDateTime = fiveMinuteGridCalculator.GetNextGridPoint(testDateTime);
            return actualDateTime;
        }
    }
}