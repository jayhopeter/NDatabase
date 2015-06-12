using System;
using NDatabase.Tool;
using log4net;

namespace NDatabase.Tutorial.Logging.log4net
{
    public class Log4NetLogger : ILogger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (ILogger));

        #region Implementation of ILogger

        public void Warning(string message)
        {
            Log.Warn(message);
        }

        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(string message, Exception t)
        {
            Log.Error(message);
            Log.ErrorFormat("Error: {0}, exception: {1}", t.Message, t);
        }

        #endregion
    }
}