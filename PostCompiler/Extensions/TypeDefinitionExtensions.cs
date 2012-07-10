using System;
using Mono.Cecil;
using PostCompiler.Factories;

namespace PostCompiler.Extensions
{
    static class TypeDefinitionExtensions
    {
        public static bool IsInheritorOf(this TypeDefinition inheritorTypeDef, string baseTypeFullName)
        {
            if (inheritorTypeDef == null)
            {
                throw new ArgumentNullException("inheritorTypeDef");
            }

            if (baseTypeFullName == null)
            {
                throw new ArgumentNullException("baseTypeFullName");
            }

            var result = false;

            for (TypeDefinition td = inheritorTypeDef, newTd; td.BaseType != null; td = newTd)
            {
                if ((newTd = td.BaseType.Resolve()).FullName == baseTypeFullName)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool IsInheritorOf(this TypeDefinition inheritorTypeDef, TypeDefinition baseTypeDefinition)
        {
            return IsInheritorOf(inheritorTypeDef, baseTypeDefinition.FullName);
        }

        public static bool IsInheritorOf(this TypeDefinition inheritorTypeDef, Type baseType)
        {
            return IsInheritorOf(inheritorTypeDef, baseType.FullName);
        }

        public static Type GetSystemType(this TypeDefinition typeDefinition)
        {
            var result = Type.GetType(typeDefinition.FullName) ??
                Type.GetType(string.Concat(typeDefinition.FullName, ",", typeDefinition.Module.Assembly.Name.Name));

            if (result == null)
            {
                var message = string.Format("Failed to get System.Type for {0}", typeDefinition.FullName);
                Globals.Loggers.File.Error("[TypeDefinitionExtensions].[GetType] is throwing exception ({0}).", message);
                throw new TypeAccessException(message);
            }

            return result;
        }
    }
}
