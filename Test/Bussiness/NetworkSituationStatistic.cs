using cnblogs.Jackson0714.PingTest.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cnblogs.Jackson0714.PingTest
{
    public static class NetworkSituationStatistic
    {
        public static TimeSpan CalculateTotalDuration(List<TimePoint> timePoints)
        {
            return timePoints[timePoints.Count-1].Time - timePoints[0].Time;
        }

        public static TimeSpan CalculateOfflineTotalDuration(List<TimePoint> timePoints)
        {
            TimeSpan offlineTotalDuration = new TimeSpan();

            for (int i = 0; i < timePoints.Count-1; i++)
            {
                if (timePoints[i].NetworkStatus == NetworkStatus.Offline && timePoints[i+1].NetworkStatus == NetworkStatus.Online)
                {
                    offlineTotalDuration += timePoints[i + 1].Time - timePoints[i].Time;
                }
            }

            return offlineTotalDuration;
        }

        public static int CalculateOfflineTotalCount(List<TimePoint> timePoints)
        {
            var query = (from timePoint in timePoints
                         where timePoint.NetworkStatus == NetworkStatus.Offline
                         select timePoint).Count();
            return query;
        }

        public static TimeSpan CalculateOfflineLongestDuration(List<TimePoint> timePoints)
        {
            TimeSpan offlineDuration = new TimeSpan();
            List<TimeSpan> offlineDurations = new List<TimeSpan>();

            for (int i = 0; i < timePoints.Count - 1; i++)
            {
                if (timePoints[i].NetworkStatus == NetworkStatus.Offline && timePoints[i + 1].NetworkStatus == NetworkStatus.Online)
                {
                    offlineDuration = timePoints[i + 1].Time - timePoints[i].Time;
                    offlineDurations.Add(offlineDuration);
                }
            }
            var query = offlineDurations.OrderByDescending(t => t).FirstOrDefault();
            return query;
        }

        public static TimeSpan CalculateOnlineLongestDuration(List<TimePoint> timePoints)
        {
            TimeSpan onlineDuration = new TimeSpan();
            List<TimeSpan> onlineDurations = new List<TimeSpan>();

            for (int i = 0; i < timePoints.Count - 1; i++)
            {
                if (timePoints[i].NetworkStatus == NetworkStatus.Online && timePoints[i + 1].NetworkStatus == NetworkStatus.Offline)
                {
                    onlineDuration = timePoints[i + 1].Time - timePoints[i].Time;
                    onlineDurations.Add(onlineDuration);
                }
            }
            var query = onlineDurations.OrderByDescending(t => t).FirstOrDefault();
            return query;
        }

        public static NetworkData Calculate(List<TimePoint> timePoints)
        {
            NetworkData networkData = new NetworkData();

            
            networkData.FromTime = timePoints[0].Time;
            networkData.ToTime = timePoints[timePoints.Count-1].Time;
            networkData.TotalDuration = CalculateTotalDuration(timePoints);
            networkData.OfflineTotalDuration = CalculateOfflineTotalDuration(timePoints);
            networkData.OfflineCount = CalculateOfflineTotalCount(timePoints);
            networkData.OfflineLongestDuration = CalculateOfflineLongestDuration(timePoints);
            networkData.OnlineLongestDuration = CalculateOnlineLongestDuration(timePoints);

            return networkData;
        }
    }
}
