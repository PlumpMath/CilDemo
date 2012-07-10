using System;
using NLog;
using PostCompiler.Factories;

namespace PostCompiler.Extensions
{
    static class LoggerExtensions
    {
        public static void Exception(this Logger logger, Exception exception, string title = null, Action<Logger, String> logMethod = null)
        {
            if (logger == null)
            {
                Globals.Loggers.File.Error("[LoggerExtensions].[Exception] is throwing error ([logger] == [null]).");
                throw new ArgumentNullException("logger");
            }

            if (exception == null)
            {
                Globals.Loggers.File.Error("[LoggerExtensions].[Exception] is throwing error ([exception] == [null]).");
                throw new ArgumentNullException("exception");
            }

            if (logMethod == null)
            {
                logMethod = Globals.Loggers.Methods.Error;
            }

            if (title != null)
            {
                logMethod(logger, title);
            }

            var ex = exception;
            for (var level = 1; level <= Globals.ExceptionMaxLevel && ex != null; level++, ex = ex.InnerException)
            {
                logMethod(logger, ex.ToString(level));
            }
        }
    }
}
