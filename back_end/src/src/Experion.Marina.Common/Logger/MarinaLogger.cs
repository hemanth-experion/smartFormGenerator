using NLog;
using System;

namespace Experion.Marina.Common
{
    public class MarinaLogger : ILogger
    {
        private readonly NLog.Logger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarinaLogger"/> class.
        /// </summary>
        /// <param name="loggerType">Type of the logger.</param>
        public MarinaLogger(Type loggerType)
        {
            logger = LogManager.GetLogger(loggerType.FullName);
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(string message, Exception exception)
        {
            logger.Error(exception, message, null);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(string message)
        {
            logger.Fatal(message);
        }

        /// <summary>
        /// Fatals the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(string message, Exception exception)
        {
            logger.Fatal(exception, message, null);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Traces the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Trace(string message)
        {
            logger.Trace(message);
        }

        /// <summary>
        /// Warnings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warning(string message)
        {
            logger.Warn(message);
        }
    }
}