using Mono.Cecil;
using PostCompiler.Factories;
using System.Collections.Generic;
using System.Linq;

namespace PostCompiler.Main
{
    static partial class Compiler
    {
        private class HandleClassAttributeArgs
        {
            public TypeDefinition TypeDefinition { get; set; }
            public CustomAttribute CustomAttribute { get; set; }
            public IEnumerable<string> ClassAttributeNames { get; set; }
        }

        private static bool HandleTypeAttribute(HandleClassAttributeArgs args)
        {
            var attributeTypeName = args.CustomAttribute.AttributeType.FullName;
            Globals.Loggers.File.Trace("Handling attribute {0}.", attributeTypeName);

            bool result = false;
            if (args.ClassAttributeNames.Contains(attributeTypeName))
            {
                foreach (var methodDef in args.TypeDefinition.Methods)
                {
                    Globals.Loggers.File.Debug("Handling method {0}.", methodDef.Name);

                    var processor = methodDef.Body.GetILProcessor();
                    var instructionBuilder = new InstructionBuilders.ClassAttributeBase(processor, args.CustomAttribute);
                    if (instructionBuilder.TryInject())
                    {
                        Globals.Loggers.Global.Info("Attribute {0} logic injected into {1}.{2} method.",
                            args.CustomAttribute.AttributeType.FullName, args.TypeDefinition.FullName, methodDef.Name);
                    }
                }
                args.TypeDefinition.CustomAttributes.Remove(args.CustomAttribute);
                result = true;
            }
            else
            {
                Globals.Loggers.File.Trace("Attribute is not inheritor of ClassAttributeBase.");
                result = false;
            }
            return result;
        }
    }
}
