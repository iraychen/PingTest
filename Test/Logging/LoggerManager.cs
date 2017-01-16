using System;
using System.IO;
using System.Reflection;

using log4net.Config;
using Log4netLoggerManager = log4net.Core.LoggerManager;
using System.Collections;
using log4net.Core;
using log4net.Repository;
using log4net.Appender;
using System.Linq;

namespace Framework.Logging
{
    /// <summary>
    /// Controls the creation of logger.
    /// </summary>
    public static class LoggerManager
    {
        private static Hashtable cachedLoggers = new Hashtable();

        private const string DefaultRepository = "log4net-default-repository";

        /// <summary>
        /// Static constructor, loads the config file.
        /// </summary>
        static LoggerManager()
        {

            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(dir, LoggingConstants.DefaultConfigFile);
            FileInfo fileInfo = new FileInfo((filePath));

            XmlConfigurator.ConfigureAndWatch(fileInfo);

        }

        public static Logger GetLogger(string loggerName)
        {
            Logger logger = null;
            if (loggerName == null)
            {
                throw new ArgumentNullException(
                    MethodInfo.GetCurrentMethod().GetParameters()[0].Name);
            }

            if (null != cachedLoggers[loggerName])
            {
                logger = (Logger)cachedLoggers[loggerName];
            }
            else
            {
                logger = CreateLogger(loggerName);
            }

            return logger;
        }

        private static Logger CreateLogger(string loggerName)
        {
            Logger logger = new Logger(GetLog4netLogger(loggerName));
            lock (cachedLoggers)
            {
                if (!cachedLoggers.Contains(loggerName))
                {
                    cachedLoggers.Add(loggerName, logger);
                }
            }

            return logger;
        }

        public static ILogger GetLog4netLogger(string loggerName)
        {
            return Log4netLoggerManager.GetLogger(DefaultRepository, loggerName);
        }

        public static void ChangeFileName(string loggerName, string fileName)
        {
            ILoggerRepository repository = Log4netLoggerManager.GetRepository(DefaultRepository);
            IAppender[] appenders = repository.GetAppenders();
            var targetApder = appenders.First(p => p.Name == loggerName) as RollingFileAppender;
            targetApder.File = fileName;
            targetApder.Writer = new StreamWriter(targetApder.File, targetApder.AppendToFile, targetApder.Encoding);
            //targetApder.ActivateOptions();
        }
    }
}
