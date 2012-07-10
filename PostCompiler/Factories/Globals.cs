using System;
using System.Globalization;
using NLog;
using PostCompilation;

namespace PostCompiler.Factories
{
    static class Globals
    {
        public static class Loggers
        {
            public static class Methods
            {
                public static Action<Logger, string> Trace { get; private set; }
                public static Action<Logger, string> Debug { get; private set; }
                public static Action<Logger, string> Info { get; private set; }
                public static Action<Logger, string> Warn { get; private set; }
                public static Action<Logger, string> Error { get; private set; }
                public static Action<Logger, string> Fatal { get; private set; }



                static Methods()
                {
                    Trace = (l, m) => l.Trace(m);
                    Debug = (l, m) => l.Debug(m);
                    Info = (l, m) => l.Info(m);
                    Warn = (l, m) => l.Warn(m);
                    Error = (l, m) => l.Error(m);
                    Fatal = (l, m) => l.Fatal(m);
                }
            }



            public static Logger File { get; private set; }
            public static Logger Console { get; private set; }
            public static Logger Global { get; private set; }



            static Loggers()
            {
                File = LogManager.GetLogger("File");
                Console = LogManager.GetLogger("Console");
                Global = LogManager.GetLogger("Global");
            }
        }

        public static class TypeReferences
        {
            public static Type AttributeBase { get; private set; }
            public static Type MethodAttributeBase { get; private set; }
            public static Type ClassAttributeBase { get; private set; }

            static TypeReferences()
            {
                AttributeBase = typeof(BaseAttribute);
                MethodAttributeBase = typeof(MethodAttribute);
                ClassAttributeBase = typeof(ClassAttribute);
            }
        }



        public static CultureInfo DefaultCultureInfo { get; private set; }
        public static int ExceptionMaxLevel { get; private set; }



        static Globals()
        {
            DefaultCultureInfo = CultureInfo.InvariantCulture;
            ExceptionMaxLevel = 10;
        }
    }
}
