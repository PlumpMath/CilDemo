using System;
using System.Linq;
using System.Linq.Expressions;
using Mono.Cecil;
using Mono.Cecil.Cil;
using PostCompiler.Extensions;
using PostCompiler.Factories;
using Mono.Collections.Generic;

namespace PostCompiler.InstructionBuilders
{
    abstract class AttributeBase : Attribute
    {
        protected AttributeBase(ILProcessor processor, CustomAttribute attribute)
            : base(processor, attribute)
        {
            var baseTypeDef = TypeDefinition.Module.Types.FirstOrDefault(t => t.FullName == Globals.TypeReferences.AttributeBase.FullName);

            if (baseTypeDef == null || !TypeDefinition.IsInheritorOf(baseTypeDef))
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[AttributeBase] is throwing exception (![TypeDefinition].[IsInheritorOf(baseTypeDef)]).");
                throw new ArgumentException("[attribute] is not inheritor of [AttributeBase].");
            }
        }



        private Instruction GetBeforeMethodActionInvocation()
        {
            Expression<Action<PostCompilation.BaseAttribute>> methodSelector = a => a.BeforeMethodAction();
            return GetActionInvocation(methodSelector);
        }

        private Instruction GetAfterMethodActionInvocation()
        {
            Expression<Action<PostCompilation.BaseAttribute>> methodSelector = a => a.AfterMethodAction();
            return GetActionInvocation(methodSelector);
        }

        protected Collection<Instruction> GetAfterMethodActionInjection()
        {
            var result = GetConstructor();
            result.Add(GetAfterMethodActionInvocation());
            return result;
        }

        protected Collection<Instruction> GetBeforeMethodActionInjection()
        {
            var result = GetConstructor();
            result.Add(GetBeforeMethodActionInvocation());
            return result;
        }
    }
}
