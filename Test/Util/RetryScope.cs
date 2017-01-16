using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

namespace Framework.RetryScope
{
    public static class RetryScope
    {
        public const string RetryScopeSlotName = "RetryScopeSlotName";
        private static LocalDataStoreSlot localDataStoreSlot = Thread.GetNamedDataSlot(RetryScopeSlotName);

        public static void BeginRetry(int retryInterval)
        {
            var slotData = Thread.GetData(localDataStoreSlot) as RetryScopeSlotData;

            if (null == slotData)
            {
                slotData = new RetryScopeSlotData();
                slotData.RetryInterval = retryInterval;
            }
            else
            {
                slotData.RetryInterval = retryInterval;
                slotData.Counter++;
            }

            Thread.SetData(localDataStoreSlot, slotData);
        }

        /// <summary>
        /// Removes this retry scope data from thread slot.
        /// </summary>
        public static void EndRetry(int retryInterval)
        {
            var slotData = Thread.GetData(localDataStoreSlot) as RetryScopeSlotData;
            if (slotData != null)
            {
                slotData.RetryInterval = retryInterval;
                if (slotData.Counter == 0)
                {
                    Thread.SetData(localDataStoreSlot, null);
                }
                else
                {
                    slotData.Counter--;
                }
            }
        }
        public static bool HandleException(Exception exception)
        {
            var slotData = Thread.GetData(localDataStoreSlot) as RetryScopeSlotData;
            if (slotData != null)
            {
                if (null != exception)
                {
                    if (slotData.ShouldContinue)
                    {
                        if (slotData.Counter == 0)
                        {
                            if (slotData.RetryCount == slotData.maximumRetries)
                            {
                                slotData.ShouldContinue = false;
                            }
                            else
                            {
                                slotData.RetryCount++;
                                Thread.Sleep(slotData.RetryInterval);
                            }
                        }
                    }
                }
            }

            return slotData != null ? (slotData.ShouldContinue && slotData.Counter == 0) : false;
        }

        public static bool Continue
        {
            get
            {
                var slotData = Thread.GetData(localDataStoreSlot) as RetryScopeSlotData;
                if (slotData != null)
                {
                    return slotData.ShouldContinue && slotData.Counter == 0;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    public class RetryScopeSlotData
    {
        #region Private Fields

        private bool shouldContinue = true;
        private int retryCount = 0;
        private int counter;

        #endregion

        public readonly int maximumRetries = 3;
        //public readonly int retryInterval = 400;

        private int retryInterval;

        public int RetryInterval
        {
            get { return retryInterval; }
            set { retryInterval = value; }
        }

        public RetryScopeSlotData()
        {
            shouldContinue = true;
            retryCount = 0;
            counter = 0;
        }

        public bool ShouldContinue
        {
            get
            {
                return shouldContinue;
            }
            set
            {
                shouldContinue = value;
            }
        }

        public int Counter
        {
            get
            {
                return counter;
            }
            set
            {
                counter = value;
            }
        }

        public int RetryCount
        {
            get
            {
                return retryCount;
            }
            set
            {
                retryCount = value;
            }
        }
    }
}
