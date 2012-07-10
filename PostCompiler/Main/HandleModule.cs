using System.Reflection;
using Mono.Cecil;
using PostCompiler.Factories;
using System.Linq;

namespace PostCompiler.Main
{
    static partial class Compiler
    {
        private class HandleModuleArgs
        {
            public ModuleDefinition ModuleDefinition { get; set; }
        }

        static void HandleModule(HandleModuleArgs args)
        {
            if (args.ModuleDefinition.HasTypes)
            {
                var postCompilationAssembly = Assembly.GetAssembly(Globals.TypeReferences.AttributeBase);
                if (postCompilationAssembly == null)
                {
                    Globals.Loggers.Global.Fatal("PostCompilation Assembly was not found in references.");
                    return;
                }

                var methodAttributeNames = postCompilationAssembly.GetTypes().
                    Where(t => Globals.TypeReferences.MethodAttributeBase.IsAssignableFrom(t) && 
                        t.FullName != Globals.TypeReferences.MethodAttributeBase.FullName).
                    Select(t => t.FullName).ToArray();
                var classAttributeNames = postCompilationAssembly.GetTypes().
                    Where(t => Globals.TypeReferences.ClassAttributeBase.IsAssignableFrom(t) &&
                        t.FullName != Globals.TypeReferences.ClassAttributeBase.FullName).
                    Select(t => t.FullName).ToArray();

                if (methodAttributeNames.Length == 0 || classAttributeNames.Length == 0)
                {
                    Globals.Loggers.Global.Warn("MethodAttributeBase and ClassAttributeBase inheritors were not found.");
                    return;
                }

                Globals.Loggers.File.Debug("Inheritors of MethodAttributeBase were found: {0} types.", methodAttributeNames.Length);
                Globals.Loggers.File.Debug("Inheritors of ClassAttributeBase were found: {0} types.", classAttributeNames.Length);

                Globals.Loggers.File.Trace("Handling types.");
                foreach (var typeDef in args.ModuleDefinition.Types)
                {
                    var handleTypeArgs = new HandleTypeArgs
                    {
                        TypeDefinition = typeDef,
                        MethodAttributeNames = methodAttributeNames,
                        ClassAttributeNames = classAttributeNames
                    };
                    HandleType(handleTypeArgs);
                }
            }
            else
            {
                Globals.Loggers.File.Warn("Module does not has types.");
            }
        }
    }
}
