using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using PostCompiler.Extensions;
using PostCompiler.Factories;
using PostCompilation;

namespace PostCompiler.InstructionBuilders
{
    sealed class ClassAttributeBase : AttributeBase
    {
        public ClassAttributeBase(ILProcessor processor, CustomAttribute attribute)
            : base(processor, attribute)
        {
            var baseTypeDef = TypeDefinition.Module.Types.FirstOrDefault(t => t.FullName == Globals.TypeReferences.ClassAttributeBase.FullName);

            if (baseTypeDef == null || !TypeDefinition.IsInheritorOf(baseTypeDef))
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[ClassAttributeBase] is throwing exception (![TypeDefinition].[IsInheritorOf(baseTypeDefinition)]).");
                throw new ArgumentException("[attribute] is not inheritor of [ClassAttributeBase].");
            }
        }



        private void Inject(ClassAttribute attribute)
        {
            if (attribute.MethodInvocationPoint == MethodInvocationPoint.BeforeMethod ||
                attribute.MethodInvocationPoint == MethodInvocationPoint.MethodBoundary)
            {
                Processor.InsertBeforeMethod(GetBeforeMethodActionInjection());
            }

            if (attribute.MethodInvocationPoint == MethodInvocationPoint.AfterMethod ||
                attribute.MethodInvocationPoint == MethodInvocationPoint.MethodBoundary)
            {
                Processor.InsertAfterMethod(GetAfterMethodActionInjection());
            }
        }

        public bool TryInject()
        {
            var result = false;
            var attrObj = ConstructAttributeObject<PostCompilation.ClassAttribute>();

            var methodDef = Processor.Body.Method;
            var classAttrs = attrObj.ClassInvocationPoint;

            if (methodDef.IsConstructor)
            {
                if (classAttrs.IsContainsFlag(ClassInvocationPoint.PrivateConstructors) && methodDef.IsPrivate)
                { result = true; }
                else if (classAttrs.IsContainsFlag(ClassInvocationPoint.ProtectedConstructors) && (methodDef.IsFamily || methodDef.IsFamilyAndAssembly))
                { result = true; }
                else if (classAttrs.IsContainsFlag(ClassInvocationPoint.PublicConstructors) && methodDef.IsPublic)
                { result = true; }
            }
            else if (methodDef.IsGetter || methodDef.IsSetter)
            {
                if (classAttrs.IsContainsFlag(ClassInvocationPoint.PrivateProperties) && methodDef.IsPrivate)
                { result = true; }
                else if (classAttrs.IsContainsFlag(ClassInvocationPoint.ProtectedProperties) && (methodDef.IsFamily || methodDef.IsFamilyAndAssembly))
                { result = true; }
                else if (classAttrs.IsContainsFlag(ClassInvocationPoint.PublicProperties) && methodDef.IsPublic)
                { result = true; }
            }
            else
            {
                if (classAttrs.IsContainsFlag(ClassInvocationPoint.PrivateMethods) && methodDef.IsPrivate)
                { result = true; }
                else if (classAttrs.IsContainsFlag(ClassInvocationPoint.ProtectedMethods) && (methodDef.IsFamily || methodDef.IsFamilyAndAssembly))
                { result = true; }
                else if (classAttrs.IsContainsFlag(ClassInvocationPoint.PublicMethods) && methodDef.IsPublic)
                { result = true; }
            }

            if (result)
            {
                Inject(attrObj);
            }

            return result;
        }
    }
}
