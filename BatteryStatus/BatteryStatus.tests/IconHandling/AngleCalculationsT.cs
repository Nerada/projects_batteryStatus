// -----------------------------------------------
//     Author: Ramon Bollen
//      File: BatteryStatus.tests.AngleCalculationsT.cs
// Created on: 20201207
// -----------------------------------------------

using BatteryStatus.Exceptions;
using BatteryStatus.IconHandling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BatteryStatus.tests.IconHandling
{
    [TestClass]
    public class AngleCalculationsT
    {
        private       AngleCalculations _angleCalculations = new AngleCalculations();
        private const double            Delta              = 0.0001;

        [TestInitialize]
        public void Initializer()
        {
            _angleCalculations = new AngleCalculations();
        }

        [TestMethod]
        public void TestNoPercentageGiven()
        {
            Assert.AreEqual(_angleCalculations.End,         0.0F,                     Delta);
            Assert.AreEqual(_angleCalculations.Start2,      _angleCalculations.Start, Delta);
            Assert.AreEqual(_angleCalculations.End2(false), 360.0F,                   Delta);
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyOutOfRangeException))]
        public void TestInvalidLowPercentage()
        {
            _angleCalculations.Percentage = -1;
        }

        [TestMethod]
        [ExpectedException(typeof(PropertyOutOfRangeException))]
        public void TestInvalidHighPercentage()
        {
            _angleCalculations.Percentage = 101;
        }

        [TestMethod]
        public void TestValidScenario()
        {
            float percentage = 50;
            _angleCalculations.Percentage = percentage;

            Assert.AreEqual(_angleCalculations.End,         (360F                                    / 100) * percentage, Delta);
            Assert.AreEqual(_angleCalculations.Start2,      _angleCalculations.Start + ((360F / 100) * percentage),       Delta);
            Assert.AreEqual(_angleCalculations.End2(false), 360F                     - ((360F / 100) * percentage),       Delta);
        }
    }
}