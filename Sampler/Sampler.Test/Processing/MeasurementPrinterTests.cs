using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sampler.Container;
using Sampler.Contracts;
using Sampler.Enums;
using Sampler.Processing;
using Sampler.Utilities;

namespace Sampler.Test.Processing
{
    [TestClass]
    public class MeasurementPrinterTests
    {
        private readonly Mock<IPrinter> _printerMock;
        internal const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        public MeasurementPrinterTests()
        {
            _printerMock = new Mock<IPrinter>();
        }

        [TestMethod]
        public void PrintMeasurementsByType_ImplementedCorrectly()
        {
            var heartRateMeasurement = new Measurement(DateTime.Now, 0d, MeasurementType.HeartRate);
            var temperatureMeasurement = new Measurement(DateTime.Now, 0d, MeasurementType.Temperature);
            var mappedMeasurements = new Dictionary<MeasurementType, IEnumerable<Measurement>>()
            {
                {MeasurementType.HeartRate, new List<Measurement>(){heartRateMeasurement} },
                {MeasurementType.Temperature, new List<Measurement>() {temperatureMeasurement}}
            };

            var measurementPrinter = new MeasurementPrinter(_printerMock.Object);
            measurementPrinter.PrintMeasurementsByMeasurementType(mappedMeasurements);

            _printerMock.Verify(printer => printer.Print(It.IsAny<string>()), Times.Exactly(4));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(MeasurementType.HeartRate.GetDescription()))));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(heartRateMeasurement.ToString()))));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(MeasurementType.Temperature.GetDescription()))));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(temperatureMeasurement.ToString()))));
        }

        [TestMethod]
        public void PrintMeasurementsByMeasurementTime_ImplementedCorrectly()
        {
            var earlierTime = new DateTime(2018, 6, 25, 9, 30, 0);
            var laterTime = new DateTime(2018, 6, 25, 9, 30, 5);
            var heartRateMeasurement = new Measurement(earlierTime, 0d, MeasurementType.HeartRate);
            var spo2Measurement = new Measurement(earlierTime, 0d, MeasurementType.SpO2);
            var temperatureMeasurement = new Measurement(laterTime, 0d, MeasurementType.Temperature);
            var mappedMeasurements = new Dictionary<MeasurementType, IEnumerable<Measurement>>()
            {
                {MeasurementType.HeartRate, new List<Measurement>(){heartRateMeasurement} },
                {MeasurementType.Temperature, new List<Measurement>() {temperatureMeasurement}},
                {MeasurementType.SpO2, new List<Measurement>() {spo2Measurement}}
            };

            var measurementPrinter = new MeasurementPrinter(_printerMock.Object);
            measurementPrinter.PrintMeasurementsByMeasurementTime(mappedMeasurements);

            _printerMock.Verify(printer => printer.Print(It.IsAny<string>()), Times.Exactly(5));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(earlierTime.ToString(DateTimeFormat)))));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(heartRateMeasurement.ToString()))));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(spo2Measurement.ToString()))));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(laterTime.ToString(DateTimeFormat)))));
            _printerMock.Verify(printer => printer.Print(It.Is<string>(output => output.Contains(temperatureMeasurement.ToString()))));
        }
    }
}