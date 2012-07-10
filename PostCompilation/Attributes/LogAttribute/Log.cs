using NLog;
using PostCompilation.Configuration;

namespace PostCompilation.Attributes.LogAttribute
{
    public class Log
    {
        public string BeforeMethodMessage { get; set; }
        public string AfterMethodMessage { get; set; }
        public string Target { get; set; }
        public LogLevel Level { get; set; }



        private void LogMessage(string message)
        {
            if (Target != Settings.LogAttributeDefaultTargetDefault &&
                Level != Settings.LogAttributeDefaultLevelDefault)
            {
                var logger = LogManager.GetLogger(Target);

                switch (Level)
                {
                    case LogLevel.Trace:
                        logger.Trace(message);
                        break;

                    case LogLevel.Debug:
                        logger.Debug(message);
                        break;

                    case LogLevel.Info:
                        logger.Info(message);
                        break;

                    case LogLevel.Warn:
                        logger.Warn(message);
                        break;

                    case LogLevel.Error:
                        logger.Error(message);
                        break;

                    case LogLevel.Fatal:
                        logger.Fatal(message);
                        break;
                }
            }
        }

        public void BeforeMethodAction()
        {
            LogMessage(BeforeMethodMessage);
        }

        public void AfterMethodAction()
        {
            LogMessage(AfterMethodMessage);
        }
    }
}
