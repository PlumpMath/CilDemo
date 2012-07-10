using System;

namespace PostCompilation.Extensions
{
    static class ObjectExtensions
    {
        private static readonly Type nullableType;



        static ObjectExtensions()
        {
            nullableType = typeof(Nullable<>);
        }



        public static bool IsDefault<TValue>(this TValue value)
        {
            var valueType = typeof(TValue);
            var result = (valueType.IsValueType && default(TValue).Equals(value)) || 
                (!valueType.IsValueType && value == null);
            return result;
        }

        public static TValue ChangeType<TValue>(this string source, IFormatProvider format = null)
        {
            var valueType = typeof(TValue);
            TValue result;

            if (valueType.IsEnum)
            {
                result = source.ParseFromName<TValue>();
            }
            else
            {
                if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == nullableType)
                {
                    valueType = valueType.GetGenericArguments()[0];
                }

                if (format == null)
                {
                    result = (TValue)Convert.ChangeType(source, valueType);
                }
                else
                {
                    result = (TValue)Convert.ChangeType(source, valueType, format);
                }
            }

            return result;
        }
    }
}
