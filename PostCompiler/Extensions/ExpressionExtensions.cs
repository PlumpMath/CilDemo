using System;
using System.Reflection;
using System.Linq.Expressions;
using PostCompiler.Factories;

namespace PostCompiler.Extensions
{
    static class ExpressionExtensions
    {
        //public static PropertyInfo GetPropertyInfo<TSource, TValue>(this Expression<Func<TSource, TValue>> expression)
        //{
        //    var memberExpression = expression.Body as MemberExpression;
        //    if (memberExpression == null)
        //    {
        //        Globals.Loggers.File.Error("[ExpressionExtensions].[GetPropertyInfo] is throwing error (Failed to cast [expression] to [MemberExpression]).");
        //        throw new InvalidCastException("Failed to cast [expression] to [MemberExpression]");
        //    }

        //    var propertyInfo = memberExpression.Member as PropertyInfo;
        //    if (propertyInfo == null)
        //    {
        //        Globals.Loggers.File.Error("[ExpressionExtensions].[GetPropertyInfo] is throwing error ([expression].[Member] == [null]).");
        //        throw new InvalidOperationException("[expression].[Member] == [null]");
        //    }

        //    var sourceType = typeof(TSource);
        //    if (sourceType != propertyInfo.ReflectedType && sourceType.IsSubclassOf(propertyInfo.ReflectedType))
        //    {
        //        Globals.Loggers.File.Error("[ExpressionExtensions].[GetPropertyInfo] is throwing error ([sourceType] != [propertyInfo].[ReflectedType]).");
        //        throw new InvalidOperationException("[expression] returns wrong type.");
        //    }

        //    return propertyInfo;
        //}

        public static MethodInfo GetMethodInfo(this LambdaExpression expression)
        {
            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                Globals.Loggers.File.Error("[ExpressionExtensions].[GetMethodInfo] is throwing error (Failed to cast [expression] to [MethodCallExpression]).");
                throw new InvalidCastException("Failed to cast [expression] to [MethodCallExpression]");
            }

            var methodInfo = methodCallExpression.Method;
            if (methodInfo == null)
            {
                Globals.Loggers.File.Error("[ExpressionExtensions].[GetMethodInfo] is throwing error ([expression].[Method] == [null]).");
                throw new InvalidOperationException("[expression].[Method] == [null]");
            }

            return methodInfo;
        }
    }
}