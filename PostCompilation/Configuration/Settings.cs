using PostCompilation.Attributes.LogAttribute;

namespace PostCompilation.Configuration
{
    static class Settings
    {
        public const string LogAttributeDefaultTargetDefault = "";
        public const LogLevel LogAttributeDefaultLevelDefault = LogLevel.None;



        private static readonly AppSettingsProperty<string> logAttributeDefaultTarget;
        private static readonly AppSettingsProperty<LogLevel> logAttributeDefaultLevel;



        public static string LogAttributeDefaultTarget { get { return logAttributeDefaultTarget.Value; } }
        public static LogLevel LogAttributeDefaultLevel { get { return logAttributeDefaultLevel.Value; } }


        static Settings()
        {
            logAttributeDefaultTarget = new AppSettingsProperty<string>("LogAttributeDefaultTarget", LogAttributeDefaultTargetDefault);
            logAttributeDefaultLevel = new AppSettingsProperty<LogLevel>("LogAttributeDefaultLevel", LogAttributeDefaultLevelDefault);
        }
    }
}
