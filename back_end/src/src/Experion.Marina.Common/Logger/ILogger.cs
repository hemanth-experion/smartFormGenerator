using System;

namespace Experion.Marina.Common
{
    public interface ILogger
    {
        void Debug(string message);

        void Error(string message);

        void Error(string message, Exception exception);

        void Fatal(string message);

        void Fatal(string message, Exception exception);

        void Info(string message);

        void Trace(string message);

        void Warning(string message);
    }
}