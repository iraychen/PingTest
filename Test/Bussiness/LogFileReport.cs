using cnblogs.Jackson0714.Framework.Logging;
using cnblogs.Jackson0714.PingTest.Entity;
using cnblogs.Jackson0714.PingTest.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cnblogs.Jackson0714.PingTest.Bussiness
{
    public class LogFileReport : IExport
    {
        private Logger logger = null;
        public LogFileReport(Logger logger)
        {
            this.logger = logger;
        }
        public void Export(NetworkData networkData)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Report");
            builder.AppendLine(string.Format(MessageConstant.IPAddress, networkData.IPAddress));
            builder.AppendLine(string.Format(MessageConstant.FromTimeToTime, networkData.FromTime, networkData.ToTime));

            if (networkData.TotalDuration.TotalMinutes > 1)
            {
                builder.AppendLine(string.Format(MessageConstant.TotalDuration, (int)networkData.TotalDuration.TotalMinutes, MessageConstant.Mins));
            }
            else
            {
                builder.AppendLine(string.Format(MessageConstant.TotalDuration, (int)networkData.TotalDuration.TotalSeconds, MessageConstant.Second));
            }

            

            if (networkData.OfflineTotalDuration.TotalMinutes > 1)
            {
                builder.AppendLine(string.Format(MessageConstant.OfflineTotalDuration, (int)networkData.OfflineTotalDuration.TotalMinutes, MessageConstant.Mins));
            }
            else
            {
                builder.AppendLine(string.Format(MessageConstant.OfflineTotalDuration, (int)networkData.OfflineTotalDuration.TotalSeconds, MessageConstant.Second));
            }

            if (networkData.OfflineLongestDuration.TotalMinutes > 1)
            {
                builder.AppendLine(string.Format(MessageConstant.OfflineLongestDuration, (int)networkData.OfflineLongestDuration.TotalMinutes, MessageConstant.Mins));
            }
            else
            {
                builder.AppendLine(string.Format(MessageConstant.OfflineLongestDuration, (int)networkData.OfflineLongestDuration.TotalSeconds, MessageConstant.Second));
            }

            if (networkData.OnlineLongestDuration.TotalMinutes > 1)
            {
                builder.AppendLine(string.Format(MessageConstant.OnlineLongestDuration, (int)networkData.OnlineLongestDuration.TotalMinutes, MessageConstant.Mins));
            }
            else
            {
                builder.AppendLine(string.Format(MessageConstant.OnlineLongestDuration, (int)networkData.OnlineLongestDuration.TotalSeconds, MessageConstant.Second));
            }
            builder.AppendLine(string.Format(MessageConstant.OfflineCount, networkData.OfflineCount));

            logger.Info(builder.ToString());
        }

        
    }
}
