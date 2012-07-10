using Mono.Cecil;
using PostCompiler.Factories;
using System.Collections.Generic;

namespace PostCompiler.Main
{
    static partial class Compiler
    {
        private class HandleTypeArgs
        {
            public TypeDefinition TypeDefinition { get; set; }
            public IEnumerable<string> ClassAttributeNames { get; set; }
            public IEnumerable<string> MethodAttributeNames { get; set; }
        }

        static void HandleType(HandleTypeArgs args)
        {
            Globals.Loggers.File.Trace("Handling {0} type.", args.TypeDefinition.FullName);
            if (args.TypeDefinition.HasMethods)
            {
                if (args.TypeDefinition.HasCustomAttributes)
                {
                    Globals.Loggers.File.Trace("Handling type custom attributes.");
                    for (var i = 0; i < args.TypeDefinition.CustomAttributes.Count; i++ )
                    {
                        var handleArgs = new HandleClassAttributeArgs
                        {
                            ClassAttributeNames = args.ClassAttributeNames,
                            CustomAttribute = args.TypeDefinition.CustomAttributes[i],
                            TypeDefinition = args.TypeDefinition
                        };

                        if (HandleTypeAttribute(handleArgs))
                        {
                            i--;
                        }
                    }
                }
                else
                {
                    Globals.Loggers.File.Trace("Type does not has custom attributes.");
                }

                Globals.Loggers.File.Trace("Handling methods.");
                for (var i = 0; i < args.TypeDefinition.Methods.Count; i++)
                {
                    var handleArgs = new HandleMethodArgs
                    {
                        MethodAttributeNames = args.MethodAttributeNames,
                        MethodDefinition = args.TypeDefinition.Methods[i]
                    };
                    HandleMethod(handleArgs);
                }
            }
            else
            {
                Globals.Loggers.File.Trace("Type does not has methods.");
            }
        }
    }
}
