using Mono.Cecil;
using PostCompiler.Factories;
using System.Collections.Generic;
using System.Linq;

namespace PostCompiler.Main
{
    static partial class Compiler
    {
        private class HandleMethodAttributeArgs
        {
            public MethodDefinition MethodDefinition { get; set; }
            public CustomAttribute CustomAttribute { get; set; }
            public IEnumerable<string> MethodAttributeNames { get; set; }
        }

        private static bool HandleMethodAttribute(HandleMethodAttributeArgs args)
        {
            var attributeTypeName = args.CustomAttribute.AttributeType.FullName;
            Globals.Loggers.File.Trace("Handling attribute {0}.", attributeTypeName);

            bool result;
            if (args.MethodAttributeNames.Contains(attributeTypeName))
            {
                var processor = args.MethodDefinition.Body.GetILProcessor();
                var instructionBuilder = new InstructionBuilders.MethodAttributeBase(processor, args.CustomAttribute);
                instructionBuilder.Inject();
                Globals.Loggers.Global.Info("Attribute {0} logic injected into {1} method.",
                    args.CustomAttribute.AttributeType.FullName, args.MethodDefinition.FullName);
                args.MethodDefinition.CustomAttributes.Remove(args.CustomAttribute);
                result = true;
            }
            else
            {
                Globals.Loggers.File.Trace("Attribute is not inheritor of MethodAttributeBase.");
                result = false;
            }
            return result;
        }
    }
}
