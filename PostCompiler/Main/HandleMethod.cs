using System;
using Mono.Cecil;
using PostCompiler.Factories;
using System.Collections.Generic;

namespace PostCompiler.Main
{
    static partial class Compiler
    {
        private class HandleMethodArgs
        {
            public MethodDefinition MethodDefinition { get; set; }
            public IEnumerable<string> MethodAttributeNames { get; set; }
        }

        private static void HandleMethod(HandleMethodArgs args)
        {
            Globals.Loggers.File.Debug("Handling method {0}.", args.MethodDefinition.FullName);
            if (args.MethodDefinition.HasCustomAttributes)
            {
                Globals.Loggers.File.Trace("Handling method custom attributes.");
                for (var i = 0; i < args.MethodDefinition.CustomAttributes.Count; i++ )
                {
                    var attribute = args.MethodDefinition.CustomAttributes[i];
                    var handleAttributeArgs = new HandleMethodAttributeArgs
                    {
                        CustomAttribute = attribute,
                        MethodAttributeNames = args.MethodAttributeNames,
                        MethodDefinition = args.MethodDefinition
                    };

                    if (HandleMethodAttribute(handleAttributeArgs))
                    {
                        i--;
                    }
                }
            }
            else
            {
                Globals.Loggers.File.Trace("Method does not has custom attributes.");
            }
        }
    }
}
