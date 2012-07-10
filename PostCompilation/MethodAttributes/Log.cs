using PostCompilation.Attributes.LogAttribute;
using PostCompilation.Configuration;
using PostCompilation.Utility;

namespace PostCompilation.MethodAttributes
{
    public sealed class LogAttribute : MethodAttribute
    {
        public string BeforeMethodMessage { get; set; }
        public string AfterMethodMessage { get; set; }

        private readonly Lazy<string> target;
        public string Target
        {
            get { return target.Value; }
            set { target.Value = value; }
        }

        private readonly Lazy<LogLevel> level;
        public LogLevel Level
        {
            get { return level.Value; }
            set { level.Value = value; }
        }



        public LogAttribute()
        {
            BeforeMethodMessage = "Invoked.";
            AfterMethodMessage = "Finished work.";
            target = new Lazy<string>(() => Settings.LogAttributeDefaultTarget);
            level = new Lazy<LogLevel>(() => Settings.LogAttributeDefaultLevel);
        }



        public override void BeforeMethodAction()
        {
            ((Log)this).BeforeMethodAction();
        }

        public override void AfterMethodAction()
        {
            ((Log)this).AfterMethodAction();
        }

        public static explicit operator Log(LogAttribute attribute)
        {
            return new Log
            {
                AfterMethodMessage = attribute.AfterMethodMessage,
                BeforeMethodMessage = attribute.BeforeMethodMessage,
                Level = attribute.Level,
                Target = attribute.Target
            };
        }
    }
}
