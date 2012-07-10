using System;
using PostCompiler.Factories;

namespace PostCompiler.Extensions
{
    static class ExceptionExtensions
    {
        public static string ToString(this Exception exception, int? level = null )
        {
            if (exception == null)
            {
                Globals.Loggers.File.Error("[ExceptionExtensions].[ToString] is throwing error (exception == null).");
                throw new ArgumentNullException("exception");
            }

            const string errorFormatWithLevel = "Level: '{0}'; Message: '{1}'; Source: '{2}'; Stack trace: '{3}';",
                errorFormatWithoutLevel = "Message: '{0}'; Source: '{1}'; Stack trace: '{2}';",
                returnValue = "\r\n",
                returnReplacement = "",
                returnJoint = " ! ";

            var message = exception.Message == null ? string.Empty : exception.Message.Replace(returnValue, returnReplacement);
            var source = exception.Source == null ? string.Empty : exception.Source.Replace(returnValue, returnReplacement);
            var stackTrace = exception.StackTrace != null ?
                string.Join(returnJoint, exception.StackTrace.Split(new[] { returnValue }, StringSplitOptions.RemoveEmptyEntries)) :
                string.Empty;

            var result = level.HasValue ? 
                string.Format(errorFormatWithLevel, level.Value, message, source, stackTrace) : 
                string.Format(errorFormatWithoutLevel, message, source, stackTrace);

            return result;
        }
    }
}
