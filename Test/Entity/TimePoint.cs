using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingTest.Entity
{
    public class TimePoint
    {

        private DateTime time;

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        private NetworkStatus networkStatus;

        public NetworkStatus NetworkStatus
        {
            get { return networkStatus; }
            set { networkStatus = value; }
        }

        public TimePoint(DateTime time, NetworkStatus networkStatus)
        {
            this.time = time;
            this.networkStatus = networkStatus;
        }

    }
}
