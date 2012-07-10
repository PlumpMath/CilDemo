using System;
using System.Linq;
using PostCompiler.Factories;

namespace PostCompiler.Extensions
{
    static class EnumExtensions
    {
        private static readonly Type flagsAttributeType;



        static EnumExtensions()
        {
            flagsAttributeType = typeof(FlagsAttribute);
        }



        public static bool IsContainsFlag<TEnum>(this TEnum value, TEnum flag)
            where TEnum : struct
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
            {
                Globals.Loggers.File.Error("[EnumExtensions].[IsContainsFlag] is throwing exception (!typeof(TEnum).IsEnum).");
                throw new ArgumentException("TEnum is not of enum type.");
            }

            var flagsAttribute = type.GetCustomAttributes(flagsAttributeType, false).FirstOrDefault();
            if (flagsAttribute == null)
            {
                Globals.Loggers.File.Error("[EnumExtensions].[IsContainsFlag] is throwing exception ([flagsAttribute] == [null]).");
                throw new ArgumentException("TEnum doesn't has FlagsAttribute.");
            }

            var longValue = Convert.ToInt64(value);
            var longFlag = Convert.ToInt64(flag);

            return longValue == (longValue | longFlag);
        }
    }
}
