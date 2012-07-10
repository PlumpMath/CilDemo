using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using PostCompiler.Factories;
using System.Linq;
using PostCompiler.Extensions;

namespace PostCompiler.InstructionBuilders
{
    abstract class Attribute : Type
    {
        private static readonly string boolTypeName;
        private static readonly string byteTypeName;
        private static readonly string sbyteTypeName;

        private static readonly string charTypeName;
        private static readonly string ushortTypeName;
        private static readonly string shortTypeName;
        private static readonly string uintTypeName;
        private static readonly string intTypeName;

        private static readonly string ulongTypeName;
        private static readonly string longTypeName;

        private static readonly string floatTypeName;
        private static readonly string doubleTypeName;
        private static readonly string stringTypeName;
        private static readonly string enumBaseTypeName;



        protected CustomAttribute CustomAttribute { get; private set; }



        protected Attribute(ILProcessor processor, CustomAttribute attribute)
            : base(processor, attribute.AttributeType.Resolve())
        {
            if(attribute == null)
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[Attribute] is throwing exception ([attribute] == [null]).");
                throw new ArgumentNullException("attribute");
            }

            CustomAttribute = attribute;
        }

        static Attribute()
        {
            boolTypeName = typeof(bool).FullName;
            byteTypeName = typeof(byte).FullName;
            sbyteTypeName = typeof(sbyte).FullName;

            charTypeName = typeof(char).FullName;
            ushortTypeName = typeof(ushort).FullName;
            shortTypeName = typeof(short).FullName;
            uintTypeName = typeof(uint).FullName;
            intTypeName = typeof(int).FullName;

            ulongTypeName = typeof(ulong).FullName;
            longTypeName = typeof(long).FullName;

            floatTypeName = typeof(float).FullName;
            doubleTypeName = typeof(double).FullName;
            stringTypeName = typeof(string).FullName;

            enumBaseTypeName = typeof(Enum).FullName;
        }


        protected TAttribute ConstructAttributeObject<TAttribute>()
            where TAttribute : PostCompilation.BaseAttribute
        {
            if (TypeDefinition.IsAbstract)
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[Attribute].[ConstructAttributeObject] is throwing exception ([TypeDefinition].[IsAbstract]).");
                throw new InvalidOperationException("TypeDefinition is of abstract type.");
            }

            var attrCtorArgTypes = CustomAttribute.HasConstructorArguments ?
                CustomAttribute.ConstructorArguments.Select(a => a.Type.Resolve().GetSystemType()).ToArray() :
                new System.Type[0];
            var attrCtorArgValues = CustomAttribute.HasConstructorArguments ?
                CustomAttribute.ConstructorArguments.Select(a => a.Value).ToArray() :
                new object[0];
            var ctor = SystemType.GetConstructor(attrCtorArgTypes);
            var result = ctor.Invoke(attrCtorArgValues);
            var type = result.GetType();

            if (CustomAttribute.HasProperties)
            {
                foreach (var propertyDef in CustomAttribute.Properties)
                {
                    var propertyInfo = type.GetProperty(propertyDef.Name);
                    propertyInfo.SetValue(result, propertyDef.Argument.Value, null);
                }
            }

            return (TAttribute)result;
        }

        private Instruction GetValueLoad(TypeDefinition valueTypeDef, object value)
        {
            var valueTypeName = valueTypeDef.BaseType != null && valueTypeDef.BaseType.FullName == enumBaseTypeName ?
                        valueTypeDef.Fields[0].FieldType.FullName : valueTypeDef.FullName;

            Instruction instruction;
            if (valueTypeName == boolTypeName || valueTypeName == byteTypeName || valueTypeName == sbyteTypeName)
            {
                instruction = Processor.Create(OpCodes.Ldc_I4_S, Convert.ToSByte(value));
            }
            else if (valueTypeName == charTypeName || valueTypeName == ushortTypeName || valueTypeName == shortTypeName ||
                valueTypeName == uintTypeName || valueTypeName == intTypeName)
            {
                instruction = Processor.Create(OpCodes.Ldc_I4, Convert.ToInt32(value));
            }
            else if (valueTypeName == ulongTypeName || valueTypeName == longTypeName)
            {
                instruction = Processor.Create(OpCodes.Ldc_I8, Convert.ToInt64(value));
            }
            else if (valueTypeName == floatTypeName)
            {
                instruction = Processor.Create(OpCodes.Ldc_R4, Convert.ToSingle(value));
            }
            else if (valueTypeName == doubleTypeName)
            {
                instruction = Processor.Create(OpCodes.Ldc_R8, Convert.ToDouble(value));
            }
            else if (valueTypeName == stringTypeName)
            {
                instruction = Processor.Create(OpCodes.Ldstr, Convert.ToString(value));
            }
            else
            {
                Globals.Loggers.File.Error("[InstructionBuilders].[Attribute].[GetValueLoad] is throwing exception ([valueTypeName] is unknown).");
                throw new NotImplementedException("Implemented only for using primitive types and enum.");
            }
            return instruction;
        }

        public Collection<Instruction> GetConstructor()
        {
            var result = new Collection<Instruction>();

            if (CustomAttribute.HasConstructorArguments)
            {
                foreach (var arg in CustomAttribute.ConstructorArguments)
                {
                    result.Add(GetValueLoad(arg.Type.Resolve(), arg.Value));
                }
            }

            result.Add(Processor.Create(OpCodes.Newobj, CustomAttribute.Constructor));

            if (TypeDefinition.HasProperties && CustomAttribute.HasProperties)
            {
                var propertyNames = CustomAttribute.Properties.Select(p => p.Name).ToArray();
                foreach (var propertyDef in TypeDefinition.Properties.Where(p =>  propertyNames.Contains(p.Name) && p.SetMethod.IsPublic))
                {
                    var property = CustomAttribute.Properties.First(p => p.Name == propertyDef.Name);
                    var setMethodRef = Processor.Body.Method.Module.Import(propertyDef.SetMethod);

                    result.Add(Processor.Create(OpCodes.Dup));
                    result.Add(GetValueLoad(property.Argument.Type.Resolve(), property.Argument.Value));
                    result.Add(Processor.Create(OpCodes.Callvirt, setMethodRef));
                }
            }

            return result;
        }
    }
}
