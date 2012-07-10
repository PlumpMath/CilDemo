using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using PostCompiler.Extensions;
using PostCompiler.Factories;

namespace PostCompiler.InstructionBuilders
{
    sealed class MethodAttributeBase : AttributeBase
    {
        public MethodAttributeBase(ILProcessor processor, CustomAttribute attribute)
            : base(processor, attribute)
        {
            var baseTypeDef = TypeDefinition.Module.Types.FirstOrDefault(t => t.FullName == Globals.TypeReferences.MethodAttributeBase.FullName);

            if (baseTypeDef == null || !TypeDefinition.IsInheritorOf(baseTypeDef))
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[MethodAttributeBase] is throwing exception (![TypeDefinition].[IsInheritorOf(baseTypeDefinition)]).");
                throw new ArgumentException("[attribute] is not inheritor of [MethodAttributeBase].");
            }
        }



        public void Inject()
        {
            var attrObj = ConstructAttributeObject<PostCompilation.MethodAttribute>();

            if (attrObj.MethodInvocationPoint == PostCompilation.MethodInvocationPoint.BeforeMethod ||
                attrObj.MethodInvocationPoint == PostCompilation.MethodInvocationPoint.MethodBoundary)
            {
                Processor.InsertBeforeMethod(GetBeforeMethodActionInjection());
            }

            if (attrObj.MethodInvocationPoint == PostCompilation.MethodInvocationPoint.AfterMethod ||
                attrObj.MethodInvocationPoint == PostCompilation.MethodInvocationPoint.MethodBoundary)
            {
                Processor.InsertAfterMethod(GetAfterMethodActionInjection());
            }
        }
    }
}
