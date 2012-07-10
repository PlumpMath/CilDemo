using System;
using System.Linq;
using System.Linq.Expressions;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using PostCompiler.Extensions;
using PostCompiler.Factories;

namespace PostCompiler.InstructionBuilders
{
    abstract class Type : InstructionBuilderBase
    {
        protected System.Type SystemType { get; private set; }
        protected TypeDefinition TypeDefinition { get; private set; }



        protected Type(ILProcessor processor, TypeDefinition definition)
            : base(processor)
        {
            if (definition == null)
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[Type] is throwing exception ([definition] == [null]).");
                throw new ArgumentNullException("definition");
            }

            TypeDefinition = definition;
            SystemType = definition.GetSystemType();
        }

        public Instruction GetActionInvocation(LambdaExpression methodSelector)
        {
            if (methodSelector == null)
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[Type].[GetActionInvocation] is throwing exception ([methodSelector] == [null]).");
                throw new ArgumentNullException("methodSelector");
            }

            var methodName = methodSelector.GetMethodInfo().Name;
            var methodDefinition = TypeDefinition.Methods.FirstOrDefault(m => m.Name == methodName);
            var methodReference = Processor.Body.Method.Module.Import(methodDefinition);

            if (methodDefinition == null)
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[Type].[GetActionInvocation] is throwing exception ([methodDefinition] == [null]).");
                throw new ArgumentException("[methodSelector] has returned non existing method name.");
            }

            var result = Processor.Create(OpCodes.Callvirt, methodReference);
            return result;
        }
    }
}
