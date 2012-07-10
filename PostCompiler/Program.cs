using PostCompiler.Main;

namespace PostCompiler
{
    class Program
    {
        static int Main(string[] args)
        {
            var exitCode = Compiler.Run(args);
            System.Environment.ExitCode = exitCode;
            return exitCode;
        }
    }
}
