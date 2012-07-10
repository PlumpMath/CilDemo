using PostCompilation;
using NLog;
using System;

namespace TargetSample
{
    [PostCompilation.ClassAttributes.Log(Target = "Console", 
        ClassInvocationPoint = ClassInvocationPoint.PublicMethods,
        Level = PostCompilation.Attributes.LogAttribute.LogLevel.Debug,
        BeforeMethodMessage = "Postcompilation type before message.",
        AfterMethodMessage = "Postcompilation type after message.")]
    public class Target
    {
        public void TestA()
        {
            LogManager.GetLogger("Console").Info("TargetSample.Target.TestA is working.");
            TestB();
        }

        [PostCompilation.MethodAttributes.Log(Target = "Console", 
            MethodInvocationPoint = MethodInvocationPoint.MethodBoundary,
            Level = PostCompilation.Attributes.LogAttribute.LogLevel.Trace,
            BeforeMethodMessage = "Postcompilation method before message.",
            AfterMethodMessage = "Postcompilation method after message.")]
        private void TestB()
        {
            LogManager.GetLogger("Console").Info("TargetSample.Target.TestB is working.");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            LogManager.GetLogger("Console").Info("TargetSample.Program.Main is working.");
            new Target().TestA();

            Console.ReadKey();
        }
    }
}
