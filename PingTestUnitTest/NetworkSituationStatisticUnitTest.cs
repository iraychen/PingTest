using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PingTest;
using PingTest.Entity;

namespace PingTestUnitTest
{
    /// <summary>
    /// Summary description for UnitTest
    /// </summary>
    [TestClass]
    public class NetworkSituationStatisticUnitTest
    {
        List<TimePoint> timePoints;
        public NetworkSituationStatisticUnitTest()
        {
            timePoints = new List<TimePoint>();
            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(1), NetworkStatus.Online));

            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(2), NetworkStatus.Offline));
            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(5), NetworkStatus.Online));

            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(10), NetworkStatus.Offline));
            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(12), NetworkStatus.Online));

            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(15), NetworkStatus.Offline));
            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(20), NetworkStatus.Online));

            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(22), NetworkStatus.Offline));
            timePoints.Add(new TimePoint(DateTime.Now.AddSeconds(30), NetworkStatus.Online));
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void CalculateTotalDurationTestMethod()
        {
            TimeSpan experted = NetworkSituationStatistic.CalculateTotalDuration(timePoints);
            TimeSpan actual = new TimeSpan(0, 0, 29);
            Assert.AreEqual(experted, actual);
        }


        [TestMethod]
        public void CalculateOfflineTotalDurationTestMethod()
        {
            TimeSpan experted = NetworkSituationStatistic.CalculateOfflineTotalDuration(timePoints);
            TimeSpan actual = new TimeSpan(0, 0, 18);
            Assert.AreEqual(experted, actual);

        }

        [TestMethod]
        public void CalculateOfflineTotalCountTestMethod()
        {
            int experted = NetworkSituationStatistic.CalculateOfflineTotalCount(timePoints);
            int actual = 4;
            Assert.AreEqual(experted, actual);
        }

        [TestMethod]
        public void CalculateOfflineLongestDurationTestMethod()
        {
            TimeSpan experted = NetworkSituationStatistic.CalculateOfflineLongestDuration(timePoints);
            TimeSpan actual = new TimeSpan(0, 0, 8);
            Assert.AreEqual(experted, actual);
        }

        [TestMethod]
        public void CalculateOnlineLongestDurationTestMethod()
        {
            TimeSpan experted = NetworkSituationStatistic.CalculateOnlineLongestDuration(timePoints);
            TimeSpan actual = new TimeSpan(0, 0, 5);
            Assert.AreEqual(experted, actual);
        }
    }
}
