#region File Header & Copyright Notice
/*
 * Copyright (C) 2006, MOTOROLA, INC. All Rights Reserved.
 * THIS SOURCE CODE IS CONFIDENTIAL AND PROPRIETARY AND MAY NOT BE USED
 * OR DISTRIBUTED WITHOUT THE WRITTEN PERMISSION OF MOTOROLA, INC.
 *
 */
#endregion

using System;
using System.Reflection;
using System.Globalization;

using log4net.Core;
using Log4netLoggerManager = log4net.Core.LoggerManager;
using System.Threading;

namespace cnblogs.Jackson0714.Framework.Logging
{

    public partial class Logger 
    {

        private ILogger logger;

        internal Logger(ILogger logger)
        {
            this.logger = logger;
        }

        public string LoggerName
        {
            get
            {
                return this.logger.Name;
            }
        }

        public void Info(string message)
        {
            logger.Log(null, Level.Info, message, null);
        }
    }
}