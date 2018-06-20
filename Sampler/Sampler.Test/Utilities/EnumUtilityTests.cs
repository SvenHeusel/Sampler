using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sampler.Utilities;

namespace Sampler.Test.Utilities
{
    [TestClass]
    public class EnumUtilityTests
    {
        [TestMethod]
        public void GetEnumValues_ReturnsEntireAmountOfEntries()
        {
            var enumValues = EnumUtilities.GetEnumValues<TestEnum>();
            Assert.AreEqual(4, enumValues.Count());
            Assert.IsTrue(enumValues.Contains(TestEnum.First));
            Assert.IsTrue(enumValues.Contains(TestEnum.Second));
            Assert.IsTrue(enumValues.Contains(TestEnum.Third));
            Assert.IsTrue(enumValues.Contains(TestEnum.Fourth));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetEnumValues_NonEnumType_ShouldThrowInvalidOperationException()
        {
            EnumUtilities.GetEnumValues<DummyClass>();
        }

        private class DummyClass
        {
        }

        private enum TestEnum
        {
            First,
            Second,
            Third,
            Fourth
        }
    }
}