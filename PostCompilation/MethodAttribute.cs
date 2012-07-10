using System;

namespace PostCompilation
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property,
        Inherited = true, AllowMultiple = false)]
    public abstract class MethodAttribute : BaseAttribute
    {
        public MethodInvocationPoint MethodInvocationPoint { get; set; }

        protected MethodAttribute()
        {
            MethodInvocationPoint = MethodInvocationPoint.MethodBoundary;
        }
    }
}
