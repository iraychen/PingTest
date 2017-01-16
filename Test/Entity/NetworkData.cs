using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cnblogs.Jackson0714.PingTest.Entity
{
    public class NetworkData
    {
        private string ipAddress;

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        private DateTime fromTime;

        public DateTime FromTime
        {
            get { return fromTime; }
            set { fromTime = value; }
        }

        private DateTime toTime;

        public DateTime ToTime
        {
            get { return toTime; }
            set { toTime = value; }
        }


        private TimeSpan totalDuration;

        public TimeSpan TotalDuration
        {
            get { return totalDuration; }
            set { totalDuration = value; }
        }


        private TimeSpan offlineTotalDuration;

        public TimeSpan OfflineTotalDuration
        {
            get { return offlineTotalDuration; }
            set { offlineTotalDuration = value; }
        }

        private int offlineCount;

        public int OfflineCount
        {
            get { return offlineCount; }
            set { offlineCount = value; }
        }

        private TimeSpan offlineLongestDuration;

        public TimeSpan OfflineLongestDuration
        {
            get { return offlineLongestDuration; }
            set { offlineLongestDuration = value; }
        }

        private TimeSpan onlineLongestDuration;

        public TimeSpan OnlineLongestDuration
        {
            get { return onlineLongestDuration; }
            set { onlineLongestDuration = value; }
        }

    }
}
