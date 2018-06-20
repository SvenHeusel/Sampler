using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sampler.Utilities;

namespace Sampler.Test.Utilities
{
    [TestClass]
    public class ExtensionTests
    {
        private const string DescriptionText = "There is a description indeed";

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetDescription_NonEnumType_ShouldThrowInvalidOperationException()
        {
            var dummyClass = new DummyClass();
            dummyClass.GetDescription();
        }

        [TestMethod]
        public void GetDescription_ShouldReturnEnumNameForMissingDescription()
        {
            var description = TestEnum.ValueWithoutDescription.GetDescription();
            Assert.AreEqual(TestEnum.ValueWithoutDescription.ToString(), description);
        }

        [TestMethod]
        public void GetDescription_ShouldReturnDescriptionIfExists()
        {
            var description = TestEnum.ValueWithDescription.GetDescription();
            Assert.AreEqual(DescriptionText, description);
        }

        private enum TestEnum
        {
            [System.ComponentModel.Description(DescriptionText)]
            ValueWithDescription,

            ValueWithoutDescription
        }

        private class DummyClass
        {
        }

        [TestMethod]
        public void SetMinute_ShouldOverwriteExistingMinuteEntry()
        {
            var dateTime = new DateTime(2018, 6, 25, 9, 0, 0);
            const int newMinuteValue = 5;
            var updatedDateTime = dateTime.SetMinute(newMinuteValue);
            Assert.AreEqual(newMinuteValue, updatedDateTime.Minute);
        }

        [TestMethod]
        public void SetSecond_ShouldOverwriteExistingSecondEntry()
        {
            var dateTime = new DateTime(2018, 6, 25, 9, 0, 0);
            const int newSecondValue = 59;
            var updatedDateTime = dateTime.SetSecond(newSecondValue);
            Assert.AreEqual(newSecondValue, updatedDateTime.Second);
        }
    }
}