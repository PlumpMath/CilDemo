using System;
using System.Linq;

namespace PostCompilation.Extensions
{
    static class StringExtensions
    {
        public static TEnum ParseFromName<TEnum>(this string name)
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("TEnum");
            }

            if (Enum.GetNames(enumType).Contains(name))
            {
                try
                {
                    return (TEnum)Enum.ToObject(enumType, Enum.Parse(enumType, name, false));
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("name", ex);
                }
            }

            throw new ArgumentException("name");
        }
    }
}
