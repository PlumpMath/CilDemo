using System;
using System.Diagnostics;
using System.IO;
using Mono.Cecil;
using PostCompiler.Factories;
using PostCompiler.Extensions;

namespace PostCompiler.Main
{
    static partial class Compiler
    {
        public static int Run(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            ErrorCode result;
            Globals.Loggers.File.Info("Program was started.");



            if (args == null || (args.Length != 1 && args.Length != 2) || args[0] == null)
            {
                Globals.Loggers.File.Fatal("Program was called with wrong argumets.");
                Globals.Loggers.Console.Info("Usage: PostCompiler.exe <Input module filename> [Output module filename]");
                result = ErrorCode.WrongArguments;
                goto EndMain;
            }

            if (!File.Exists(args[0]))
            {
                Globals.Loggers.Global.Fatal("File does not exits: {0}.", args[0]);
                result = ErrorCode.FileDoesNotExists;
                goto EndMain;
            }

            var inputFilename = args[0];
            var outputFilename = args.Length == 2 ? args[1] : inputFilename;



            FileStream inputStream = null;
            try
            {
                inputStream = new FileStream(inputFilename, FileMode.Open);
            }
            catch (Exception ex)
            {
                Globals.Loggers.File.Exception(ex, "[FileStream] is throwing exception.", Globals.Loggers.Methods.Fatal);
                Globals.Loggers.Console.Fatal("Unable to create FileStream from file: {0}", inputFilename);
                result = ErrorCode.FileReadError;
                goto EndMain;
            }

            ModuleDefinition modDef;
            try
            {
                modDef = ModuleDefinition.ReadModule(inputStream);
            }
            catch (Exception ex)
            {
                Globals.Loggers.File.Exception(ex, "[ModuleDefinition].[ReadModule] is throwing exception.", Globals.Loggers.Methods.Fatal);
                Globals.Loggers.Console.Fatal("Unable to load ModuleDefinition from FileStream.");
                result = ErrorCode.ModuleReadError;
                goto EndMain;
            }
            finally
            {
                inputStream.Dispose();
            }

            Globals.Loggers.Global.Info("Module {0} loaded.", modDef.FullyQualifiedName);



            var handleArgs = new HandleModuleArgs
            {
                ModuleDefinition = modDef
            };
            HandleModule(handleArgs);



            var outputStream = new MemoryStream();
            try
            {
                modDef.Write(outputStream);
            }
            catch (Exception ex)
            {
                outputStream.Dispose();
                Globals.Loggers.File.Exception(ex, "[ModuleDefinition].[Write] is throwing exception.", Globals.Loggers.Methods.Fatal);
                Globals.Loggers.Console.Fatal("Unable to write ModuleDefinition to stream.");
                result = ErrorCode.ModuleWriteError;
                goto EndMain;
            }

            try
            {
                File.WriteAllBytes(outputFilename, outputStream.ToArray());
            }
            catch (Exception ex)
            {
                Globals.Loggers.File.Exception(ex, "[File].[WriteAllBytes] is throwing exception.", Globals.Loggers.Methods.Fatal);
                Globals.Loggers.Console.Fatal("Unable to write Stream to file: {0}", inputFilename);
                result = ErrorCode.FileWriteError;
                goto EndMain;
            }
            finally
            {
                outputStream.Dispose();
            }



            result = ErrorCode.Success;

            stopwatch.Stop();
            Globals.Loggers.File.Info("Module {0} saved.", modDef.FullyQualifiedName);
            Globals.Loggers.Console.Info("Postcompilation finished.");
            Globals.Loggers.Global.Info("Elapsed time: {0}", stopwatch.Elapsed);

        EndMain:
            Globals.Loggers.File.Info("Program finished work.");
            return (int)result;
        }
    }
}
